using UnityEngine;

namespace Flexalon
{
    [ExecuteAlways, DisallowMultipleComponent, AddComponentMenu("Flexalon/Flexalon Constraint"), HelpURL("https://www.flexalon.com/docs/constraints")]
    public class FlexalonConstraint : FlexalonComponent
    {
        [SerializeField]
        private GameObject _target;
        public GameObject Target
        {
            get { return _target; }
            set { _target = value; MarkDirty(); }
        }

        [SerializeField]
        private Align _horizontalAlign = Align.Center;
        public Align HorizontalAlign
        {
            get { return _horizontalAlign; }
            set { _horizontalAlign = value; MarkDirty(); }
        }

        [SerializeField]
        private Align _verticalAlign = Align.Center;
        public Align VerticalAlign
        {
            get { return _verticalAlign; }
            set { _verticalAlign = value; MarkDirty(); }
        }

        [SerializeField]
        private Align _depthAlign = Align.Center;
        public Align DepthAlign
        {
            get { return _depthAlign; }
            set { _depthAlign = value; MarkDirty(); }
        }

        [SerializeField]
        private Align _horizontalPivot = Align.Center;
        public Align HorizontalPivot
        {
            get { return _horizontalPivot; }
            set { _horizontalPivot = value; MarkDirty(); }
        }

        [SerializeField]
        private Align _verticalPivot = Align.Center;
        public Align VerticalPivot
        {
            get { return _verticalPivot; }
            set { _verticalPivot = value; MarkDirty(); }
        }

        [SerializeField]
        private Align _depthPivot = Align.Center;
        public Align DepthPivot
        {
            get { return _depthPivot; }
            set { _depthPivot = value; MarkDirty(); }
        }

        private Vector3 _lastTargetPosition;
        private Quaternion _lastTargetRotation;
        private Vector3 _lastTargetScale;

        protected override void ResetProperties()
        {
            UpdateTarget(null);
        }

        public override void DoUpdate()
        {
            if (_target)
            {
                if (_lastTargetPosition != _target.transform.position ||
                    _lastTargetRotation != _target.transform.rotation ||
                    _lastTargetScale != _target.transform.lossyScale)
                {
                    MarkDirty();
                }

                // Detect scale/rotation changes in edit mode, even if we don't have a FlexalonObject.
                if (!Application.isPlaying && !Node.Dirty && _node.FlexalonObject == null &&
                    (_node.Result.TransformPosition != transform.localPosition ||
                     _node.Result.TransformRotation != transform.localRotation ||
                     _node.Result.TransformScale != transform.localScale))
                {
                    MarkDirty();
                }
            }
        }

        protected override void UpdateProperties()
        {
            UpdateTarget(_target);
        }

        private void UpdateTarget(GameObject target)
        {
            if (target)
            {
                var targetNode = Flexalon.GetOrCreateNode(target);
                _node.SetConstraint(this, targetNode);

                if (!targetNode.HasResult)
                {
                    targetNode.MarkDirty();
                }

                _lastTargetPosition = target.transform.position;
                _lastTargetRotation = target.transform.rotation;
                _lastTargetScale = target.transform.lossyScale;
            }
            else
            {
                _node.SetConstraint(null, null);
            }
        }

        public void Constrain(FlexalonNode node)
        {
            if (_target)
            {
                var targetNode = Flexalon.GetOrCreateNode(_target);
                var targetSize = targetNode.Result.AdapterBounds.size + targetNode.Margin.Size;
                var bounds = node.Result.RotatedAndScaledBounds;
                bounds.center += node.Margin.Center;
                bounds.size += node.Margin.Size;

                var alignPosition = Math.Align(targetSize, _horizontalAlign, _verticalAlign, _depthAlign);

                FlexalonLog.Log("Constrain:AlignPosition [Initial]", node, alignPosition);

                alignPosition += targetNode.Result.AdapterBounds.center;
                alignPosition += targetNode.Margin.Center;
                FlexalonLog.Log("Constrain:AlignPosition [Centered]", node, alignPosition);

                alignPosition.Scale(targetNode.GetWorldBoxScale(true));
                FlexalonLog.Log("Constrain:AlignPosition [Scaled]", node, alignPosition);

                var pivotPosition = Math.Align(bounds.size, _horizontalPivot, _verticalPivot, _depthPivot);

                FlexalonLog.Log("Constrain:PivotPosition", node, pivotPosition);

                var worldRotation = _target.transform.rotation;
                var localRotation = Quaternion.Inverse(transform.parent?.rotation ?? Quaternion.identity) * worldRotation;

                var position = alignPosition - pivotPosition - bounds.center + node.Offset;
                var worldPosition = worldRotation * (position) + _target.transform.position;
                FlexalonLog.Log("Constrain:WorldPosition", node, worldPosition);

                var localPosition = transform.parent?.worldToLocalMatrix.MultiplyPoint(worldPosition) ?? worldPosition;
                FlexalonLog.Log("Constrain:LocalPosition", node, localPosition);

                node.SetPositionResult(localPosition);
                node.SetRotationResult(localRotation);
            }
        }
    }
}