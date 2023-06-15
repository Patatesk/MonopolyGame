using UnityEngine;

namespace Flexalon
{
    [AddComponentMenu("Flexalon/Flexalon Drag Target"), HelpURL("https://www.flexalon.com/docs/dragging"), DisallowMultipleComponent]
    public class FlexalonDragTarget : MonoBehaviour
    {
        [SerializeField]
        private bool _canRemoveObjects = true;
        public bool CanRemoveObjects {
            get => _canRemoveObjects;
            set => _canRemoveObjects = value;
        }

        [SerializeField]
        private bool _canAddObjects = true;
        public bool CanAddObjects {
            get => _canAddObjects;
            set => _canAddObjects = value;
        }

        [SerializeField]
        private int _minObjects;
        public int MinObjects {
            get => _minObjects;
            set => _minObjects = value;
        }

        [SerializeField]
        private int _maxObjects;
        public int MaxObjects {
            get => _maxObjects;
            set => _maxObjects = value;
        }

        private BoxCollider _collider;
        private FlexalonNode _node;

        void OnEnable()
        {
            _node = Flexalon.GetOrCreateNode(gameObject);
            _collider = gameObject.AddComponent<BoxCollider>();
            _node.ResultChanged += OnResultChanged;
            OnResultChanged(_node);
        }

        void OnDisable()
        {
            _node.ResultChanged -= OnResultChanged;
            Destroy(_collider);
            _collider = null;
            _node = null;
        }

        void OnResultChanged(FlexalonNode node)
        {
            if (_collider)
            {
                _collider.center = node.Result.LayoutBounds.center;
                _collider.size = node.Result.LayoutBounds.size;
            }
        }
    }
}