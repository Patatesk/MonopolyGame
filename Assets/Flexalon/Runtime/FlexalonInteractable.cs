using UnityEngine;
using UnityEngine.Events;

namespace Flexalon
{
    [AddComponentMenu("Flexalon/Flexalon Interactable"), HelpURL("https://www.flexalon.com/docs/interactable"), DisallowMultipleComponent]
    public class FlexalonInteractable : MonoBehaviour
    {
        // Determines if this object can be clicked and generate click events.
        [SerializeField]
        private bool _clickable = false;
        public bool Clickable {
            get => _clickable;
            set => _clickable = value;
        }

        // With a mouse or touch input, a click is defined as a press and release.
        // The time between press and release must be less than Max Click Time to
        // count as a click. A drag interaction cannot start until Max Click Time is exceeded.
        [SerializeField]
        private float _maxClickTime = 0.1f;
        public float MaxClickTime {
            get => _maxClickTime;
            set => _maxClickTime = value;
        }

        // Determines if this object can be dragged and generate drag events.
        [SerializeField]
        private bool _draggable = false;
        public bool Draggable {
            get => _draggable;
            set => _draggable = value;
        }

        // Determins how quickly the object moves towards the cursor when dragged.
        [SerializeField]
        private float _interpolationSpeed = 10;
        public float InterpolationSpeed {
            get => _interpolationSpeed;
            set => _interpolationSpeed = value;
        }

        public enum RestrictionType
        {
            None,

            // Plane restriction ensures the object moves along a plane, defined
            // by the objects initial position and the Plane Normal property.
            Plane,

            // Line restriction ensures the object moves along a line, defined
            // by the object's initial position and the Line Direction property.
            Line
        }

        // Determines how to restrict the object's drag movement.
        [SerializeField]
        private RestrictionType _restriction = RestrictionType.None;
        public RestrictionType Restriction {
            get => _restriction;
            set => _restriction = value;
        }

        // Defines the normal of the plane when using a plane restriction.
        // If 'Local Space' is checked, this normal is rotated by the transform
        // of the layout that the object started in.
        [SerializeField]
        private Vector3 _planeNormal = Vector3.up;
        public Vector3 PlaneNormal {
            get => _planeNormal;
            set
            {
                _restriction = RestrictionType.Plane;
                _planeNormal = value;
            }
        }

        // Defines the direction of the line when using a line restriction.
        // If 'Local Space'is checked, this direction is rotated by the transform
        // of the layout that the object started in.
        [SerializeField]
        private Vector3 _lineDirection = Vector3.right;
        public Vector3 LineDirection {
            get => _lineDirection;
            set
            {
                _restriction = RestrictionType.Line;
                _lineDirection = value;
            }
        }

        [SerializeField]
        private bool _localSpaceRestriction = true;
        public bool LocalSpaceRestriction {
            get => _localSpaceRestriction;
            set => _localSpaceRestriction = value;
        }

        // When dragged, this option adds an offset to the dragged object's position.
        // This can be used to float the object near the layout while it is being dragged.
        // If 'Local Space' is checked, this offset is rotated and scaled by the transform
        // of the layout that the object started in.
        [SerializeField]
        private Vector3 _holdOffset;
        public Vector3 HoldOffset {
            get => _holdOffset;
            set => _holdOffset = value;
        }

        [SerializeField]
        private bool _localSpaceOffset = true;
        public bool LocalSpaceOffset {
            get => _localSpaceOffset;
            set => _localSpaceOffset = value;
        }

        // When dragged, this option adds a rotation to the dragged object.
        // This can be used to tilt the object while it is being dragged.
        // If 'Local Space' is checked, this rotation will be in the local
        // space of the layout that the object started in.
        [SerializeField]
        private bool _rotateOnDrag = false;
        public bool RotateOnDrag {
            get => _rotateOnDrag;
            set => _rotateOnDrag = value;
        }

        [SerializeField]
        private Quaternion _holdRotation;
        public Quaternion HoldRotation {
            get => _holdRotation;
            set
            {
                _rotateOnDrag = true;
                _holdRotation = value;
            }
        }

        [SerializeField]
        private bool _localSpaceRotation = true;
        public bool LocalSpaceRotation {
            get => _localSpaceRotation;
            set => _localSpaceRotation = value;
        }

        // When checked, Cursor.visible is set to false when the object is dragged.
        [SerializeField]
        private bool _hideCursor = false;
        public bool HideCursor {
            get => _hideCursor;
            set => _hideCursor = value;
        }

        // If set, the object cannot be dragged outside of the bounds collider.
        [SerializeField]
        private Collider _bounds;
        public Collider Bounds {
            get => _bounds;
            set => _bounds = value;
        }

        // When dragged, limits which Flexalon Drag Targets will accept this object
        // by comparing the Layer Mask to the target GameObject's layer.
        [SerializeField]
        public LayerMask _layerMask = -1;
        public LayerMask LayerMask {
            get => _layerMask;
            set => _layerMask = value;
        }

        [System.Serializable]
        public class InteractableEvent : UnityEvent<FlexalonInteractable>{}

        // Unity Event invoked when the object is pressed and released within MaxClickTime.
        [SerializeField]
        private InteractableEvent _clicked;
        public InteractableEvent Clicked => _clicked;

        // Unity Event invoked when the object starts being hovered.
        [SerializeField]
        private InteractableEvent _hoverStart;
        public InteractableEvent HoverStart => _hoverStart;

        // Unity Event invoked when the object stops being hovered.
        [SerializeField]
        private InteractableEvent _hoverEnd;
        public InteractableEvent HoverEnd => _hoverEnd;

        // Unity Event invoked when the object starts being selected (e.g. press down mouse over object).
        [SerializeField]
        private InteractableEvent _selectStart;
        public InteractableEvent SelectStart => _selectStart;

        // Unity Event invoked when the object stops being selected (e.g. release mouse).
        [SerializeField]
        private InteractableEvent _selectEnd;
        public InteractableEvent SelectEnd => _selectEnd;

        // Unity Event invoked when the object starts being dragged.
        [SerializeField]
        private InteractableEvent _dragStart;
        public InteractableEvent DragStart => _dragStart;

        // Unity Event invoked when the object stops being dragged.
        [SerializeField]
        private InteractableEvent _dragEnd;
        public InteractableEvent DragEnd => _dragEnd;

        // The currently selected / dragged object.
        private static FlexalonInteractable _selectedObject;
        public static FlexalonInteractable SelectedObject => _selectedObject;

        // The currently hovered object.
        private static FlexalonInteractable _hoveredObject;
        public static FlexalonInteractable HoveredObject => _hoveredObject;

        private Vector3 _target;
        private Vector3 _lastTarget;
        private float _distance;
        private GameObject _placeholder;
        private Vector3 _startPosition;
        private Plane _plane = new Plane();
        private Transform _localSpace;
        private static Collider[] _colliders = new Collider[10];
        private static Raycaster _raycaster = new Raycaster();
        private float _selectTime;

        public enum InteractableState
        {
            Init,
            Hovering,
            Selecting,
            Dragging
        }

        private InteractableState _state = InteractableState.Init;
        public InteractableState State => _state;

        // Singleton object to perform one raycast per frame for all interactables.
        private class Raycaster
        {
            private RaycastHit[] _raycastHits = new RaycastHit[10];
            private int _raycastFrame = 0;
            private FlexalonInteractable _hitInteractable;

            public bool IsHit(Ray ray, FlexalonInteractable interactable)
            {
                // Check if we've already casted this frame.
                if (_raycastFrame != Time.frameCount)
                {
                    _hitInteractable = null;
                    _raycastFrame = Time.frameCount;
                    int hits = Physics.RaycastNonAlloc(ray, _raycastHits, 1000);
                    float minDistance = float.MaxValue;

                    // Find the nearest hit interactable.
                    for (int i = 0; i < hits; i++)
                    {
                        var hit = _raycastHits[i];
                        if (hit.distance < minDistance && hit.transform.TryGetComponent<FlexalonInteractable>(out var hitInteractable))
                        {
                            _hitInteractable = hitInteractable;
                            minDistance = hit.distance;
                        }
                    }
                }

                return _hitInteractable == interactable;
            }
        }

        void Awake()
        {
            if (_clicked == null)
            {
                _clicked = new InteractableEvent();
            }

            if (_hoverStart == null)
            {
                _hoverStart = new InteractableEvent();
            }

            if (_hoverEnd == null)
            {
                _hoverEnd = new InteractableEvent();
            }

            if (_selectStart == null)
            {
                _selectStart = new InteractableEvent();
            }

            if (_selectEnd == null)
            {
                _selectEnd = new InteractableEvent();
            }

            if (_dragStart == null)
            {
                _dragStart = new InteractableEvent();
            }

            if (_dragEnd == null)
            {
                _dragEnd = new InteractableEvent();
            }
        }

        void Update()
        {
            // Ensure only one object is interacted with at a time.
            if (SelectedObject && SelectedObject != this)
            {
                return;
            }

            // Figure out if this object is being hit.
            var input = Flexalon.GetInputProvider();
            var ray = input.Ray;
            bool isHit = _raycaster.IsHit(ray, this);

            UpdateState(input, ray, isHit);
        }

        void FixedUpdate()
        {
            if (_state != InteractableState.Dragging)
            {
                return;
            }

            if (_target == _lastTarget)
            {
                return;
            }

            // Check if we're already in a target layout.
            var currentDragTarget = _placeholder.transform.parent ? _placeholder.transform.parent.GetComponent<FlexalonDragTarget>() : null;

            // Find a candidate target to insert into.
            var newDragTarget = FindLayoutToInsert();

            if (CanMoveFromTo(currentDragTarget, newDragTarget))
            {
                if (newDragTarget)
                {
                    AddToLayout(currentDragTarget, newDragTarget);
                }
                else
                {
                    MovePlaceholder(null);
                }
            }

            _lastTarget = _target;
        }

        private void SetState(InteractableState state)
        {
            _state = state;
        }

        private void UpdateState(InputProvider input, Ray ray, bool isHit)
        {
            if (_state == InteractableState.Init)
            {
                if (isHit)
                {
                    SetState(InteractableState.Hovering);
                    OnHoverStart();
                }
            }

            if (_state == InteractableState.Hovering)
            {
                if (!isHit)
                {
                    SetState(InteractableState.Init);
                    OnHoverEnd();
                }
                else if (input.Activated)
                {
                    SetState(InteractableState.Selecting);
                    OnSelectStart();
                }
            }

            if (_state == InteractableState.Selecting)
            {
                if (!input.Active)
                {
                    if (_clickable && isHit && (Time.time - _selectTime <= _maxClickTime))
                    {
                        Clicked.Invoke(this);
                    }

                    if (isHit)
                    {
                        SetState(InteractableState.Hovering);
                        OnSelectEnd();
                    }
                    else
                    {
                        SetState(InteractableState.Init);
                        OnSelectEnd();
                        OnHoverEnd();
                    }

                }
                else if (_draggable && (!_clickable || (Time.time - _selectTime > _maxClickTime)))
                {
                    SetState(InteractableState.Dragging);
                    OnDragStart(ray);
                }
            }

            if (_state == InteractableState.Dragging)
            {
                if (!input.Active)
                {
                    if (isHit)
                    {
                        SetState(InteractableState.Hovering);
                        OnDragEnd();
                        OnSelectEnd();
                    }
                    else
                    {
                        SetState(InteractableState.Init);
                        OnDragEnd();
                        OnSelectEnd();
                        OnHoverEnd();
                    }
                }
                else
                {
                    OnDragMove(input);
                }
            }
        }

        private void OnHoverStart()
        {
            _hoveredObject = this;
            HoverStart.Invoke(this);
        }

        private void OnHoverEnd()
        {
            if (_hoveredObject == this)
            {
                _hoveredObject = null;
            }

            HoverEnd.Invoke(this);
        }

        private void OnSelectStart()
        {
            _selectTime = Time.time;
            _selectedObject = this;
            SelectStart.Invoke(this);
        }

        private void OnSelectEnd()
        {
            if (_selectedObject == this)
            {
                _selectedObject = null;
            }

            SelectEnd.Invoke(this);
        }

        private void OnDragStart(Ray ray)
        {
            if (_hideCursor)
            {
                Cursor.visible = false;
            }

            _target = _lastTarget = transform.position;
            _distance = Vector3.Distance(_target, ray.origin);
            _startPosition = transform.position;

            // Create a placeholder
            _placeholder = new GameObject("Drag Placeholder");
            _placeholder.AddComponent<BoxCollider>();
            var placeholderObj = _placeholder.AddComponent<FlexalonObject>();
            var node = Flexalon.GetOrCreateNode(gameObject);
            placeholderObj.Size = node.Size;
            placeholderObj.SizeOfParent = node.SizeOfParent;
            placeholderObj.WidthType = node.GetSizeType(0);
            placeholderObj.HeightType = node.GetSizeType(1);
            placeholderObj.DepthType = node.GetSizeType(2);
            placeholderObj.Rotation = node.Rotation;
            placeholderObj.Scale = node.Scale;
            placeholderObj.Margin = node.Margin;
            placeholderObj.Padding = node.Padding;

            // If we're in a drag target, swap with the placeholder.
            if (transform.parent && transform.parent.GetComponent<FlexalonDragTarget>())
            {
                _placeholder.transform.SetParent(transform.parent.transform, true);
                _placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());
                _localSpace = transform.parent;
            }
            else
            {
                _placeholder.transform.SetParent(null);
                _placeholder.SetActive(false);
            }

            // Remove the object from its parent so it can be dragged freely.
            transform.SetParent(null, true);

            DragStart.Invoke(this);
        }

        private void OnDragMove(InputProvider input)
        {
            // Update the target where we want the object to go.
            UpdateTarget(input.Ray);

            // Apply hold offset
            var offset = _holdOffset;
            if (_localSpaceOffset && _localSpace)
            {
                offset = _localSpace.localToWorldMatrix.MultiplyVector(_holdOffset);
            }

            // Interpolate object towards target.
            transform.position = Vector3.Lerp(transform.position, _target + offset, Time.deltaTime * _interpolationSpeed);

            // Apply hold rotation
            if (_rotateOnDrag)
            {
                var rotation = _holdRotation;
                if (_localSpaceRotation && _localSpace)
                {
                    rotation = _localSpace.rotation * _holdRotation;;
                }

                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _interpolationSpeed);
            }
        }

        private void OnDragEnd()
        {
            // Swap places with the placeholder and destroy it.
            if (_placeholder.activeSelf)
            {
                transform.SetParent(_placeholder.transform.parent, true);
                transform.SetSiblingIndex(_placeholder.transform.GetSiblingIndex());
            }

            _localSpace = null;
            _placeholder.transform.SetParent(null);
            Destroy(_placeholder);

            if (_hideCursor)
            {
                Cursor.visible = true;
            }

            _selectedObject = null;
            DragEnd.Invoke(this);
        }

        private static bool ClosestPointOnTwoLines(Vector3 p0, Vector3 v0, Vector3 p1, Vector3 v1, out Vector3 closestPointLine2)
        {
            closestPointLine2 = Vector3.zero;

            float a = Vector3.Dot(v0, v0);
            float b = Vector3.Dot(v0, v1);
            float e = Vector3.Dot(v1, v1);

            float d = a * e - b * b;

            // Lines are not parallel
            if (d != 0.0f)
            {
                Vector3 r = p0 - p1;
                float c = Vector3.Dot(v0, r);
                float f = Vector3.Dot(v1, r);
                float t = (a * f - c * b) / d;
                closestPointLine2 = p1 + v1 * t;
                return true;
            }

            return false;
        }

        // Sets _target to where we want to move the dragged object -- based on the input ray, restrictions, and bounds.
        private void UpdateTarget(Ray ray)
        {
            if (_restriction == RestrictionType.Line)
            {
                var lineDir = _lineDirection;
                if (_localSpaceRestriction && _localSpace)
                {
                    lineDir = _localSpace.rotation * _lineDirection;
                }

                if (!ClosestPointOnTwoLines(ray.origin, ray.direction, _startPosition, lineDir.normalized, out _target))
                {
                    _target = _startPosition;
                }
            }
            else if (_restriction == RestrictionType.Plane)
            {
                var normal = _planeNormal;
                if (_localSpaceRestriction && _localSpace)
                {
                    normal = _localSpace.rotation * _planeNormal;
                }

                _plane.SetNormalAndPosition(normal.normalized, _startPosition);
                _plane.Raycast(ray, out var distance);
                _target = ray.origin + ray.direction * distance;
            }
            else
            {
                // If there's no restriction, just project forward at the same distance as the placeholder.
                if (_placeholder.gameObject.activeSelf)
                {
                    _distance = Vector3.Distance(ray.origin, _placeholder.transform.position);
                }

                _target = ray.origin + ray.direction * _distance;
            }

            // Apply bounds restriction
            if (_bounds && !_bounds.bounds.Contains(_target))
            {
                _target = _bounds.ClosestPointOnBounds(_target);
            }
        }

        // Uses a small sphere overlap to determine if the target is over a drag target
        // that we can insert into.
        private FlexalonDragTarget FindLayoutToInsert()
        {
            var overlaps = Physics.OverlapSphereNonAlloc(_target, 0.5f, _colliders);
            FlexalonDragTarget newDragTarget = null;
            for (int i = 0; i < overlaps; i++)
            {
                var hitTransform = _colliders[i].transform;
                var dragTarget = hitTransform.GetComponentInParent<FlexalonDragTarget>();
                if (dragTarget && (_layerMask.value & (1 << dragTarget.gameObject.layer)) != 0)
                {
                    newDragTarget = dragTarget;
                    break;
                }
            }

            return newDragTarget;
        }

        // Moves the placeholder into the drag target at a particular index.
        private void MovePlaceholder(Transform newParent, int siblingIndex = 0)
        {
            if (newParent != _placeholder.transform.parent || siblingIndex != _placeholder.transform.GetSiblingIndex())
            {
                _placeholder.SetActive(!!newParent);
                _placeholder.transform.SetParent(newParent);
                if (newParent)
                {
                    _placeholder.transform.SetSiblingIndex(siblingIndex);
                }
            }
        }

        // Finds an appropriate place to add the placeholder into the drag target.
        private void AddToLayout(FlexalonDragTarget currentDragTarget, FlexalonDragTarget newDragTarget)
        {
            // Insert at the nearest child.
            int insertIndex = 0;
            float minDistance = float.MaxValue;
            var moveDirection = (_target - _lastTarget).normalized;
            foreach (Transform child in newDragTarget.transform)
            {
                var childPos = newDragTarget.transform.localToWorldMatrix.MultiplyPoint(child.GetComponent<FlexalonResult>().TargetPosition);
                var toChild = (childPos - _lastTarget).normalized;
                if (child == _placeholder.transform || Vector3.Dot(toChild, moveDirection) > 0)
                {
                    var distSq = Vector3.SqrMagnitude(childPos - _target);
                    if (distSq < minDistance)
                    {
                        minDistance = distSq;
                        insertIndex = child.GetSiblingIndex();
                    }
                }
            }

            // Special case -- if adding a new item at the end, the user usually wants to place
            // it after the last element.
            if (currentDragTarget != newDragTarget && insertIndex == newDragTarget.transform.childCount - 1)
            {
                insertIndex++;
            }

            MovePlaceholder(newDragTarget.transform, insertIndex);
        }

        // Determines if we can move from one drag target to another, considering their options.
        // Layers are not considered here -- they're considered in FindLayoutToInsert to make
        // sure we skip those drag targets completely.
        private bool CanMoveFromTo(FlexalonDragTarget fromTarget, FlexalonDragTarget toTarget)
        {
            if (fromTarget == toTarget)
            {
                return true;
            }

            bool canLeave = fromTarget == null ||
                (fromTarget.CanRemoveObjects && fromTarget.transform.childCount > fromTarget.MinObjects);
            bool canAdd = toTarget == null ||
                (toTarget.CanAddObjects &&
                (toTarget.MaxObjects == 0 || toTarget.transform.childCount < toTarget.MaxObjects));
            return canLeave && canAdd;
        }
    }
}