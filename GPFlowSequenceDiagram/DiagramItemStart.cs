using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace GPFlowSequenceDiagram
{
    public class DiagramItemStart: Item
    {
        public DiagramItemStart()
        {
            OriginPoint.SetCursor(Keys.None, Cursors.SizeAll);
            EndPoint.WantsConnect = ConnectivityWanted.StartPointWanted;
        }

        public override void Paint(System.Drawing.Graphics g, HighlightType highType)
        {
            Pen p1 = GetPenForHighlight(highType);
            Brush b1 = GetBrushForHighlight(highType);

            g.DrawEllipse(p1, OriginPoint.X - 8, OriginPoint.Y - 8, 16, 16);
            g.DrawEllipse(p1, EndPoint.X - 3, EndPoint.Y - 3, 6, 6);
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 8, EndPoint.X, EndPoint.Y - 3);
        }

        public override void ItemPartDidChanged(ItemPart changeItem)
        {
            if (changeItem.PartType == ItemPart.ORIGIN_POINT)
            {
                SizeF diff = new SizeF(OriginPoint.X - EndPoint.X, OriginPoint.Y + 32 - EndPoint.Y);
                EndPoint.X = OriginPoint.X;
                EndPoint.Y = OriginPoint.Y + 32;
                if (EndPoint.MoveReferencedItemPart && EndPoint.RefItem != null)
                {
                    EndPoint.RefItem.Point = EndPoint.RefItem.Point + diff;
                    EndPoint.RefItem.Item.ItemPartDidChanged(EndPoint.RefItem);
                }
            }
        }

        public override ItemPart GetHitItem(PointF pt)
        {
            float x = (OriginPoint.X - pt.X);
            float y = (OriginPoint.Y - pt.Y);
            if ((x * x + y * y) <= 64)
                return OriginPoint;
            return base.GetHitItem(pt);
        }

        public override void MouseMove(ItemPart item, ItemPart startValue, SizeF diff, DiagramMouseKeys keys)
        {
            switch (item.PartType)
            {
                case ItemPart.ORIGIN_POINT:
                    if (startValue is ItemPartPointF)
                    {
                        ItemPartPointF ptx = startValue as ItemPartPointF;
                        OriginPoint.X = ptx.X + diff.Width;
                        OriginPoint.Y = ptx.Y + diff.Height;
                    }
                    ItemPartDidChanged(OriginPoint);
                    break;
            }
        }

        public override RectangleAnchored DrawingRectangle
        {
            get
            {
                return new RectangleAnchored(OriginPoint.X, OriginPoint.Y, 8, 8, EndPoint.Y - OriginPoint.Y + 8);
            }
        }

    }
}
