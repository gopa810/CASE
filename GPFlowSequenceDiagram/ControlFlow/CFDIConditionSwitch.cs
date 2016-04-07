using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace GPFlowSequenceDiagram
{
    public class CFDIConditionSwitch: DiagramItem
    {
        public class SwitchBranch
        {
            public String label = "";
            public ItemPartSimpleRect labelBox = new ItemPartSimpleRect();
            public ItemPartOutput branchEnding = null;
            public ItemPartOutput lastOut = null;
            public RectangleAnchored rectArea = null;

            public void SetY(float py, Graphics g)
            {
                labelBox.Top = py + 32;
                labelBox.Size = g.MeasureString(label, DrawProperties.fontSmallTitles);
                branchEnding.Y = labelBox.Bottom + 16;
            }

            public void SetX(float px)
            {
                labelBox.CenterX = px;
                branchEnding.X = px;
                if (branchEnding.RefItem != null)
                {
                    branchEnding.RefItem.Point = branchEnding.Point;
                }
            }

            public float LeftSideWidth
            {
                get
                {
                    if (rectArea == null)
                        return labelBox.LeftSideWidth;
                    return Math.Max(rectArea.LeftSideWidth, labelBox.LeftSideWidth);
                }
            }

            public float RightSideWidth
            {
                get
                {
                    if (rectArea == null)
                        return labelBox.RightSideWidth;
                    return Math.Max(rectArea.RightSideWidth, labelBox.RightSideWidth);
                }
            }

            public float Height
            {
                get
                {
                    if (rectArea == null)
                        return labelBox.Height + 32;
                    return labelBox.Height + 32 + rectArea.Height;
                }
            }
            public float Width
            {
                get
                {
                    if (rectArea == null)
                        return labelBox.Width;
                    return Math.Max(rectArea.Width, labelBox.Width);
                }
            }
        }

        protected List<SwitchBranch> branches = new List<SwitchBranch>();
        protected SwitchBranch newBranch = new SwitchBranch();
        protected SwitchBranch defaultBranch = new SwitchBranch();

        protected ItemPartSimpleRect conditionBox = null;

        public CFDIConditionSwitch(DiagramElement de): base(de)
        {
            conditionBox = new ItemPartSimpleRect(this, DiagramItemPart.ET_PRIMARY_AREA);
            OriginPoint.DE_SetCursor(Keys.None, Cursors.SizeAll);
            OriginPoint.WantsConnect = ConnectivityWanted.EndPointWanted;

            newBranch.branchEnding = new ItemPartOutput(this, DiagramItemPart.ET_ENDING_POINT);
            newBranch.branchEnding.WantsConnect = ConnectivityWanted.StartPointWanted;
            newBranch.label = "<new>";

            defaultBranch.branchEnding = new ItemPartOutput(this, DiagramItemPart.ET_ENDING_POINT);
            defaultBranch.branchEnding.WantsConnect = ConnectivityWanted.StartPointWanted;
            defaultBranch.label = "<default>";

            EndPoint.WantsConnect = ConnectivityWanted.StartPointWanted;
        }


        public string ConditionText
        {
            get
            {
                return "swithLabel";
            }
        }

        public override SizeF DE_DrawShape(DiagramDrawingContext ctx, HighlightType highType)
        {
            Graphics g = ctx.Graphics;
            Pen p1 = GetPenForHighlight(highType);
            Brush b1 = GetBrushForHighlight(highType);

            PointF cn = GetPointOnPicoForSide(ShapeSide.Top);

            // draw origin circle
            g.DrawEllipse(p1, OriginPoint.X - 3, OriginPoint.Y - 3, 6, 6);
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 3, OriginPoint.X, conditionBox.Top);
            if (!conditionBox.Size.IsEmpty)
            {
                g.DrawRectangle(p1, conditionBox.Left, conditionBox.Top,
                    conditionBox.Width, conditionBox.Height);
                g.DrawString(ConditionText, DrawProperties.fontSmallTitles, Brushes.Black,
                    conditionBox.Left + 1,
                    conditionBox.Top + 1);
            }
            g.DrawLine(p1, cn.X, conditionBox.Bottom,
                cn.X, conditionBox.Bottom + DrawProperties.p_drawingStep - 8);
            g.DrawString("switch", DrawProperties.fontSmallTitles, Brushes.Black, OriginPoint.X + 6, OriginPoint.Y);
            // draw ending circles
            PointF pt1 = GetPointOnPicoForSide(ShapeSide.Bottom);
            g.DrawLine(p1, pt1.X, pt1.Y, pt1.X, pt1.Y + 16);

            SwitchBranch sb = newBranch;
            if (branches.Count > 0)
                sb = branches[0];

            g.DrawLine(p1, sb.branchEnding.X, pt1.Y + 16, defaultBranch.branchEnding.X, pt1.Y + 16);

            foreach (SwitchBranch sbe in branches)
            {
                DrawSwitchBranch(g, p1, pt1, sb);
            }

            newBranch.labelBox.Size = g.MeasureString(newBranch.label, DrawProperties.fontSmallTitles);
            defaultBranch.labelBox.Size = g.MeasureString(defaultBranch.label, DrawProperties.fontSmallTitles);
            DrawSwitchBranch(g, p1, pt1, newBranch);
            DrawSwitchBranch(g, p1, pt1, defaultBranch);

            PointF[] points = new PointF[] {
                GetPointOnPicoForSide(ShapeSide.Top),
                GetPointOnPicoForSide(ShapeSide.Right),
                GetPointOnPicoForSide(ShapeSide.Bottom),
                GetPointOnPicoForSide(ShapeSide.Left)
            };
            g.FillPolygon(b1, points);
            g.DrawPolygon(p1, points);

            bool minEndingXInit = false;
            float minEndingX = 1000;
            float maxEndingX = 0;
            foreach (SwitchBranch sbb in branches)
            {
                if (sbb.lastOut.WantsConnect == ConnectivityWanted.StartPointWanted)
                {
                    if (!minEndingXInit)
                    {
                        minEndingX = sbb.lastOut.X;
                        minEndingXInit = true;
                    }
                    DrawArrow(g, p1, Brushes.Black, sbb.lastOut.X, sbb.lastOut.Y + 3, sbb.lastOut.X, EndPoint.Y - 16);
                    maxEndingX = sbb.lastOut.X;
                }
            }

            if (defaultBranch.lastOut.WantsConnect == ConnectivityWanted.StartPointWanted)
            {
                DrawArrow(g, p1, Brushes.Black, defaultBranch.lastOut.X, defaultBranch.lastOut.Y + 3,
                    defaultBranch.lastOut.X, EndPoint.Y - 16);
                maxEndingX = defaultBranch.lastOut.X;
            }

            if (minEndingX > EndPoint.X)
                minEndingX = EndPoint.X;
            if (maxEndingX < EndPoint.X)
                maxEndingX = EndPoint.X;

            g.DrawLine(p1, minEndingX, EndPoint.Y - 16, maxEndingX, EndPoint.Y - 16);
            DrawArrow(g, p1, Brushes.Black, EndPoint.X, EndPoint.Y - 16, EndPoint.X, EndPoint.Y - 3);
            g.DrawEllipse(p1, EndPoint.X - 3, EndPoint.Y - 3, 6, 6);

            return new SizeF(32, EndPoint.Y - OriginPoint.Y);

        }

        private void DrawSwitchBranch(System.Drawing.Graphics g, Pen p1, PointF pt1, SwitchBranch sb)
        {
            g.DrawLine(p1, sb.labelBox.CenterX, pt1.Y + 16, sb.labelBox.CenterX, sb.labelBox.Top);
            g.DrawString(sb.label, DrawProperties.fontSmallTitles, Brushes.Black, sb.labelBox.Left + 1, sb.labelBox.Top + 1);
            g.DrawRectangle(p1, sb.labelBox.Left, sb.labelBox.Top, sb.labelBox.Right, sb.labelBox.Bottom);
            g.DrawLine(p1, sb.labelBox.CenterX, sb.labelBox.Bottom, sb.branchEnding.X, sb.branchEnding.Y - 3);
            g.DrawEllipse(p1, sb.branchEnding.X - 3, sb.branchEnding.Y - 3, 6, 6);
        }

        public PointF GetPointOnPicoForSide(ShapeSide side)
        {
            float cnX = OriginPoint.X;
            float cnY = conditionBox.Bottom + DrawProperties.p_drawingStep;

            switch (side)
            {
                case ShapeSide.Top:
                    return new PointF(cnX, cnY - 8);
                case ShapeSide.Right:
                    return new PointF(cnX + 8, cnY);
                case ShapeSide.Bottom:
                    return new PointF(cnX, cnY + 8);
                case ShapeSide.Left:
                    return new PointF(cnX - 8, cnY);
                default:
                    return new PointF(cnX, cnY);
            }

        }


        private static void RecalculateBranchRectangle(ref float maxHeight, SwitchBranch sb)
        {
            float thisHeight = 0;
            sb.lastOut = sb.branchEnding.GetLastOutputItem();
            sb.rectArea = new RectangleAnchored();// sb.branchEnding.CalculateSubordinatesDrawingRectangle();
            thisHeight = sb.Height;
            if (thisHeight > maxHeight)
                maxHeight = thisHeight;
        }


        public override void DE_FindElements(DiagramContext context)
        {
            foreach (SwitchBranch sb in branches)
            {
                if (context.FoundElement == null && sb.branchEnding.NearTo(context.PagePoint))
                    context.InsertElement(sb.branchEnding);
            }
            if (context.FoundElement == null && newBranch.branchEnding.NearTo(context.PagePoint))
                context.InsertElement(newBranch.branchEnding);
            if (context.FoundElement == null && defaultBranch.branchEnding.NearTo(context.PagePoint))
                context.InsertElement(defaultBranch.branchEnding);
            if (context.FoundElement == null && conditionBox.Contains(context.PagePoint))
                context.InsertElement(conditionBox);
            if (context.FoundElement == null)
                base.DE_FindElements(context);
        }

        public override void MouseMove(DiagramItemPart item, DiagramItemPart startValue, SizeF diff, DiagramMouseKeys keys)
        {
            if (item.ElementType == DiagramItemPart.ET_ORIGIN_POINT)
            {
                OriginPoint.X = (startValue as ItemPartPointF).X + diff.Width;
                OriginPoint.Y = (startValue as ItemPartPointF).Y + diff.Height;
            }
        }

    }
}
