using UnityEngine;

namespace Flexalon
{
    public static class Math
    {
        public enum Plane
        {
            XY,
            XZ,
            YZ
        }

        public static Direction GetOppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.PositiveX: return Direction.NegativeX;
                case Direction.NegativeX: return Direction.PositiveX;
                case Direction.PositiveY: return Direction.NegativeY;
                case Direction.NegativeY: return Direction.PositiveY;
                case Direction.PositiveZ: return Direction.NegativeZ;
                case Direction.NegativeZ: return Direction.PositiveZ;
                default: return Direction.PositiveX;
            }
        }

        public static Direction GetOppositeDirection(int direction)
        {
            return GetOppositeDirection((Direction)direction);
        }

        public static Axis GetAxisFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.PositiveX: return Axis.X;
                case Direction.NegativeX: return Axis.X;
                case Direction.PositiveY: return Axis.Y;
                case Direction.NegativeY: return Axis.Y;
                case Direction.PositiveZ: return Axis.Z;
                case Direction.NegativeZ: return Axis.Z;
                default: return Axis.X;
            }
        }

        public static Axis GetAxisFromDirection(int direction)
        {
            return GetAxisFromDirection((Direction)direction);
        }

        public static (Direction, Direction) GetDirectionsFromAxis(Axis axis)
        {
            switch (axis)
            {
                case Axis.X: return (Direction.PositiveX, Direction.NegativeX);
                case Axis.Y: return (Direction.PositiveY, Direction.NegativeY);
                case Axis.Z: return (Direction.PositiveZ, Direction.NegativeZ);
                default: return (Direction.PositiveX, Direction.NegativeX);
            }
        }

        public static (Direction, Direction) GetDirectionsFromAxis(int axis)
        {
            return GetDirectionsFromAxis((Axis)axis);
        }

        public static float GetPositiveFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.PositiveX:
                case Direction.PositiveY:
                case Direction.PositiveZ:
                    return 1;
                default:
                    return -1;
            }
        }

        public static float GetPositiveFromDirection(int direction)
        {
            return GetPositiveFromDirection((Direction)direction);
        }

        public static Vector3 GetVectorFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.PositiveX:
                    return Vector3.right;
                case Direction.PositiveY:
                    return Vector3.up;
                case Direction.PositiveZ:
                    return Vector3.forward;
                case Direction.NegativeX:
                    return Vector3.left;
                case Direction.NegativeY:
                    return Vector3.down;
                case Direction.NegativeZ:
                    return Vector3.back;
            }

            return Vector3.zero;
        }

        public static Vector3 GetVectorFromDirection(int direction)
        {
            return GetVectorFromDirection((Direction)direction);
        }

        public static (Axis, Axis) GetOtherAxes(Axis axis)
        {
            switch (axis)
            {
                case Axis.X: return (Axis.Y, Axis.Z);
                case Axis.Y: return (Axis.X, Axis.Z);
                default: return (Axis.X, Axis.Y);
            }
        }

        public static (int, int) GetOtherAxes(int axis)
        {
            var other = GetOtherAxes((Axis)axis);
            return ((int)other.Item1, (int)other.Item2);
        }

        public static Axis GetThirdAxis(Axis axis1, Axis axis2)
        {
            var otherAxes = GetOtherAxes(axis1);
            return (otherAxes.Item1 == axis2) ? otherAxes.Item2 : otherAxes.Item1;
        }

        public static int GetThirdAxis(int axis1, int axis2)
        {
            return (int) GetThirdAxis((Axis)axis1, (Axis)axis2);
        }

        public static (Axis, Axis) GetPlaneAxes(Plane plane)
        {
            switch (plane)
            {
                case Plane.XY: return (Axis.X, Axis.Y);
                case Plane.XZ: return (Axis.X, Axis.Z);
                default: return (Axis.Y, Axis.Z);
            }
        }

        public static (int, int) GetPlaneAxesInt(Plane plane)
        {
            var axes = GetPlaneAxes(plane);
            return ((int)axes.Item1, (int)axes.Item2);
        }

        public static Vector3 Mul(Vector3 a, Vector3 b)
        {
            a.x *= b.x;
            a.y *= b.y;
            a.z *= b.z;
            return a;
        }

        public static Vector3 Div(Vector3 a, Vector3 b)
        {
            a.x /= b.x;
            a.y /= b.y;
            a.z /= b.z;
            return a;
        }

        public static float Min(Vector3 a)
        {
            return Mathf.Min(a.x, a.y, a.z);
        }

        public static Bounds RotateBounds(Bounds bounds, Quaternion rotation)
        {
            if (rotation == Quaternion.identity) return bounds;

            var rotatedCenter = rotation * bounds.center;
            var p1 = rotation * bounds.max;
            var p2 = rotation * new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
            var p3 = rotation * new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            var p4 = rotation * new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            var p5 = rotation * new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
            var p6 = rotation * new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            var p7 = rotation * new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
            var p8 = rotation * bounds.min;

            var rotatedBounds = new Bounds(rotatedCenter, Vector3.zero);
            rotatedBounds.Encapsulate(p1);
            rotatedBounds.Encapsulate(p2);
            rotatedBounds.Encapsulate(p3);
            rotatedBounds.Encapsulate(p4);
            rotatedBounds.Encapsulate(p5);
            rotatedBounds.Encapsulate(p6);
            rotatedBounds.Encapsulate(p7);
            rotatedBounds.Encapsulate(p8);
            return rotatedBounds;
        }

        public static Bounds CreateRotatedBounds(Vector3 center, Vector3 size, Quaternion rotation)
        {
            if (rotation == Quaternion.identity) return new Bounds(center, size);
            var bounds = RotateBounds(new Bounds(Vector3.zero, size), rotation);
            bounds.center = center;
            return bounds;
        }

        public static Bounds ScaleBounds(Bounds bounds, Vector3 scale)
        {
            bounds.center = Math.Mul(bounds.center, scale);
            bounds.size = Math.Mul(bounds.size, scale);
            return bounds;
        }

        private static float Align(float size, Align align)
        {
            if (align == global::Flexalon.Align.Start)
            {
                return -size * 0.5f;
            }
            else if (align == global::Flexalon.Align.End)
            {
                return size * 0.5f;
            }

            return 0;
        }

        public static float Align(float childSize, float parentSize, Align parentAlign, Align childAlign)
        {
            return Align(parentSize, parentAlign) - Align(childSize, childAlign);
        }

        public static float Align(float childSize, float parentSize, Align align)
        {
            return Align(childSize, parentSize, align, align);
        }

        public static float Align(Vector3 size, int axis, Align align)
        {
            return Align(size[axis], align);
        }

        public static float Align(Vector3 childSize, Vector3 parentSize, int axis, Align align)
        {
            return Align(childSize[axis], parentSize[axis], align);
        }

        public static float Align(Vector3 childSize, Vector3 parentSize, int axis, Align parentAlign, Align childAlign)
        {
            return Align(childSize[axis], parentSize[axis], parentAlign, childAlign);
        }

        public static float Align(Vector3 childSize, Vector3 parentSize, Axis axis, Align align)
        {
            return Align(childSize, parentSize, (int)axis, align);
        }

        public static float Align(Vector3 childSize, Vector3 parentSize, Axis axis, Align parentAlign, Align childAlign)
        {
            return Align(childSize, parentSize, (int)axis, parentAlign, childAlign);
        }

        public static Vector3 Align(Vector3 size, Align horizontal, Align vertical, Align depth)
        {
            return new Vector3(
                Align(size, 0, horizontal),
                Align(size, 1, vertical),
                Align(size, 2, depth));
        }

        public static Vector3 Align(Vector3 childSize, Vector3 parentSize, Align parentHorizontal, Align parentVertical, Align parentDepth, Align childHorizontal, Align childVertical, Align childDepth)
        {
            return Align(parentSize, parentHorizontal, parentVertical, parentDepth) -
                Align(childSize, childHorizontal, childVertical, childDepth);
        }

        public static Vector3 Align(Vector3 childSize, Vector3 parentSize, Align horizontal, Align vertical, Align depth)
        {
            return Align(parentSize, horizontal, vertical, depth) -
                Align(childSize, horizontal, vertical, depth);
        }

        public static Bounds MeasureComponentBounds(Bounds componentBounds, FlexalonNode node, Vector3 size)
        {
            componentBounds.size = Vector3.Max(componentBounds.size, Vector3.one * 0.0001f);
            var bounds = componentBounds;

            bool componentX = node.GetSizeType(Axis.X) == SizeType.Component;
            bool componentY = node.GetSizeType(Axis.Y) == SizeType.Component;
            bool componentZ = node.GetSizeType(Axis.Z) == SizeType.Component;

            var scale = Math.Div(size, bounds.size);
            float minScale = (componentX && componentY && componentZ) ? 1 : float.MaxValue;
            if (!componentX)
            {
                minScale = Mathf.Min(minScale, scale.x);
            }

            if (!componentY)
            {
                minScale = Mathf.Min(minScale, scale.y);
            }

            if (!componentZ)
            {
                minScale = Mathf.Min(minScale, scale.z);
            }

            scale = Vector3.one * minScale;

            bounds.size = new Vector3(
                componentX ? bounds.size.x * scale.x : size.x,
                componentY ? bounds.size.y * scale.y : size.y,
                componentZ ? bounds.size.z * scale.z : size.z);

            bounds.center = Math.Mul(bounds.center, Math.Div(bounds.size, componentBounds.size));

            return bounds;
        }

        public static Bounds MeasureComponentBounds2D(Bounds componentBounds, FlexalonNode node, Vector3 size)
        {
            componentBounds.size = Vector3.Max(componentBounds.size, Vector3.one * 0.0001f);
            var bounds = componentBounds;

            bool componentX = node.GetSizeType(Axis.X) == SizeType.Component;
            bool componentY = node.GetSizeType(Axis.Y) == SizeType.Component;
            bool componentZ = node.GetSizeType(Axis.Z) == SizeType.Component;

            var scale = Math.Div(size, bounds.size);
            float minScale = (componentX && componentY) ? 1 : float.MaxValue;
            if (!componentX)
            {
                minScale = Mathf.Min(minScale, scale.x);
            }

            if (!componentY)
            {
                minScale = Mathf.Min(minScale, scale.y);
            }

            scale = Vector3.one * minScale;

            bounds.size = new Vector3(
                componentX ? bounds.size.x * scale.x : size.x,
                componentY ? bounds.size.y * scale.y : size.y,
                componentZ ? 0 : size.z);

            bounds.center = Math.Mul(bounds.center, Math.Div(bounds.size, componentBounds.size));

            return bounds;
        }

        public static Vector3 Abs(Vector3 v)
        {
            v.x = Mathf.Abs(v.x);
            v.y = Mathf.Abs(v.y);
            v.z = Mathf.Abs(v.z);
            return v;
        }

        public static float DistanceBoxToPoint(Bounds bounds, Quaternion rotation, Vector3 point)
        {
            var p = Quaternion.Inverse(rotation) * point;
            var dx = Mathf.Max(bounds.min.x - p.x, 0, p.x - bounds.max.x);
            var dy = Mathf.Max(bounds.min.y - p.y, 0, p.y - bounds.max.y);
            var dz = Mathf.Max(bounds.min.y - p.z, 0, p.z - bounds.max.z);
            return Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static bool Intersect(Bounds bounds, Quaternion rotation, Ray ray, out Vector3 hitPosition)
        {
            hitPosition = Vector3.zero;
            var inverseRotation = Quaternion.Inverse(rotation);
            ray.origin = inverseRotation * ray.origin;
            ray.direction = inverseRotation * ray.direction;

            var txmin = ((ray.direction.x > 0 ? bounds.min.x : bounds.max.x) - ray.origin.x) / ray.direction.x;
            var txmax = ((ray.direction.x > 0 ? bounds.max.x : bounds.min.x) - ray.origin.x) / ray.direction.x;
            var tymin = ((ray.direction.y > 0 ? bounds.min.y : bounds.max.y) - ray.origin.y) / ray.direction.y;
            var tymax = ((ray.direction.y > 0 ? bounds.max.y : bounds.min.y) - ray.origin.y) / ray.direction.y;
            var tzmin = ((ray.direction.z > 0 ? bounds.min.z : bounds.max.z) - ray.origin.z) / ray.direction.z;
            var tzmax = ((ray.direction.z > 0 ? bounds.max.z : bounds.min.z) - ray.origin.z) / ray.direction.z;

            if (txmin > tymax || txmin > tzmax || tymin > txmax || tymin > tzmax || tzmin > txmax || tzmin > tymax)
            {
                return false;
            }

            float tmin = Mathf.Max(tymin, txmin, tzmin);
            hitPosition = ray.direction * tmin;
            return true;
        }
    }
}