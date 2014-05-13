using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class DiagramItemIfCondition: ItemWithBorders
    {
        public bool UseTrueBranch { get; set; }
        public bool UseFalseBranch { get; set; }
        protected ShapeSide sideTrueBranch = ShapeSide.Left;
        protected ShapeSide sideFalseBranch = ShapeSide.Right;
        protected ItemPartOutput trueEnding = null;
        protected ItemPartOutput falseEnding = null;
        protected ItemPartSimpleRect conditionBox = null;

        public DiagramItemIfCondition()
        {
            UseFalseBranch = true;
            UseTrueBranch = true;
            conditionBox = new ItemPartSimpleRect(this, ItemPart.PRIMARY_AREA);
            OriginPoint.SetCursor(Keys.None, Cursors.SizeAll);
            OriginPoint.WantsConnect = ConnectivityWanted.EndPointWanted;
            trueEnding = new ItemPartOutput(this, ItemPart.ENDING_POINT);
            falseEnding = new ItemPartOutput(this, ItemPart.ENDING_POINT);
            trueEnding.WantsConnect = ConnectivityWanted.StartPointWanted;
            falseEnding.WantsConnect = ConnectivityWanted.StartPointWanted;
            EndPoint.WantsConnect = ConnectivityWanted.StartPointWanted;
        }


        public string ConditionText
        {
            get
            {
                return "ref == 1 || ref2 == 2";
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
            g.DrawString("if", DrawProperties.fontSmallTitles, Brushes.Black, OriginPoint.X + 6, OriginPoint.Y);
            // draw ending circles
            if (UseTrueBranch)
            {
                PointF pt1 = GetPointOnPicoForSide(sideTrueBranch);
                g.DrawLine(p1, pt1.X, pt1.Y, trueEnding.X, pt1.Y);
                g.DrawLine(p1, trueEnding.X, pt1.Y, trueEnding.X, trueEnding.Y - 3);
                g.DrawEllipse(p1, trueEnding.X - 3, trueEnding.Y - 3, 6, 6);
                SizeF sz1 = g.MeasureString("true", DrawProperties.fontSmallTitles);
                g.DrawString("true", DrawProperties.fontSmallTitles, Brushes.Black, trueEnding.X - sz1.Width,
                    trueEnding.Y - sz1.Height - 8);
            }
            if (UseFalseBranch)
            {
                PointF pt2 = GetPointOnPicoForSide(sideFalseBranch);
                g.DrawLine(p1, pt2.X, pt2.Y, falseEnding.X, pt2.Y);
                g.DrawLine(p1, falseEnding.X, pt2.Y, falseEnding.X, falseEnding.Y - 3);
                g.DrawEllipse(p1, falseEnding.X - 3, falseEnding.Y - 3, 6, 6);
                SizeF sz1 = g.MeasureString("false", DrawProperties.fontSmallTitles);
                g.DrawString("false", DrawProperties.fontSmallTitles, Brushes.Black, falseEnding.X,
                    falseEnding.Y - sz1.Height - 8);
            }
            PointF[] points = new PointF[] {
                GetPointOnPicoForSide(ShapeSide.Top),
                GetPointOnPicoForSide(ShapeSide.Right),
                GetPointOnPicoForSide(ShapeSide.Bottom),
                GetPointOnPicoForSide(ShapeSide.Left)
            };
            g.FillPolygon(b1, points);
            g.DrawPolygon(p1, points);

            ItemPartOutput last1 = trueEnding.GetLastItem();
            ItemPartOutput last2 = falseEnding.GetLastItem();

            if (last1.WantsConnect == ConnectivityWanted.StartPointWanted)
            {
                if (last1.X == EndPoint.X)
                {
                    g.DrawLine(p1, last1.X, last1.Y + 3, last1.X, EndPoint.Y - 3);
                }
                else
                {
                    g.DrawLine(p1, last1.X, last1.Y + 3, last1.X, EndPoint.Y);
                    g.DrawLine(p1, last1.X, EndPoint.Y, EndPoint.X + 3, EndPoint.Y);
                }
            }
            if (last2.WantsConnect == ConnectivityWanted.StartPointWanted)
            {
                if (last2.X == EndPoint.X)
                {
                    g.DrawLine(p1, last2.X, last2.Y + 3, last2.X, EndPoint.Y - 3);
                }
                else
                {
                    g.DrawLine(p1, last2.X, last2.Y + 3, last2.X, EndPoint.Y);
                    g.DrawLine(p1, last2.X, EndPoint.Y, EndPoint.X - 3, EndPoint.Y);
                }
            }
            g.DrawEllipse(p1, EndPoint.X - 3, EndPoint.Y - 3, 6, 6);

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

                ItemPartOutput lastTrue = trueEnding.GetLastItem();
                ItemPartOutput lastFalse = falseEnding.GetLastItem();

                RectangleAnchored rectTrue = trueEnding.CalculateSubordinatesDrawingRectangle();
                RectangleAnchored rectFalse = falseEnding.CalculateSubordinatesDrawingRectangle();

                RecalculateBranchEndings(rectTrue, rectFalse, lastTrue, lastFalse);

                RelayoutBranches();

                float ox = EndPoint.X;
                float oy = EndPoint.Y;
                PointF center = GetPointOnPicoForSide(ShapeSide.Center);
                EndPoint.X = center.X;
                EndPoint.Y = center.Y + Math.Max(rectTrue.Height, rectFalse.Height) + 48;
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

        /// <summary>
        /// After calculation of ending points,
        /// we have to call RelayoutBranches to update
        /// layout of of branches
        /// </summary>
        /// <param name="rectTrue"></param>
        /// <param name="rectFalse"></param>
        /// <param name="lastTrue"></param>
        /// <param name="lastFalse"></param>
        private void RecalculateBranchEndings(RectangleAnchored rectTrue, RectangleAnchored rectFalse, ItemPartOutput lastTrue, ItemPartOutput lastFalse)
        {
            PointF center = GetPointOnPicoForSide(ShapeSide.Center);
            falseEnding.Y = center.Y + 32;
            trueEnding.Y = center.Y + 32;

            int trueMain = 0;
            int falseMain = 0;
            int xcalctype = 0;

            if (rectFalse.Width > 0)
            {
                if (lastFalse.WantsConnect == ConnectivityWanted.StartPointWanted)
                    falseMain = 2;
                else
                    falseMain = 1;
            }
            if (rectTrue.Width > 0)
            {
                if (lastTrue.WantsConnect == ConnectivityWanted.StartPointWanted)
                    trueMain = 2;
                else
                    trueMain = 1;
            }
            if (trueMain == 2)
            {
                if (falseMain == 2)
                {
                    sideTrueBranch = ShapeSide.Right;
                    sideFalseBranch = ShapeSide.Left;
                }
                else if (falseMain == 1)
                {
                    sideTrueBranch = ShapeSide.Bottom;
                    sideFalseBranch = ShapeSide.Left;
                    xcalctype = 2;
                }
                else
                {
                    sideTrueBranch = ShapeSide.Bottom;
                    sideFalseBranch = ShapeSide.Left;
                    xcalctype = 2;
                }
            }
            else if (trueMain == 1)
            {
                if (falseMain == 2)
                {
                    sideTrueBranch = ShapeSide.Right;
                    sideFalseBranch = ShapeSide.Bottom;
                    xcalctype = 3;
                }
                else if (falseMain == 1)
                {
                    sideTrueBranch = ShapeSide.Right;
                    sideFalseBranch = ShapeSide.Left;
                }
                else
                {
                    sideTrueBranch = ShapeSide.Right;
                    sideFalseBranch = ShapeSide.Bottom;
                    xcalctype = 3;
                }
            }
            else
            {
                if (falseMain == 2)
                {
                    sideTrueBranch = ShapeSide.Right;
                    sideFalseBranch = ShapeSide.Bottom;
                    xcalctype = 3;
                }
                else if (falseMain == 1)
                {
                    sideTrueBranch = ShapeSide.Bottom;
                    sideFalseBranch = ShapeSide.Left;
                    xcalctype = 2;
                }
                else
                {
                    sideTrueBranch = ShapeSide.Right;
                    sideFalseBranch = ShapeSide.Left;
                    xcalctype = 1;
                }
            }

            if (xcalctype == 0)
            {
                float startX = OriginPoint.X - (rectTrue.LeftSideWidth + 16 + rectFalse.RightSideWidth);
                falseEnding.X = startX + rectFalse.RightSideWidth;
                trueEnding.X = startX + rectFalse.Width + 16 + rectTrue.LeftSideWidth;
            }
            else if (xcalctype == 1)
            {
                trueEnding.X = OriginPoint.X + 32;
                falseEnding.X = OriginPoint.X - 32;
            }
            else if (xcalctype == 2)
            {
                trueEnding.X = OriginPoint.X;
                falseEnding.X = trueEnding.X - (rectFalse.RightSideWidth + rectTrue.LeftSideWidth + 16);
            }
            else if (xcalctype == 3)
            {
                falseEnding.X = OriginPoint.X;
                trueEnding.X = falseEnding.X + (rectFalse.RightSideWidth + rectTrue.LeftSideWidth + 16);
            }

            BorderRight = trueEnding.X + rectTrue.RightSideWidth;
            BorderLeft = falseEnding.X - rectFalse.LeftSideWidth;

        }

        /// <summary>
        /// This function depends on previous calcualtion of ending points for branches.
        /// </summary>
        private void RelayoutBranches()
        {
            if (trueEnding.MoveReferencedItemPart && trueEnding.RefItem != null)
            {
                trueEnding.RefItem.Point = trueEnding.Point;
                trueEnding.RefItem.ItemPartDidChanged();
            }
            if (falseEnding.MoveReferencedItemPart && falseEnding.RefItem != null)
            {
                falseEnding.RefItem.Point = falseEnding.Point;
                falseEnding.RefItem.ItemPartDidChanged();
            }
        }

        public override ItemPart GetHitItem(PointF pt)
        {
            if (trueEnding.NearTo(pt))
                return trueEnding;
            if (falseEnding.NearTo(pt))
                return falseEnding;
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
