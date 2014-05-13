using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GPFlowSequenceDiagram
{
    public class ItemPartCompoundRectangle: ItemPart
    {
        public ItemPartFloat Top = null;
        public ItemPartFloat Right = null;
        public ItemPartFloat Bottom = null;
        public ItemPartFloat Left = null;

        public ItemPartCompoundRectangle()
        {
        }

        public ItemPartCompoundRectangle(Item it)
        {
            Item = it;
        }

        public ItemPartCompoundRectangle(Item it, int type)
        {
            Item = it;
            PartType = type;
        }

        public ItemPartCompoundRectangle(Item it, int type, ItemPartFloat pLeft, ItemPartFloat pTop, ItemPartFloat pRight, ItemPartFloat pBottom)
        {
            Item = it;
            PartType = type;
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
