using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace GPFlowSequenceDiagram
{
    public class CFDIStart: DiagramItem
    {
        public CFDIStart(DiagramElement parent)
            : base(parent)
        {
            OriginPoint.DE_SetCursor(Keys.None, Cursors.SizeAll);
            EndPoint.WantsConnect = ConnectivityWanted.StartPointWanted;
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

            g.DrawEllipse(p1, OriginPoint.X - 8, OriginPoint.Y - 8, 16, 16);
            g.DrawEllipse(p1, EndPoint.X - 3, EndPoint.Y - 3, 6, 6);
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 8, EndPoint.X, EndPoint.Y - 3);

            return UsedRectangle.Size;

        }

        public override void DE_FindElements(DiagramContext context)
        {
            if (context.FoundElement != null)
                return;

            float x = (OriginPoint.X - context.PagePoint.X);
            float y = (OriginPoint.Y - context.PagePoint.Y);
            if ((x * x + y * y) <= 64)
            {
                context.InsertElement(OriginPoint);
            }

            if (context.FoundElement == null)
                base.DE_FindElements(context);
        }

    }
}
