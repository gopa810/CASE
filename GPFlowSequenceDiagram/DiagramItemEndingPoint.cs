using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;

namespace GPFlowSequenceDiagram
{
    public class DiagramItemEndingPoint
    {
        private Item refItem = null;
        private PointF refPoint = new PointF();
        private bool p_initd = false;

        public void SetItem(Item item)
        {
            p_initd = (item != null);
            refItem = item;
        }

        public void SetPoint(PointF pt)
        {
            p_initd = true;
            refPoint.X = pt.X;
            refPoint.Y = pt.Y;
            refItem = null;
        }

        public PointF EndPoint
        {
            get
            {
                if (refItem == null)
                    return refPoint;
                return refItem.OriginPoint.Point;
            }
            set
            {
                SetPoint(value);
            }
        }

        public bool HasItem
        {
            get
            {
                return refItem != null;
            }
        }

        public bool IsInitialized
        {
            get
            {
                return p_initd;
            }
        }

        public bool IsNear(PointF pt)
        {
            PointF my = EndPoint;
            return (Math.Abs(pt.X - my.X) + Math.Abs(pt.Y - my.Y)) < 8;
        }
    }
}
