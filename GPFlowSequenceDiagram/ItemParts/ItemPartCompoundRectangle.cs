using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GPFlowSequenceDiagram
{
    public class ItemPartCompoundRectangle: DiagramItemPart
    {
        public ItemPartFloat Top = null;
        public ItemPartFloat Right = null;
        public ItemPartFloat Bottom = null;
        public ItemPartFloat Left = null;

        public ItemPartCompoundRectangle(): base(null)
        {
        }

        public ItemPartCompoundRectangle(DiagramElement it)
            : base(it)
        {
        }

        public ItemPartCompoundRectangle(DiagramElement it, int type)
            : base(it, type)
        {
        }

        public ItemPartCompoundRectangle(DiagramElement it, int type, ItemPartFloat pLeft, ItemPartFloat pTop, ItemPartFloat pRight, ItemPartFloat pBottom)
            : base(it, type)
        {
            Right = pRight;
            Top = pTop;
            Left = pLeft;
            Bottom = pBottom;
        }

        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(Left.Value, Top.Value, Right.Value - Left.Value, Bottom.Value - Top.Value);
            }
        }
    }
}
