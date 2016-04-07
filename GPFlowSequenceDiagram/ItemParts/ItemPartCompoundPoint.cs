using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPFlowSequenceDiagram
{
    public class ItemPartCompoundPoint: DiagramItemPart
    {
        public ItemPartFloat X = null;
        public ItemPartFloat Y = null;

        public ItemPartCompoundPoint(): base(null)
        {
        }
        public ItemPartCompoundPoint(DiagramElement it): base(it)
        {
        }
        public ItemPartCompoundPoint(DiagramElement it, int type): base(it, type)
        {
        }
        public ItemPartCompoundPoint(DiagramElement it, int type, ItemPartFloat px, ItemPartFloat py)
            : base(it, type)
        {
            X = px;
            Y = py;
        }

        public override DiagramItemPart Copy()
        {
            ItemPartPointF p = new ItemPartPointF(Parent, ElementType);
            if (X != null)
                p.X = X.Value;
            if (Y != null)
                p.Y = Y.Value;
            return p;
        }
    }
}
