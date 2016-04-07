using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace GPFlowSequenceDiagram
{
    public class ItemPartPointF: DiagramItemPart
    {
        protected DiagramPoint p_pt = new DiagramPoint();

        public DiagramPoint Point
        {
            set
            {
                p_pt.X = value.X;
                p_pt.Y = value.Y;
            }
            get
            {
                return p_pt;
            }
        }

        public float X
        {
            get { return p_pt.X; }
            set 
            {
                p_pt.X = value;
            }
        }

        public float Y
        {
            get { return p_pt.Y; }
            set
            {
                p_pt.Y = value;
            }
        }

        public ItemPartPointF(): base(null)
        {
        }

        public ItemPartPointF(DiagramElement it)
            : base(it)
        {
        }

        public ItemPartPointF(DiagramElement it, int type): base(it)
        {
            ElementType = type;
        }

        public override DiagramItemPart Copy()
        {
            ItemPartPointF pt = new ItemPartPointF(Parent, ElementType);
            pt.X = X;
            pt.Y = Y;
            return pt;
        }

        public bool NearTo(DiagramPoint pt)
        {
            return ((Math.Abs(pt.X - X) + Math.Abs(pt.Y - Y)) < 8) ;
        }
    }
}
