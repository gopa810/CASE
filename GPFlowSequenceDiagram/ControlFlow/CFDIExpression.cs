using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class CFDIExpression: DiagramItem
    {
        public DiagramPageDataflow mainArea = null;
        public RectangleF mainAreaRectDrawn;

        public CFDIExpression(DiagramElement de)
            : base(de)
        {
            mainArea =  new DiagramPageDataflow(this);
            //mainArea.ElementType = DiagramItemPart.ET_PRIMARY_AREA;
            OriginPoint.DE_SetCursor(Keys.None, Cursors.SizeAll);
            OriginPoint.WantsConnect = ConnectivityWanted.EndPointWanted;
            EndPoint.WantsConnect = ConnectivityWanted.StartPointWanted;
        }

        public override SizeF DE_DrawShape(DiagramDrawingContext ctx, HighlightType highType)
        {
            Graphics g = ctx.Graphics;
            SizeF szArea = mainArea.DE_DrawShape(ctx, HighlightType.NotDraw);

            float topY = OriginPoint.Y + DrawProperties.p_drawingStep;
            float bottomY = topY + szArea.Height;
            float width = szArea.Width;

            EndPoint.X = OriginPoint.X;
            EndPoint.Y = bottomY + DrawProperties.p_drawingStep;

            UsedRectangle = new RectangleF(OriginPoint.X - width/2 - 16, OriginPoint.Y, 
                width + 32, EndPoint.Y - OriginPoint.Y);

            mainAreaRectDrawn = new RectangleF(OriginPoint.X - width / 2, topY, width, szArea.Height);

            if (highType == HighlightType.NotDraw)
                return UsedRectangle.Size;

            Pen p1 = GetPenForHighlight(highType);
            Pen p2 = GetDashPenForHighlight(highType);
            Brush b1 = GetBrushForHighlight(highType);

            // draw origin
            g.DrawEllipse(p1, OriginPoint.X - 3, OriginPoint.Y - 3, 6, 6);
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 3, OriginPoint.X, mainAreaRectDrawn.Y);
            // draw rectangle
            //g.FillRectangle(b1, r);
            g.DrawRectangle(p2, mainAreaRectDrawn.X, mainAreaRectDrawn.Y, 
                mainAreaRectDrawn.Width, mainAreaRectDrawn.Height);

            mainArea.PG_DrawPageInRect(ctx, highType, mainAreaRectDrawn);

            // draw ending line
            g.DrawLine(p1, OriginPoint.X, bottomY, EndPoint.X, EndPoint.Y - 3);
            g.DrawEllipse(p1, EndPoint.X - 3, EndPoint.Y - 3, 6, 6);

            return UsedRectangle.Size;

        }

        public override void DE_FindElements(DiagramContext context)
        {
            if (context.FoundElement != null)
                return;

            base.DE_FindElements(context);

            if (context.FoundElement == null 
                && mainAreaRectDrawn.Contains(context.PagePoint.X, context.PagePoint.Y))
            {
                mainArea.DE_FindElements(context);
                if (context.FoundElement == null)
                    context.InsertElement(mainArea);
            }

        }

    }
}
