using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class CFDILoopBreak: CFDIReturn
    {
        public CFDILoopBreak(DiagramElement de)
            : base(de)
        {
        }

        public override SizeF DE_DrawShape(DiagramDrawingContext ctx, HighlightType highType )
        {
            Graphics g = ctx.Graphics;
            EndPoint.X = OriginPoint.X;
            EndPoint.Y = OriginPoint.Y + 32;

            UsedRectangle = new RectangleF(OriginPoint.X - 16, OriginPoint.Y,
                32, 32);

            if (highType == HighlightType.NotDraw)
                return UsedRectangle.Size;

            Pen p1 = GetPenForHighlight(highType);
            Brush b1 = GetBrushForHighlight(highType);

            g.DrawEllipse(p1, EndPoint.X - 6, EndPoint.Y - 6, 12, 12);
            g.DrawEllipse(p1, OriginPoint.X - 3, OriginPoint.Y - 3, 6, 6);
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 3, EndPoint.X, EndPoint.Y - 6);
            DrawArrow(g, p1, b1, EndPoint.X + 6, EndPoint.Y, EndPoint.X + 16, EndPoint.Y);

            CFDILoopBase loopItem = LoopItem;
            if (loopItem != null)
            {
                DrawArrow(g, p1, b1, loopItem.ExitWayX - 16, EndPoint.Y,
                    loopItem.ExitWayX, EndPoint.Y);
            }

            return UsedRectangle.Size;

        }

        public virtual CFDILoopBase LoopItem
        {
            get
            {
                DiagramItem prev = PreviousItem;
                while (prev != null)
                {
                    if (prev is CFDILoopBase)
                        return prev as CFDILoopBase;
                    prev = prev.PreviousItem;
                }
                return null;
            }
        }

    }
}
