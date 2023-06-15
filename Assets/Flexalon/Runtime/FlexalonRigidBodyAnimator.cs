using UnityEngine;

namespace Flexalon
{
    [AddComponentMenu("Flexalon/Flexalon Rigid Body Animator"), HelpURL("https://www.flexalon.com/docs/animators")]
    public class FlexalonRigidBodyAnimator : MonoBehaviour, TransformUpdater
    {
        private FlexalonNode _node;
        private Rigidbody _rigidBody;

        [SerializeField]
        private float _positionForce = 5.0f;
        public float PositionForce
        {
            get => _positionForce;
            set { _positionForce = value; }
        }

        [SerializeField]
        private float _rotationForce = 5.0f;
        public float RotationForce
        {
            get => _rotationForce;
            set { _rotationForce = value; }
        }

        [SerializeField]
        private float _scaleInterpolationSpeed = 5.0f;
        public float ScaleInterpolationSpeed
        {
            get => _scaleInterpolationSpeed;
            set { _scaleInterpolationSpeed = value; }
        }

        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        private Vector3 _fromScale;

        void OnEnable()
        {
            _node = Flexalon.GetOrCreateNode(gameObject);
            _node.SetTransformUpdater(this);
            _rigidBody = GetComponent<Rigidbody>();
            _targetPosition = transform.localPosition;
            _targetRotation = transform.localRotation;
        }

        void OnDisable()
        {
            _node.SetTransformUpdater(null);
            _node = null;
        }

        public void PreUpdate(FlexalonNode node)
        {
            _fromScale = transform.lossyScale;
        }

        public bool UpdatePosition(FlexalonNode node, Vector3 position)
        {
            if (_rigidBody)
            {
                _targetPosition = position;
                return false;
            }
            else
            {
                transform.localPosition = position;
                return true;
            }
        }

        public bool UpdateRotation(FlexalonNode node, Quaternion rotation)
        {
            if (_rigidBody)
            {
                _targetRotation = rotation;
                return false;
            }
            else
            {
                transform.localRotation = rotation;
                return true;
            }
        }

        public bool UpdateScale(FlexalonNode node, Vector3 scale)
        {
            var worldScale = transform.parent == null ? scale : Math.Mul(scale, transform.parent.lossyScale);
            if (Vector3.Distance(_fromScale, worldScale) < 0.001f)
            {
                transform.localScale = scale;
                return true;
            }
            else
            {
                var newWorldScale = Vector3.Lerp(_fromScale, worldScale, _scaleInterpolationSpeed * Time.smoothDeltaTime);
                transform.localScale = transform.parent == null ? newWorldScale : Math.Div(newWorldScale, transform.parent.lossyScale);
                return false;
            }
        }

        void FixedUpdate()
        {
            if (_rigidBody)
            {
                var worldPos = transform.parent ? transform.parent.localToWorldMatrix.MultiplyPoint(_targetPosition) : _targetPosition;
                _rigidBody.AddForce((worldPos - transform.position) * _positionForce, ForceMode.Force);

                var rot = Quaternion.Slerp(transform.localRotation, _targetRotation, _rotationForce * Time.deltaTime);
                var rotWorldSpace = (transform.parent?.rotation ?? Quaternion.identity) * rot;
                _rigidBody.MoveRotation(rotWorldSpace);
            }
        }
    }
}