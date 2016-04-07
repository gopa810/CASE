using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using GPFlowSequenceDiagram.ItemParts;

namespace GPFlowSequenceDiagram
{
    public class CFDIProcesss: DiagramItem
    {
        public StringItemArea ProcessName = new StringItemArea();
        public StringItemArea ReturnDataType = new StringItemArea();
        public StringItemArea ProcessDescription = new StringItemArea();

        public ItemPartSimpleRect mainArea = null;

        public CFDIProcesss(DiagramElement parent)
            : base(parent)
        {
            mainArea =  new ItemPartSimpleRect(this, DiagramItemPart.ET_PRIMARY_AREA);
            OriginPoint.DE_SetCursor(Keys.None, Cursors.SizeAll);
            OriginPoint.WantsConnect = ConnectivityWanted.EndPointWanted;
            EndPoint.WantsConnect = ConnectivityWanted.StartPointWanted;
        }

        public override SizeF DE_DrawShape(DiagramDrawingContext ctx, HighlightType highType)
        {
            Graphics g = ctx.Graphics;

            SizeF szName = ProcessName.GetSize(g);
            SizeF szDesc = ProcessDescription.GetSize(g);
            SizeF szReturn = ReturnDataType.GetSize(g);

            float topY = OriginPoint.Y + DrawProperties.p_drawingStep;
            float bottomY = topY + szName.Height + szDesc.Height + szReturn.Height;
            float width = Math.Max(szName.Width, szDesc.Width);
            width = Math.Max(width, szReturn.Width);

            EndPoint.X = OriginPoint.X;
            EndPoint.Y = bottomY + DrawProperties.p_drawingStep;

            UsedRectangle = new RectangleF(OriginPoint.X - width / 2 - 16, OriginPoint.Y,
                width + 32, EndPoint.Y - OriginPoint.Y);

            if (highType == HighlightType.NotDraw)
                return UsedRectangle.Size;

            RectangleF r = new RectangleF(OriginPoint.X - width/2, topY, width, bottomY - topY);
            mainArea.SetRectangle(r);

            Pen p1 = GetPenForHighlight(highType);
            Brush b1 = GetBrushForHighlight(highType);

            // draw origin
            g.DrawEllipse(p1, OriginPoint.X - 3, OriginPoint.Y - 3, 6, 6);
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 3, OriginPoint.X, r.Y);
            // draw rectangle
            g.FillRectangle(b1, r);
            g.DrawRectangle(p1, r.X, r.Y, r.Width, r.Height);

            // draw texts
            ProcessName.DrawAtPointWithWidth(g, r.Left, topY, width);
            ProcessDescription.DrawAtPointWithWidth(g, r.Left, topY + szName.Height, width);
            ReturnDataType.DrawAtPointWithWidth(g, r.Left, topY + szName.Height + szDesc.Height, width);

            // draw ending line
            g.DrawLine(p1, OriginPoint.X, bottomY, EndPoint.X, EndPoint.Y - 3);
            g.DrawEllipse(p1, EndPoint.X - 3, EndPoint.Y - 3, 6, 6);

            return UsedRectangle.Size;

        }

        public override void DE_FindElements(DiagramContext context)
        {
            if (context.FoundElement != null)
                return;
            if (mainArea.Contains(context.PagePoint))
                context.InsertElement(mainArea);
            if (context.FoundElement != null)
                return;
            base.DE_FindElements(context);
        }
    }
}
