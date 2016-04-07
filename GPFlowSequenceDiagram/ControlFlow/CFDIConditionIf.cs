using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using GPFlowSequenceDiagram.ItemParts;

namespace GPFlowSequenceDiagram
{
    public class CFDIConditionIf: DiagramItem
    {
        protected ItemPartOutput trueEnding = null;
        protected ItemPartOutput falseEnding = null;
        protected ItemPartSimpleRect conditionBox = null;

        public StringItemArea ConditionText = new StringItemArea();
        public StringItemArea TrueText = new StringItemArea();
        public StringItemArea FalseText = new StringItemArea();

        public CFDIConditionIf(DiagramElement de)
            : base(de)
        {
            conditionBox = new ItemPartSimpleRect(this, DiagramItemPart.ET_PRIMARY_AREA);
            OriginPoint.DE_SetCursor(Keys.None, Cursors.SizeAll);
            OriginPoint.WantsConnect = ConnectivityWanted.EndPointWanted;
            trueEnding = new ItemPartOutput(this, DiagramItemPart.ET_ENDING_POINT);
            falseEnding = new ItemPartOutput(this, DiagramItemPart.ET_ENDING_POINT);
            trueEnding.WantsConnect = ConnectivityWanted.StartPointWanted;
            falseEnding.WantsConnect = ConnectivityWanted.StartPointWanted;
            EndPoint.WantsConnect = ConnectivityWanted.StartPointWanted;

            TrueText.Text = "TRUE";
            TrueText.Padding.Horizontal = 1;
            FalseText.Text = "FALSE";
            FalseText.Padding.Horizontal = 1;
        }

        public override SizeF DE_DrawShape(DiagramDrawingContext ctx, HighlightType highType)
        {
            Graphics g = ctx.Graphics;

            // draw condition text
            SizeF sz = ConditionText.GetSize(g);
            SizeF szCond = ConditionText.GetSize(g);
            SizeF szTrue = TrueText.GetSize(g);
            SizeF szFalse = FalseText.GetSize(g);
            SizeF szTrueBranch = trueEnding.CalculateSuccessorSize(ctx);
            SizeF szFalseBranch = falseEnding.CalculateSuccessorSize(ctx);
            RectangleF box = new RectangleF();


            szTrue.Width = Math.Max(szTrue.Width, szTrueBranch.Width + 32);
            szTrueBranch.Width = szTrue.Width;
            szFalse.Width = Math.Max(szFalse.Width, szFalseBranch.Width + 32);
            szFalseBranch.Width = szFalse.Width;

            float boxTop = OriginPoint.Y + DrawProperties.p_drawingStep;
            float boxBottom = boxTop + szCond.Height + szTrue.Height;
            
            box.Height = boxBottom - boxTop;
            box.Width = Math.Max(szCond.Width, szFalse.Width + szTrue.Width);
            box.X = OriginPoint.X - box.Width / 2;
            box.Y = boxTop;

            conditionBox.SetRectangle(box);

            float trueX = box.Left + szTrue.Width / 2;
            float falseX = box.Left + szTrue.Width + szFalse.Width / 2;
            float branchStartY = box.Bottom + DrawProperties.p_drawingStep;

            trueEnding.X = trueX;
            trueEnding.Y = branchStartY;
            falseEnding.X = falseX;
            falseEnding.Y = branchStartY;

            ItemPartOutput last1 = trueEnding.GetLastOutputItem();
            ItemPartOutput last2 = falseEnding.GetLastOutputItem();

            EndPoint.SetPosition(OriginPoint.X, Math.Max(last1.Y, last2.Y) + 32);

            UsedRectangle = new RectangleF(OriginPoint.X - box.Width / 2 - 16, OriginPoint.Y, 
                box.Width + 32, EndPoint.Y - OriginPoint.Y);

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
            TrueText.DrawAtPointWithWidth(g, box.Left, box.Top + sz.Height, szTrue.Width);
            FalseText.DrawAtPointWithWidth(g, box.Left + szTrue.Width, box.Top + sz.Height, szFalse.Width);

            g.DrawLine(p1, trueX, box.Bottom, trueX, branchStartY - 3);
            g.DrawEllipse(p1, trueX - 3, branchStartY - 3, 6, 6);

            g.DrawLine(p1, falseX, box.Bottom, falseX, branchStartY - 3);
            g.DrawEllipse(p1, falseX - 3, branchStartY - 3, 6, 6);

            g.DrawString("if", DrawProperties.fontSmallTitles, Brushes.Black, OriginPoint.X + 6, OriginPoint.Y);


            // drawing lines from end of branches to the end of if
            if (last1.WantsConnect == ConnectivityWanted.StartPointWanted)
            {
                g.DrawLine(p1, last1.X, last1.Y + 3, last1.X, EndPoint.Y - 16);
                g.DrawLine(p1, last1.X, EndPoint.Y - 16, EndPoint.X, EndPoint.Y - 16);
            }
            if (last2.WantsConnect == ConnectivityWanted.StartPointWanted)
            {
                g.DrawLine(p1, last2.X, last2.Y + 3, last2.X, EndPoint.Y - 16);
                g.DrawLine(p1, last2.X, EndPoint.Y - 16, EndPoint.X, EndPoint.Y - 16);
            }
            DrawArrow(g, p1, b1, EndPoint.X, EndPoint.Y - 16, EndPoint.X, EndPoint.Y - 3);
            g.DrawEllipse(p1, EndPoint.X - 3, EndPoint.Y - 3, 6, 6);

            return UsedRectangle.Size;
        }

        public override void DE_FindElements(DiagramContext context)
        {
            if (trueEnding.NearTo(context.PagePoint))
                context.InsertElement(trueEnding);
            if (context.FoundElement == null && falseEnding.NearTo(context.PagePoint))
                context.InsertElement(falseEnding);
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
            if (falseEnding.RefItem != null && falseEnding.RefItem.Item != null)
            {
                falseEnding.RefItem.Item.SetOriginPoint(g, falseEnding.X, falseEnding.Y);
            }

        }


        public override void DE_PopulateContextMenu(ContextMenuStrip cms, DiagramContext context)
        {
            ToolStripMenuItem tsi = new ToolStripMenuItem("Edit condition", null);
            tsi.Name = "edit if condition";
            tsi.Click += new EventHandler(tsi_Click);
            cms.Items.Add(tsi);

            cms.Items.Add(new ToolStripSeparator());

            tsi = new ToolStripMenuItem("Insert AND", null);
            tsi.Name = "if new and";
            tsi.Click += new EventHandler(tsi_Click);
            cms.Items.Add(tsi);

            tsi = new ToolStripMenuItem("Insert OR", null);
            tsi.Name = "if new or";
            tsi.Click += new EventHandler(tsi_Click);
            cms.Items.Add(tsi);


        }

        public void tsi_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            if (tsmi.Name == "if new and")
            {
            }
            else if (tsmi.Name == "if new or")
            {
            }
        }

    }
}
