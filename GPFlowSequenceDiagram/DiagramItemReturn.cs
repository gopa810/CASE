using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class DiagramItemReturn: Item
    {
        public DiagramItemReturn()
        {
            OriginPoint.SetCursor(Keys.None, Cursors.SizeAll);
            OriginPoint.WantsConnect = ConnectivityWanted.EndPointWanted;
        }

        public override void Paint(System.Drawing.Graphics g, HighlightType highType)
        {
            Pen p1 = GetPenForHighlight(highType);
            Brush b1 = GetBrushForHighlight(highType);

            g.DrawEllipse(p1, EndPoint.X - 6, EndPoint.Y - 6, 12, 12);
            g.DrawEllipse(p1, OriginPoint.X - 3, OriginPoint.Y - 3, 6, 6);
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 3, EndPoint.X, EndPoint.Y - 6);
            g.DrawLine(p1, EndPoint.X - 4, EndPoint.Y - 4, EndPoint.X + 4, EndPoint.Y + 4);
            g.DrawLine(p1, EndPoint.X + 4, EndPoint.Y - 4, EndPoint.X - 4, EndPoint.Y + 4);
        }

        public override void ItemPartDidChanged(ItemPart changeItem)
        {
            if (changeItem.PartType == ItemPart.ORIGIN_POINT)
            {
                EndPoint.X = OriginPoint.X;
                EndPoint.Y = OriginPoint.Y + 32;
            }
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

    }
}
