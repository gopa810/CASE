using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using GPFlowSequenceDiagram.ItemParts;

namespace GPFlowSequenceDiagram
{
    public class CFDILoopForeach : CFDILoopBase
    {
        public ItemPartOutput trueEnding = null;
        protected ItemPartSimpleRect conditionBox = null;

        public StringItemArea ConditionText = new StringItemArea();

        public CFDILoopForeach(DiagramElement parent)
            : base(parent)
        {
            conditionBox = new ItemPartSimpleRect(this, DiagramItemPart.ET_PRIMARY_AREA);
            OriginPoint.DE_SetCursor(Keys.None, Cursors.SizeAll);
            OriginPoint.WantsConnect = ConnectivityWanted.EndPointWanted;
            trueEnding = new ItemPartOutput(this, DiagramItemPart.ET_ENDING_POINT);
            trueEnding.WantsConnect = ConnectivityWanted.StartPointWanted;
            EndPoint.WantsConnect = ConnectivityWanted.StartPointWanted;
        }

        public override SizeF DE_DrawShape(DiagramDrawingContext ctx, HighlightType highType)
        {
            Graphics g = ctx.Graphics;
            // draw condition text
            SizeF sz = ConditionText.GetSize(g);
            SizeF szCond = ConditionText.GetSize(g);
            SizeF szTrueBranch = trueEnding.CalculateSuccessorSize(ctx);
            RectangleF box = new RectangleF();


            float boxTop = OriginPoint.Y + DrawProperties.p_drawingStep;
            float boxBottom = boxTop + szCond.Height;

            box.Height = boxBottom - boxTop;
            box.Width = Math.Max(szCond.Width, szTrueBranch.Width);
            box.X = OriginPoint.X - box.Width / 2;
            box.Y = boxTop;

            ExitWayX = box.Right + 16;
            ReturnWayX = box.Left - 16;

            conditionBox.SetRectangle(box);

            float trueX = OriginPoint.X;
            float branchStartY = box.Bottom + DrawProperties.p_drawingStep;

            trueEnding.X = trueX;
            trueEnding.Y = branchStartY;

            ItemPartOutput last1 = trueEnding.GetLastOutputItem();

            EndPoint.SetPosition(OriginPoint.X, last1.Y + 32);

            UsedRectangle = new RectangleF(OriginPoint.X - box.Width / 2 - 32, OriginPoint.Y, box.Width + 32, EndPoint.Y - OriginPoint.Y);

            if (highType == HighlightType.NotDraw)
                return UsedRectangle.Size;

            Pen p1 = GetPenForHighlight(highType);
            Brush b1 = GetBrushForHighlight(highType);

            // draw origin circle
            g.DrawEllipse(p1, OriginPoint.X - 3, OriginPoint.Y - 3, 6, 6);
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 3, OriginPoint.X, OriginPoint.Y + DrawProperties.p_drawingStep);


            // draw bounding rectangle
            g.DrawRectangle(p1, box.Left, box.Top, box.Width, box.Height);

            // draw text in rectangle
            ConditionText.DrawAtPoint(g, box.Left, box.Top);

            g.DrawLine(p1, trueX, box.Bottom, trueX, branchStartY - 3);
            g.DrawEllipse(p1, trueX - 3, branchStartY - 3, 6, 6);

            g.DrawString("foreach", DrawProperties.fontSmallTitles, Brushes.Black, OriginPoint.X + 6, OriginPoint.Y);


            // drawing lines from end of branches to the end of if
            if (last1.WantsConnect == ConnectivityWanted.StartPointWanted)
            {
                g.DrawLine(p1, EndPoint.X, last1.Y + 3, EndPoint.X, EndPoint.Y - 21);
                g.DrawLine(p1, EndPoint.X, EndPoint.Y - 21, box.Left - 16, EndPoint.Y - 21);
                g.DrawLine(p1, box.Left - 16, EndPoint.Y - 21, box.Left - 16, box.Top - DrawProperties.p_drawingStep / 2);
                DrawArrow(g, p1, b1, box.Left - 16, box.Top - DrawProperties.p_drawingStep / 2, OriginPoint.X, box.Top - DrawProperties.p_drawingStep / 2);
            }

            g.DrawLine(p1, box.Right, box.Top + box.Height / 2, box.Right + 16, box.Top + box.Height / 2);
            g.DrawLine(p1, box.Right + 16, box.Top + box.Height / 2, box.Right + 16, EndPoint.Y - 10);
            g.DrawLine(p1, box.Right + 16, EndPoint.Y - 10, EndPoint.X, EndPoint.Y - 10);
            DrawArrow(g, p1, b1, EndPoint.X, EndPoint.Y - 10, EndPoint.X, EndPoint.Y - 3);
            //g.DrawLine(p2, EndPoint.X, EndPoint.Y - 10, EndPoint.X, EndPoint.Y - 3);
            g.DrawEllipse(p1, EndPoint.X - 3, EndPoint.Y - 3, 6, 6);

            return UsedRectangle.Size;
        }

        public override void DE_FindElements(DiagramContext context)
        {
            if (context.FoundElement != null)
                return;

            if (context.FoundElement == null && trueEnding.NearTo(context.PagePoint))
                context.InsertElement(trueEnding);
            if (context.FoundElement == null && conditionBox.Contains(context.PagePoint))
                context.InsertElement(conditionBox);
            if (context.FoundElement == null)
                base.DE_FindElements(context);
        }

        public override void RelayoutNextShapes(DiagramDrawingContext g)
        {
            // recalculation of dimensions occurs in base implementation
            base.RelayoutNextShapes(g);

            // repositioning branches if necessary
            if (trueEnding.RefItem != null && trueEnding.RefItem.Item != null)
            {
                trueEnding.RefItem.Item.SetOriginPoint(g, trueEnding.X, trueEnding.Y);
            }

        }


    }
}
