using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class CFDIReturn: DiagramItem
    {
        public CFDIReturn(DiagramElement parent)
            : base(parent)
        {
            OriginPoint.DE_SetCursor(Keys.None, Cursors.SizeAll);
            OriginPoint.WantsConnect = ConnectivityWanted.EndPointWanted;
        }

        public override SizeF DE_DrawShape(DiagramDrawingContext ctx, HighlightType highType)
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
            g.DrawLine(p1, EndPoint.X - 4, EndPoint.Y - 4, EndPoint.X + 4, EndPoint.Y + 4);
            g.DrawLine(p1, EndPoint.X + 4, EndPoint.Y - 4, EndPoint.X - 4, EndPoint.Y + 4);

            return UsedRectangle.Size;

        }

    }
}
