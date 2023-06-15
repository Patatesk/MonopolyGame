using UnityEngine;

namespace Flexalon
{
    [ExecuteAlways, AddComponentMenu("Flexalon/Flexalon Align Layout"), HelpURL("https://www.flexalon.com/docs/alignLayout")]
    public class FlexalonAlignLayout : LayoutBase
    {
        [SerializeField]
        private Align _horizontalAlign = Align.Center;
        public Align HorizontalAlign
        {
            get { return _horizontalAlign; }
            set { _horizontalAlign = value; _node.MarkDirty(); }
        }

        [SerializeField]
        private Align _verticalAlign = Align.Center;
        public Align VerticalAlign
        {
            get { return _verticalAlign; }
            set { _verticalAlign = value; _node.MarkDirty(); }
        }

        [SerializeField]
        private Align _depthAlign = Align.Center;
        public Align DepthAlign
        {
            get { return _depthAlign; }
            set { _depthAlign = value; _node.MarkDirty(); }
        }

        [SerializeField]
        private Align _horizontalPivot = Align.Center;
        public Align HorizontalPivot
        {
            get { return _horizontalPivot; }
            set { _horizontalPivot = value; _node.MarkDirty(); }
        }

        [SerializeField]
        private Align _verticalPivot = Align.Center;
        public Align VerticalPivot
        {
            get { return _verticalPivot; }
            set { _verticalPivot = value; _node.MarkDirty(); }
        }

        [SerializeField]
        private Align _depthPivot = Align.Center;
        public Align DepthPivot
        {
            get { return _depthPivot; }
            set { _depthPivot = value; _node.MarkDirty(); }
        }

        public override Bounds Measure(FlexalonNode node, Vector3 size)
        {
            FlexalonLog.Log("AlignMeasure | Size", node, size);

            Vector3 maxSize = Vector3.zero;
            foreach (var child in node.Children)
            {
                maxSize = Vector3.Max(maxSize, child.GetMeasureSize());
            }

            for (int i = 0; i < 3; i++)
            {
                if (node.GetSizeType(i) == SizeType.Layout)
                {
                    size[i] = maxSize[i];
                }
            }

            base.Measure(node, size);
            return new Bounds(Vector3.zero, size);
        }

        public override void Arrange(FlexalonNode node, Vector3 size)
        {
            FlexalonLog.Log("AlignArrange | Size", node, size);

            foreach (var child in node.Children)
            {
                child.SetRotationResult(Quaternion.identity);
                child.SetPositionResult(Math.Align(child.GetArrangeSize(), size,
                    _horizontalAlign, _verticalAlign, _depthAlign,
                    _horizontalPivot, _verticalPivot, _depthPivot));
            }
        }
    }
}