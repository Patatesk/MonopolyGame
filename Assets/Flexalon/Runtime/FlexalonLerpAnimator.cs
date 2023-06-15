using UnityEngine;

namespace Flexalon
{
    [AddComponentMenu("Flexalon/Flexalon Lerp Animator"), HelpURL("https://www.flexalon.com/docs/animators")]
    public class FlexalonLerpAnimator : MonoBehaviour, TransformUpdater
    {
        private FlexalonNode _node;

        [SerializeField]
        private float _interpolationSpeed = 5.0f;
        public float InterpolationSpeed
        {
            get => _interpolationSpeed;
            set { _interpolationSpeed = value; }
        }

        [SerializeField]
        private bool _animatePosition = true;
        public bool AnimatePosition
        {
            get => _animatePosition;
            set { _animatePosition = value; }
        }

        [SerializeField]
        private bool _animateRotation = true;
        public bool AnimateRotation
        {
            get => _animateRotation;
            set { _animateRotation = value; }
        }

        [SerializeField]
        private bool _animateScale = true;
        public bool AnimateScale
        {
            get => _animateScale;
            set { _animateScale = value; }
        }

        private Vector3 _fromPosition;
        private Quaternion _fromRotation;
        private Vector3 _fromScale;

        void OnEnable()
        {
            _node = Flexalon.GetOrCreateNode(gameObject);
            _node.SetTransformUpdater(this);
        }

        void OnDisable()
        {
            _node?.SetTransformUpdater(null);
            _node = null;
        }

        public void PreUpdate(FlexalonNode node)
        {
            _fromPosition = transform.position;
            _fromRotation = transform.rotation;
            _fromScale = transform.lossyScale;
        }

        public bool UpdatePosition(FlexalonNode node, Vector3 position)
        {
            var worldPosition = transform.parent ? transform.parent.localToWorldMatrix.MultiplyPoint(position) : position;
            if (!_animatePosition || Vector3.Distance(_fromPosition, worldPosition) < 0.001f)
            {
                transform.localPosition = position;
                return true;
            }
            else
            {
                transform.position = Vector3.Lerp(_fromPosition, worldPosition, _interpolationSpeed * Time.smoothDeltaTime);
                return false;
            }
        }

        public bool UpdateRotation(FlexalonNode node, Quaternion rotation)
        {
            var worldRotation = transform.parent ? transform.parent.rotation * rotation : rotation;
            if (!_animateRotation || Mathf.Abs(Quaternion.Angle(_fromRotation, worldRotation)) < 0.001f)
            {
                transform.localRotation = rotation;
                return true;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(_fromRotation, worldRotation, _interpolationSpeed * Time.smoothDeltaTime);
                return false;
            }
        }

        public bool UpdateScale(FlexalonNode node, Vector3 scale)
        {
            var worldScale = transform.parent ? Math.Mul(scale, transform.parent.lossyScale) : scale;
            if (!_animateScale || Vector3.Distance(_fromScale, worldScale) < 0.001f)
            {
                transform.localScale = scale;
                return true;
            }
            else
            {
                var newWorldScale = Vector3.Lerp(_fromScale, worldScale, _interpolationSpeed * Time.smoothDeltaTime);
                transform.localScale = transform.parent ? Math.Div(newWorldScale, transform.parent.lossyScale) : newWorldScale;
                return false;
            }
        }
    }
}