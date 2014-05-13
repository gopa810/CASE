using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace GPFlowSequenceDiagram
{
    public class ItemPartPointF: ItemPart
    {
        protected PointF p_pt = new PointF();

        public PointF Point
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

        public ItemPartPointF()
        {
        }

        public ItemPartPointF(Item it, int type)
        {
            Item = it;
            PartType = type;
        }

        public override ItemPart Copy()
        {
            ItemPartPointF pt = new ItemPartPointF(Item, PartType);
            pt.X = X;
            pt.Y = Y;
            return pt;
        }

        public bool NearTo(PointF pt)
        {
            return ((Math.Abs(pt.X - X) + Math.Abs(pt.Y - Y)) < 8) ;
        }
    }
}
