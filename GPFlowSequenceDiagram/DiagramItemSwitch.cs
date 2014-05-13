using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace GPFlowSequenceDiagram
{
    public class DiagramItemSwitch: ItemWithBorders
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
                    branchEnding.RefItem.ItemPartDidChanged();
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

        public DiagramItemSwitch()
        {
            conditionBox = new ItemPartSimpleRect(this, ItemPart.PRIMARY_AREA);
            OriginPoint.SetCursor(Keys.None, Cursors.SizeAll);
            OriginPoint.WantsConnect = ConnectivityWanted.EndPointWanted;

            newBranch.branchEnding = new ItemPartOutput(this, ItemPart.ENDING_POINT);
            newBranch.branchEnding.WantsConnect = ConnectivityWanted.StartPointWanted;
            newBranch.label = "<new>";

            defaultBranch.branchEnding = new ItemPartOutput(this, ItemPart.ENDING_POINT);
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

        public override void Paint(System.Drawing.Graphics g, HighlightType highType)
        {
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

        public override void ItemPartDidChanged(ItemPart part)
        {
            if (part.PartType == ItemPart.ORIGIN_POINT)
            {
                Graphics g = Delegate.GetGraphics();
                conditionBox.TopCenter = new PointF(OriginPoint.X, OriginPoint.Y + DrawProperties.p_drawingStep);
                conditionBox.Size = g.MeasureString(ConditionText, DrawProperties.fontSmallTitles);

                float maxHeight = 0;
                foreach (SwitchBranch sb in branches)
                {
                    RecalculateBranchRectangle(ref maxHeight, sb);
                }

                //Debugger.Log(0, "", "(a) maxHeight is " + maxHeight + "\n");
                RecalculateBranchRectangle(ref maxHeight, newBranch);
                //Debugger.Log(0, "", "(b) maxHeight is " + maxHeight + "\n");
                RecalculateBranchRectangle(ref maxHeight, defaultBranch);
                //Debugger.Log(0, "", "(c) maxHeight is " + maxHeight + "\n");

                RecalculateBranchEndings();

                float ox = EndPoint.X;
                float oy = EndPoint.Y;
                PointF center = GetPointOnPicoForSide(ShapeSide.Center);
//                Debugger.Log(0, "", "center is " + center.X + ", " + center.Y + "\n");
                Debugger.Log(0, "", "------------------------------------\n");
                EndPoint.X = center.X;
                EndPoint.Y = center.Y + maxHeight + 80;
                if (ox != EndPoint.X || oy != EndPoint.Y)
                {
                    if (EndPoint.MoveReferencedItemPart && EndPoint.RefItem != null)
                    {
                        if (EndPoint.RefItem.X != EndPoint.X || EndPoint.RefItem.Y != EndPoint.Y)
                        {
                            EndPoint.RefItem.Point = EndPoint.Point;
                            EndPoint.RefItem.ItemPartDidChanged();
                        }
                    }
                }
            }
        }

        private static void RecalculateBranchRectangle(ref float maxHeight, SwitchBranch sb)
        {
            float thisHeight = 0;
            sb.lastOut = sb.branchEnding.GetLastItem();
            sb.rectArea = sb.branchEnding.CalculateSubordinatesDrawingRectangle();
            thisHeight = sb.Height;
            if (thisHeight > maxHeight)
                maxHeight = thisHeight;
        }

        /// <summary>
        /// After calculation of ending points,
        /// we have to call RelayoutBranches to update
        /// layout of of branches
        /// </summary>
        private void RecalculateBranchEndings()
        {
            PointF center = GetPointOnPicoForSide(ShapeSide.Center);

            Graphics g = Delegate.GetGraphics();

            float totalWidth = 0;
            float position = 0;
            foreach (SwitchBranch sb in branches)
            {
                sb.SetY(center.Y, g);
                totalWidth += sb.Width + 16;
            }

            newBranch.SetY(center.Y, g);
            defaultBranch.SetY(center.Y, g);
            totalWidth += newBranch.Width + defaultBranch.Width + 16;

            position = center.X - totalWidth / 2;

            foreach (SwitchBranch sb in branches)
            {
                position += sb.LeftSideWidth;
                sb.SetX(position);
                position += sb.RightSideWidth + 16;
            }

            position += newBranch.LeftSideWidth;
            newBranch.SetX(position);
            position += newBranch.RightSideWidth + 16;

            position += defaultBranch.LeftSideWidth;
            defaultBranch.SetX(position);

            BorderLeft = center.X - totalWidth / 2;
            BorderRight = BorderLeft + totalWidth;

        }

        public override ItemPart GetHitItem(PointF pt)
        {
            foreach (SwitchBranch sb in branches)
            {
                if (sb.branchEnding.NearTo(pt))
                    return sb.branchEnding;
            }
            if (newBranch.branchEnding.NearTo(pt))
                return newBranch.branchEnding;
            if (defaultBranch.branchEnding.NearTo(pt))
                return defaultBranch.branchEnding;
            if (conditionBox.Contains(pt))
                return conditionBox;
            return base.GetHitItem(pt);
        }

        public override void MouseMove(ItemPart item, ItemPart startValue, SizeF diff, DiagramMouseKeys keys)
        {
            if (item.PartType == ItemPart.ORIGIN_POINT)
            {
                OriginPoint.X = (startValue as ItemPartPointF).X + diff.Width;
                OriginPoint.Y = (startValue as ItemPartPointF).Y + diff.Height;
                ItemPartDidChanged(OriginPoint);
            }
        }

        public override RectangleAnchored DrawingRectangle
        {
            get
            {
                return new RectangleAnchored(OriginPoint.X, OriginPoint.Y, OriginPoint.X - BorderLeft, BorderRight - OriginPoint.X, EndPoint.Y - OriginPoint.Y);
            }
        }

    }
}
