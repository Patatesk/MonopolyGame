using UnityEngine;

namespace Flexalon
{
    [ExecuteAlways, AddComponentMenu("Flexalon/Flexalon Random Modifier"), HelpURL("https://www.flexalon.com/docs/randomModifier")]
    public class FlexalonRandomModifier : FlexalonComponent, FlexalonModifier
    {
        [SerializeField]
        private int _randomSeed = 1;
        public int RandomSeed
        {
            get => _randomSeed;
            set { _randomSeed = value; MarkDirty(); }
        }

        [SerializeField]
        private bool _randomizePositionX = false;
        public bool RandomizePositionX
        {
            get => _randomizePositionX;
            set { _randomizePositionX = value; MarkDirty(); }
        }

        [SerializeField]
        private float _positionMinX = 0;
        public float PositionMinX
        {
            get => _positionMinX;
            set
            {
                _positionMinX = value;
                _randomizePositionX = true;
                MarkDirty();
            }
        }

        [SerializeField]
        private float _positionMaxX = 0;
        public float PositionMaxX
        {
            get => _positionMaxX;
            set
            {
                _positionMaxX = value;
                _randomizePositionX = true;
                MarkDirty();
            }
        }

        [SerializeField]
        private bool _randomizePositionY = false;
        public bool RandomizePositionY
        {
            get => _randomizePositionY;
            set { _randomizePositionY = value; MarkDirty(); }
        }

        [SerializeField]
        private float _positionMinY = 0;
        public float PositionMinY
        {
            get => _positionMinY;
            set
            {
                _positionMinY = value;
                _randomizePositionY = true;
                MarkDirty();
            }
        }

        [SerializeField]
        private float _positionMaxY = 0;
        public float PositionMaxY
        {
            get => _positionMaxY;
            set
            {
                _positionMaxY = value;
                _randomizePositionY = true;
                MarkDirty();
            }
        }

        [SerializeField]
        private bool _randomizePositionZ = false;
        public bool RandomizePositionZ
        {
            get => _randomizePositionZ;
            set { _randomizePositionZ = value; MarkDirty(); }
        }

        [SerializeField]
        private float _positionMinZ = 0;
        public float PositionMinZ
        {
            get => _positionMinZ;
            set
            {
                _positionMinZ = value;
                _randomizePositionZ = true;
                MarkDirty();
            }
        }

        [SerializeField]
        private float _positionMaxZ = 0;
        public float PositionMaxZ
        {
            get => _positionMaxZ;
            set
            {
                _positionMaxZ = value;
                _randomizePositionZ = true;
                MarkDirty();
            }
        }

        [SerializeField]
        private Vector3 _positionMin => new Vector3(_positionMinX, _positionMinY, _positionMinZ);
        public Vector3 PositionMin
        {
            get => _positionMin;
            set
            {
                _positionMinX = value.x;
                _positionMinY = value.y;
                _positionMinZ = value.z;
                _randomizePositionX = true;
                _randomizePositionY = true;
                _randomizePositionZ = true;
                MarkDirty();
            }
        }

        [SerializeField]
        private Vector3 _positionMax => new Vector3(_positionMaxX, _positionMaxY, _positionMaxZ);
        public Vector3 PositionMax
        {
            get => _positionMax;
            set
            {
                _positionMaxX = value.x;
                _positionMaxY = value.y;
                _positionMaxZ = value.z;
                _randomizePositionX = true;
                _randomizePositionY = true;
                _randomizePositionZ = true;
                MarkDirty();
            }
        }

        [SerializeField]
        private bool _randomizeRotationX = false;
        public bool RandomizeRotationX
        {
            get => _randomizeRotationX;
            set { _randomizeRotationX = value; MarkDirty(); }
        }

        [SerializeField, Range(-360, 360)]
        private float _rotationMinX = 0;
        public float RotationMinX
        {
            get => _rotationMinX;
            set
            {
                _rotationMinX = value;
                _randomizeRotationX = true;
                MarkDirty();
            }
        }

        [SerializeField, Range(-360, 360)]
        private float _rotationMaxX = 0;
        public float RotationMaxX
        {
            get => _rotationMaxX;
            set
            {
                _rotationMaxX = value;
                _randomizeRotationX = true;
                MarkDirty();
            }
        }

        [SerializeField]
        private bool _randomizeRotationY = false;
        public bool RandomizeRotationY
        {
            get => _randomizeRotationY;
            set { _randomizeRotationY = value; MarkDirty(); }
        }

        [SerializeField, Range(-360, 360)]
        private float _rotationMinY = 0;
        public float RotationMinY
        {
            get => _rotationMinY;
            set
            {
                _rotationMinY = value;
                _randomizeRotationY = true;
                MarkDirty();
            }
        }

        [SerializeField, Range(-360, 360)]
        private float _rotationMaxY = 0;
        public float RotationMaxY
        {
            get => _rotationMaxY;
            set
            {
                _rotationMaxY = value;
                _randomizeRotationY = true;
                MarkDirty();
            }
        }

        [SerializeField]
        private bool _randomizeRotationZ = false;
        public bool RandomizeRotationZ
        {
            get => _randomizeRotationZ;
            set { _randomizeRotationZ = value; MarkDirty(); }
        }

        [SerializeField, Range(-360, 360)]
        private float _rotationMinZ = 0;
        public float RotationMinZ
        {
            get => _rotationMinZ;
            set
            {
                _rotationMinZ = value;
                _randomizeRotationZ = true;
                MarkDirty();
            }
        }

        [SerializeField, Range(-360, 360)]
        private float _rotationMaxZ = 0;
        public float RotationMaxZ
        {
            get => _rotationMaxZ;
            set
            {
                _rotationMaxZ = value;
                _randomizeRotationZ = true;
                MarkDirty();
            }
        }

        protected override void UpdateProperties()
        {
            Node.AddModifier(this);
        }

        protected override void ResetProperties()
        {
            Node.RemoveModifier(this);
        }

        public void PostArrange(FlexalonNode node)
        {
            var randomGen = new System.Random(_randomSeed);
            var randomGenPositionX = new System.Random(randomGen.Next());
            var randomGenPositionY = new System.Random(randomGen.Next());
            var randomGenPositionZ = new System.Random(randomGen.Next());
            var randomGenRotationX = new System.Random(randomGen.Next());
            var randomGenRotationY = new System.Random(randomGen.Next());
            var randomGenRotationZ = new System.Random(randomGen.Next());

            foreach (var child in node.Children)
            {
                var position = new Vector3(
                    _randomizePositionX ? _positionMinX + (float)randomGenPositionX.NextDouble() * (_positionMaxX - _positionMinX) : 0,
                    _randomizePositionY ? _positionMinY + (float)randomGenPositionY.NextDouble() * (_positionMaxY - _positionMinY) : 0,
                    _randomizePositionZ ? _positionMinZ + (float)randomGenPositionZ.NextDouble() * (_positionMaxZ - _positionMinZ) : 0);

                var rotation = new Vector3(
                    _randomizeRotationX ? _rotationMinX + (float)randomGenRotationX.NextDouble() * (_rotationMaxX - _rotationMinX) : 0,
                    _randomizeRotationY ? _rotationMinY + (float)randomGenRotationY.NextDouble() * (_rotationMaxY - _rotationMinY) : 0,
                    _randomizeRotationZ ? _rotationMinZ + (float)randomGenRotationZ.NextDouble() * (_rotationMaxZ - _rotationMinZ) : 0);

                child.SetPositionResult(child.Result.LayoutPosition + position);
                child.SetRotationResult(child.Result.LayoutRotation * Quaternion.Euler(rotation));
            }
        }
    }
}