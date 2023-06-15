using System.Collections.Generic;
using UnityEngine;

namespace Flexalon
{
    [ExecuteAlways, AddComponentMenu("Flexalon/Flexalon Curve Layout"), HelpURL("https://www.flexalon.com/docs/curveLayout")]
    public class FlexalonCurveLayout : LayoutBase
    {
        public enum TangentMode
        {
            // Define the tangent by entering a value or dragging the handle in the scene window.
            Manual,

            // Sets the tangent to match the tangent at the previous point.
            MatchPrevious,

            // Sets the tangent to zero to create a sharp corner.
            Corner,

            // Computes a tangent that will create a smooth curve between the previous and next points.
            Smooth,
        }

        [System.Serializable]
        public struct CurvePoint
        {
            public Vector3 Position;
            public TangentMode TangentMode;
            public Vector3 Tangent;

            public CurvePoint ChangePosition(Vector3 position)
            {
                var copy = Copy();
                copy.Position = position;
                return copy;
            }

            public CurvePoint ChangeTangent(Vector3 tangent)
            {
                var copy = Copy();
                copy.Tangent = tangent;
                return copy;
            }

            public CurvePoint Copy()
            {
                return new CurvePoint {
                    Position = Position,
                    TangentMode = TangentMode,
                    Tangent = Tangent,
                };
            }
        }

        [SerializeField]
        private List<CurvePoint> _points;

        public IReadOnlyList<CurvePoint> Points => _points;

        public enum SpacingOptions
        {
            // Define the distance between each child with the "Spacing" property.
            Fixed,

            // The first child is placed at the beginning of the curve and the last child is placed
            // at the end of the curve. The rest of the children are placed at even distances between
            // these points along the curve.
            Evenly,

            // If the beginning of the curve is connected to the end of the curve, then the first
            // child is placed at the beginning/end of the curve, and the rest of the children are placed
            // at even distances along the curve.
            EvenlyConnected,
        }

        // Prevents the tangent handles from appearing in the editor.
        [SerializeField]
        private bool _lockTangents = false;
        public bool LockTangents
        {
            get => _lockTangents;
            set { _lockTangents = value; }
        }

        // Prevents the position handles from appearing in the editor.
        [SerializeField]
        private bool _lockPositions = false;
        public bool LockPositions
        {
            get => _lockPositions;
            set { _lockPositions = value; }
        }

        // Determines how the children will be spaced along the curve.
        [SerializeField]
        private SpacingOptions _spacingType;
        public SpacingOptions SpacingType
        {
            get { return _spacingType; }
            set { _spacingType = value; _node.MarkDirty(); }
        }

        [SerializeField]
        private float _spacing = 0.5f;
        public float Spacing
        {
            get { return _spacing; }
            set { _spacing = value; _node.MarkDirty(); }
        }

        // Offsets all objects along the curve.
        [SerializeField]
        private float _startAt = 0.0f;
        public float StartAt
        {
            get { return _startAt; }
            set { _startAt = value; _node.MarkDirty(); }
        }

        // Determines how children should be rotated
        public enum RotationOptions
        {
            // Sets all child rotations to zero.
            None,

            // Each child is rotated to the right of the forward direction of the curve.
            In,

            // Each child is rotated to the left of the forward direction of the curve.
            Out,

            // Each child is rotated to the right of the forward direction of the curve
            // and rolled so that the X axis matches the curve backward direction.
            InWithRoll,

            // Each child is rotated to the left of the forward direction of the curve
            // and rolled so that the X axis matches the curve forward direction.
            OutWithRoll,

            // Each child is rotated to face forward along the curve.
            Forward,

            // Each child is rotated to face backward along the curve.
            Backward
        }

        [SerializeField]
        private RotationOptions _rotation;
        public RotationOptions Rotation
        {
            get { return _rotation; }
            set { _rotation = value; _node.MarkDirty(); }
        }

        void Awake()
        {
            if (_points == null)
            {
                _points = new List<CurvePoint>() {
                    new CurvePoint() { Position = Vector3.left, TangentMode = TangentMode.Smooth, Tangent = Vector3.zero },
                    new CurvePoint() { Position = Vector3.zero, TangentMode = TangentMode.Smooth, Tangent = Vector3.zero },
                    new CurvePoint() { Position = Vector3.right, TangentMode = TangentMode.Smooth, Tangent = Vector3.zero },
                };
            };
        }

        // Adds a new point to the end of the curve.
        public void AddPoint(CurvePoint point)
        {
            _points.Add(point);
            MarkDirty();
        }

        // Adds a new point to the end of the curve.
        public void AddPoint(Vector3 position, Vector3 tangent)
        {
            AddPoint(new CurvePoint{ Position = position, Tangent = tangent, TangentMode = TangentMode.Manual });
        }

        // Inserts a new point into the curve at the specified index.
        public void InsertPoint(int index, CurvePoint point)
        {
            _points.Insert(index, point);
            MarkDirty();
        }

        // Inserts a new point into the curve at the specified index.
        public void InsertPoint(int index, Vector3 position, Vector3 tangent)
        {
            InsertPoint(index, new CurvePoint{ Position = position, Tangent = tangent, TangentMode = TangentMode.Manual });
        }

        // Replaces the point at the index with a new point.
        public void ReplacePoint(int index, CurvePoint point)
        {
            _points.RemoveAt(index);
            InsertPoint(index, point);
        }

        // Replaces the point at the index with a new point.
        public void ReplacePoint(int index, Vector3 position, Vector3 tangent)
        {
            ReplacePoint(index, new CurvePoint{ Position = position, Tangent = tangent, TangentMode = TangentMode.Manual });
        }

        // Removes the point at the index.
        public void RemovePoint(int index)
        {
            _points.RemoveAt(index);
            MarkDirty();
        }

        // Replaces all points of the curve.
        public void SetPoints(List<CurvePoint> points)
        {
            _points = points;
            MarkDirty();
        }

        // Points along the curve used to position objects and can be used for visualization.
        [SerializeField] // Saved for editor visual
        private List<Vector3> _curvePositions = new List<Vector3>();
        public IReadOnlyList<Vector3> CurvePositions => _curvePositions;

        // The total length of the curve.
        private float _curveLength = 0;
        public float CurveLength => _curveLength;

        private List<float> _curveLengths = new List<float>();
        private Bounds _curveBounds;
        private List<CurvePoint> _computedPoints = new List<CurvePoint>();

        private void UpdateCurvePositions()
        {
            if (_computedPoints.Count == _points.Count)
            {
                bool curveChanged = false;
                for (int i = 0; i < _points.Count; i++)
                {
                    if (_computedPoints[i].Position != _points[i].Position || _computedPoints[i].Tangent != _points[i].Tangent)
                    {
                        curveChanged = true;
                        break;
                    }
                }

                if (!curveChanged)
                {
                    return;
                }
            }

            _curvePositions.Clear();
            _curveLengths.Clear();
            _curveLength = 0;
            _computedPoints.Clear();
            _computedPoints.AddRange(_points);

            if (_points.Count == 0)
            {
                _curveBounds = new Bounds();
                return;
            }

            _curvePositions.Add(_points[0].Position);
            _curveLengths.Add(0);
            _curveBounds = new Bounds(_points[0].Position, Vector3.zero);
            var prev = _points[0].Position;
            for (int i = 1; i < _points.Count; i++)
            {
                for (int j = 1; j <= 100; j++)
                {
                    var pos = ComputePositionOnBezierCurve(_points[i - 1], _points[i], (float) j / 100);
                    var len = Vector3.Distance(prev, pos);
                    _curvePositions.Add(pos);
                    _curveLength += len;
                    _curveLengths.Add(_curveLength);
                    _curveBounds.Encapsulate(pos);
                    prev = pos;
                }
            }
        }

        private (Vector3, Vector3) GetCurvePositionAndForwardAtDistance(float distance)
        {
            // Assumes > 1 points
            distance = Mathf.Clamp(distance, 0, _curveLength);

            int s = 0;
            int e = _curvePositions.Count - 1;

            int i = 0;
            while (s != e && i < 100)
            {
                i++;
                var m = s + (e - s) / 2;
                if (_curveLengths[m] <= distance)
                {
                    s = m + 1;
                }
                else
                {
                    e = m;
                }
            }

            // We should be at the next position after distance.
            var distanceBetweenPoints = _curveLengths[e] - _curveLengths[e - 1];
            var t = distanceBetweenPoints > 0 ? (distance - _curveLengths[e - 1]) / distanceBetweenPoints : 1;
            var p = Vector3.Lerp(_curvePositions[e - 1], _curvePositions[e], t);
            var f = _curvePositions[e] - _curvePositions[e - 1];
            return (p, f.normalized);
        }

        private void UpdateTangents()
        {
            for (int i = 0; i < _points.Count; i++)
            {
                if (_points[i].TangentMode == TangentMode.MatchPrevious && i > 0)
                {
                    _points[i] = _points[i].ChangeTangent(_points[i - 1].Tangent);
                }
                else if (_points[i].TangentMode == TangentMode.Corner)
                {
                    _points[i] = _points[i].ChangeTangent(Vector3.zero);
                }
                else if (_points[i].TangentMode == TangentMode.Smooth)
                {
                    Vector3 tangent = Vector3.zero;
                    if (i > 0)
                    {
                        if (i < _points.Count - 1)
                        {
                            var v1 = _points[i + 1].Position - _points[i].Position;
                            var v2 = _points[i].Position - _points[i - 1].Position;
                            tangent = (v1 + v2) / 4;
                        }
                        else
                        {
                            tangent = (_points[i].Position - _points[i - 1].Position) / 4;
                        }
                    }
                    else
                    {
                        if (_points.Count > 1)
                        {
                            tangent = (_points[i + 1].Position - _points[i].Position) / 4;
                        }
                    }

                    _points[i] = _points[i].ChangeTangent(tangent);
                }
            }
        }

        public override Bounds Measure(FlexalonNode node, Vector3 size)
        {
            FlexalonLog.Log("CurveMeasure | Size", node, size);

            UpdateTangents();
            UpdateCurvePositions();

            Vector3 center = Vector3.zero;
            for (int i = 0; i < 3; i++)
            {
                if (node.GetSizeType(i) == SizeType.Layout)
                {
                    center[i] = _curveBounds.center[i];
                    size[i] = _curveBounds.size[i];
                }
            }

            var childFillSizeXZ = _curveLength / node.Children.Count;
            var childFillSize = new Vector3(childFillSizeXZ, size.y, childFillSizeXZ);
            base.Measure(node, childFillSize);

            return new Bounds(center, size);
        }

        public override void Arrange(FlexalonNode node, Vector3 layoutSize)
        {
            FlexalonLog.Log("CurveArrange | LayoutSize", node, layoutSize);

            if (node.Children.Count == 0 || _points == null || _points.Count < 2)
            {
                return;
            }

            var spacing = _spacing;
            if (node.Children.Count > 1)
            {
                if (_spacingType == SpacingOptions.Evenly)
                {
                    spacing = _curveLength / (node.Children.Count - 1);
                }
                else if (_spacingType == SpacingOptions.EvenlyConnected)
                {
                    spacing = _curveLength / node.Children.Count;
                }
            }

            var d = _startAt;
            for (int i = 0; i < node.Children.Count; i++)
            {
                var (position, forward) = GetCurvePositionAndForwardAtDistance(d);
                node.Children[i].SetPositionResult(position);
                d += spacing;

                var rotation = Quaternion.identity;
                var inDirection = Vector3.Cross(forward, Vector3.up).normalized;
                switch (_rotation)
                {
                    case RotationOptions.In:
                        rotation = Quaternion.LookRotation(inDirection);
                        break;
                    case RotationOptions.Out:
                        rotation = Quaternion.LookRotation(-inDirection);
                        break;
                    case RotationOptions.InWithRoll:
                        rotation = Quaternion.LookRotation(inDirection, Vector3.Cross(inDirection, forward));
                        break;
                    case RotationOptions.OutWithRoll:
                        rotation = Quaternion.LookRotation(-inDirection, Vector3.Cross(inDirection, forward));
                        break;
                    case RotationOptions.Forward:
                        rotation = Quaternion.LookRotation(forward);
                        break;
                    case RotationOptions.Backward:
                        rotation = Quaternion.LookRotation(-forward);
                        break;
                }

                node.Children[i].SetRotationResult(rotation);
            }
        }

        public static Vector3 ComputePositionOnBezierCurve(CurvePoint point1, CurvePoint point2, float t)
        {
            Vector3 p1 = point1.Position;
            Vector3 p2 = point1.Position + point1.Tangent;
            Vector3 p3 = point2.Position - point2.Tangent;
            Vector3 p4 = point2.Position;

            float a = Mathf.Pow(1 - t, 3);
            float b = 3 * Mathf.Pow(1 - t, 2) * t;
            float c = 3 * (1 - t) * Mathf.Pow(t, 2);
            float d = Mathf.Pow(t, 3);
            return p1 * a + p2 * b + p3 * c + p4 * d;
        }

#if UNITY_EDITOR
        public int EditorHovered = -1;

        private void OnDrawGizmosSelected()
        {
            var scale = _node.GetWorldBoxScale(true);
            Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, transform.rotation, scale);

            for (int i = 0; i < _points.Count; i++)
            {
                float sphereSize = 0.025f;
                Gizmos.color = new Color(1, 1, 1, 0.9f);
                if (EditorHovered == i)
                {
                    Gizmos.color = Color.cyan;
                    sphereSize = 0.05f;
                }

                Gizmos.DrawSphere(_points[i].Position, sphereSize);
            }
        }
#endif
    }
}