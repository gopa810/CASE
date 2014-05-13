using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class DiagramItemForLoop: DiagramItemWhileLoop
    {
        public ItemPartRectangle initBlock = null;
        public ItemPartRectangle iterationBlock = null;

        public DiagramItemForLoop()
        {
            initBlock = new ItemPartRectangle(this);
            initBlock.Size = new SizeF(50, 30);
            iterationBlock = new ItemPartRectangle(this);
            iterationBlock.Size = new SizeF(50, 30);
        }

        public override void Paint(System.Drawing.Graphics g, HighlightType highType)
        {
            Pen p1 = GetPenForHighlight(highType);
            Brush b1 = GetBrushForHighlight(highType);

            PointF cn = OriginPoint.Point;
            PointF[] points = new PointF[] {
                GetPointOnPicoForSide(ShapeSide.Top),
                GetPointOnPicoForSide(ShapeSide.Right),
                GetPointOnPicoForSide(ShapeSide.Bottom),
                GetPointOnPicoForSide(ShapeSide.Left)
            };

            cn.Y += DrawProperties.p_drawingStep + 8;

            // draw origin circle
            g.DrawEllipse(p1, OriginPoint.X - 3, OriginPoint.Y - 3, 6, 6);

            // draw line from origin to init block
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 3, OriginPoint.X, initBlock.Top.Value);

            // draw init block
            g.DrawRectangle(p1, initBlock.Left.Value, initBlock.Top.Value,
                initBlock.Width, initBlock.Height);

            // draw line from init block to pico
            g.DrawLine(p1, OriginPoint.X, initBlock.Bottom.Value, OriginPoint.X, points[0].Y);

            // draw iteration block
            g.DrawRectangle(p1, iterationBlock.Left.Value, iterationBlock.Top.Value,
                iterationBlock.Width, iterationBlock.Height);

            // draw line from pico to true branch ending
            g.DrawLine(p1, points[2].X, points[2].Y, trueEnding.X, trueEnding.Y - 3);

            // draw ending circles
            g.DrawEllipse(p1, trueEnding.X - 3, trueEnding.Y - 3, 6, 6);

            // draw pico
            g.FillPolygon(b1, points);
            g.DrawPolygon(p1, points);

            ItemPartOutput last1 = trueEnding.GetLastItem();

            // drawing line from last item to iteration block
            g.DrawLine(p1, last1.X, last1.Y + 3, last1.X, last1.Y + 16);
            g.DrawLine(p1, last1.X, last1.Y + 16, BorderLeft, last1.Y + 16);
            g.DrawLine(p1, BorderLeft, last1.Y + 16, BorderLeft, points[3].Y);
            DrawArrow(g, p1, b1, BorderLeft, points[3].Y, iterationBlock.Left.Value, points[3].Y);

            // drawing line from iteration block to pico
            DrawArrow(g, p1, b1, iterationBlock.Right.Value, points[3].Y, points[3].X, points[3].Y);

            // drawing line from pico to exit from loop
            g.DrawLine(p1, points[1].X, points[1].Y, BorderRight, points[1].Y);
            g.DrawLine(p1, BorderRight, points[1].Y, BorderRight, EndPoint.Y - 16);
            g.DrawLine(p1, BorderRight, EndPoint.Y - 16, EndPoint.X, EndPoint.Y - 16);
            DrawArrow(g, p1, b1, EndPoint.X, EndPoint.Y - 16, EndPoint.X, EndPoint.Y - 3);

            // drawing exit circle
            g.DrawEllipse(p1, EndPoint.X - 3, EndPoint.Y - 3, 6, 6);

        }

        public override float PicoOriginDistance
        {
            get
            {
                return initBlock.Height + 2 * DrawProperties.p_drawingStep;
            }
        }

        public override void ItemPartDidChanged(ItemPart part)
        {
            if (part.PartType == ItemPart.ORIGIN_POINT)
            {
                initBlock.SetTopCenter(OriginPoint.X, OriginPoint.Y + DrawProperties.p_drawingStep);
                iterationBlock.SetRightCenter(initBlock.Left.Value - 16, initBlock.Bottom.Value + DrawProperties.p_drawingStep);

                float brdLeft = iterationBlock.Left.Value - DrawProperties.p_drawingStep;

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

                if (brdLeft < BorderLeft)
                    BorderLeft = brdLeft;
            }
        }

        public override ItemPart GetHitItem(PointF pt)
        {
            if ((Math.Abs(pt.X - trueEnding.X) + Math.Abs(pt.Y - trueEnding.Y)) < 8)
                return trueEnding;
            return base.GetHitItem(pt);
        }
    }
}
