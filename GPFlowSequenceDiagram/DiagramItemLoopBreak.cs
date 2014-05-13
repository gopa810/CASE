using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class DiagramItemLoopBreak: DiagramItemReturn
    {
        public override void Paint(System.Drawing.Graphics g, HighlightType highType)
        {
            Pen p1 = GetPenForHighlight(highType);
            Brush b1 = GetBrushForHighlight(highType);

            g.DrawEllipse(p1, EndPoint.X - 6, EndPoint.Y - 6, 12, 12);
            g.DrawEllipse(p1, OriginPoint.X - 3, OriginPoint.Y - 3, 6, 6);
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 3, EndPoint.X, EndPoint.Y - 6);
            DrawArrow(g, p1, b1, EndPoint.X + 6, EndPoint.Y, EndPoint.X + 16, EndPoint.Y);

            ItemWithLoop loopItem = LoopItem;
            if (loopItem != null)
            {
                DrawArrow(g, p1, b1, loopItem.BorderRight - 16, EndPoint.Y,
                    loopItem.BorderRight, EndPoint.Y);
            }
        }

        public virtual ItemWithLoop LoopItem
        {
            get
            {
                Item prev = PreviousItem;
                while (prev != null)
                {
                    if (prev is ItemWithLoop)
                        return prev as ItemWithLoop;
                    prev = prev.PreviousItem;
                }
                return null;
            }
        }

        public override RectangleAnchored DrawingRectangle
        {
            get
            {
                return new RectangleAnchored(OriginPoint.X, OriginPoint.Y, 
                    16, 32, EndPoint.Y - OriginPoint.Y);
            }
        }
    }
}
