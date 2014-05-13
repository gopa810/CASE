using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class DiagramItemForeach : ItemWithLoop
    {
        public ItemPartOutput trueEnding = null;

        public DiagramItemForeach()
        {
            OriginPoint.SetCursor(Keys.None, Cursors.SizeAll);
            OriginPoint.WantsConnect = ConnectivityWanted.EndPointWanted;
            trueEnding = new ItemPartOutput(this, ItemPart.ENDING_POINT);
            trueEnding.WantsConnect = ConnectivityWanted.StartPointWanted;
            EndPoint.WantsConnect = ConnectivityWanted.StartPointWanted;
        }

        public override void Paint(System.Drawing.Graphics g, HighlightType highType)
        {
            Pen p1 = GetPenForHighlight(highType);
            Brush b1 = GetBrushForHighlight(highType);

            PointF cn = OriginPoint.Point;

            cn.Y += DrawProperties.p_drawingStep + 8;

            // draw origin circle
            g.DrawEllipse(p1, OriginPoint.X - 3, OriginPoint.Y - 3, 6, 6);
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 3, cn.X, cn.Y);
            // draw ending circles
            PointF ptTop = GetPointOnPicoForSide(ShapeSide.Top);
            PointF ptRight = GetPointOnPicoForSide(ShapeSide.Right);
            PointF ptLeft = GetPointOnPicoForSide(ShapeSide.Left);
            PointF ptTopLeft = new PointF(ptLeft.X, ptTop.Y);
            PointF ptTopRight = new PointF(ptRight.X, ptTop.Y);
            PointF[] points = new PointF[] {
                ptTopRight,
                ptRight,
                GetPointOnPicoForSide(ShapeSide.Bottom),
                ptLeft,
                ptTopLeft,
            };
            PointF pt1 = points[2];
            g.DrawLine(p1, pt1.X, pt1.Y, trueEnding.X, pt1.Y);
            g.DrawLine(p1, trueEnding.X, pt1.Y, trueEnding.X, trueEnding.Y - 3);
            g.DrawEllipse(p1, trueEnding.X - 3, trueEnding.Y - 3, 6, 6);

            g.FillPolygon(b1, points);
            g.DrawPolygon(p1, points);

            ItemPartOutput last1 = trueEnding.GetLastItem();

            // drawing line from last item to pico
            g.DrawLine(p1, last1.X, last1.Y + 3, last1.X, last1.Y + 16);
            g.DrawLine(p1, last1.X, last1.Y + 16, BorderLeft, last1.Y + 16);
            g.DrawLine(p1, BorderLeft, last1.Y + 16, BorderLeft, points[3].Y);
            DrawArrow(g, p1, b1, BorderLeft, points[3].Y, points[3].X, points[3].Y);

            // drawing line from pico to exit from loop
            g.DrawLine(p1, points[1].X, points[1].Y, BorderRight, points[1].Y);
            g.DrawLine(p1, BorderRight, points[1].Y, BorderRight, EndPoint.Y - 16);
            g.DrawLine(p1, BorderRight, EndPoint.Y - 16, EndPoint.X, EndPoint.Y - 16);
            DrawArrow(g, p1, b1, EndPoint.X, EndPoint.Y - 16, EndPoint.X, EndPoint.Y - 3);

            // drawing exit circle
            g.DrawEllipse(p1, EndPoint.X - 3, EndPoint.Y - 3, 6, 6);

        }

        public virtual float PicoOriginDistance
        {
            get
            {
                return DrawProperties.p_drawingStep;
            }
        }

        public PointF GetPointOnPicoForSide(ShapeSide side)
        {
            float cnX = OriginPoint.X;
            float cnY = OriginPoint.Y + PicoOriginDistance;

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
                ItemPartOutput lastTrue = trueEnding.GetLastItem();

                RectangleAnchored rectTrue = trueEnding.CalculateSubordinatesDrawingRectangle();

                RecalculateBranchEndings(rectTrue, lastTrue);

                RelayoutBranches();

                EndPoint.X = OriginPoint.X;
                EndPoint.Y = OriginPoint.Y + PicoOriginDistance + 32 + rectTrue.Height + 48;
                if (EndPoint.MoveReferencedItemPart && EndPoint.RefItem != null)
                {
                    EndPoint.RefItem.Point = EndPoint.Point;
                    EndPoint.RefItem.ItemPartDidChanged();
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
        protected void RecalculateBranchEndings(RectangleAnchored rectTrue, ItemPartOutput lastTrue)
        {
            trueEnding.Y = OriginPoint.Y + PicoOriginDistance + 32;
            trueEnding.X = OriginPoint.X;

            BorderRight = trueEnding.X + rectTrue.Width / 2 + 16;
            BorderLeft = trueEnding.X - rectTrue.Width / 2 - 16;
        }

        /// <summary>
        /// This function depends on previous calcualtion of ending points for branches.
        /// </summary>
        protected void RelayoutBranches()
        {
            if (trueEnding.MoveReferencedItemPart && trueEnding.RefItem != null)
            {
                trueEnding.RefItem.Point = trueEnding.Point;
                trueEnding.RefItem.ItemPartDidChanged();
            }
        }

        public override ItemPart GetHitItem(PointF pt)
        {
            if ((Math.Abs(pt.X - trueEnding.X) + Math.Abs(pt.Y - trueEnding.Y)) < 8)
                return trueEnding;
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
