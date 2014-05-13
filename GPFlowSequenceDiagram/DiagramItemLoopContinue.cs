using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class DiagramItemLoopContinue: DiagramItemLoopBreak
    {
        public override void Paint(Graphics g, HighlightType highType)
        {
            Pen p1 = GetPenForHighlight(highType);
            Brush b1 = GetBrushForHighlight(highType);

            g.DrawEllipse(p1, EndPoint.X - 6, EndPoint.Y - 6, 12, 12);
            g.DrawEllipse(p1, OriginPoint.X - 3, OriginPoint.Y - 3, 6, 6);
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 3, EndPoint.X, EndPoint.Y - 6);
            DrawArrow(g, p1, b1, EndPoint.X - 6, EndPoint.Y, EndPoint.X - 16, EndPoint.Y);

            ItemWithLoop loopItem = LoopItem;
            if (loopItem != null)
            {
                DrawArrow(g, p1, b1, loopItem.BorderLeft + 16, EndPoint.Y,
                    loopItem.BorderLeft, EndPoint.Y);
            }
        }

        public override RectangleAnchored DrawingRectangle
        {
            get
            {
                return new RectangleAnchored(OriginPoint.X, OriginPoint.Y, 32, 8,
                    EndPoint.Y - OriginPoint.Y);
            }
        }
    }
}
