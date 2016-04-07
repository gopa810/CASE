using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Diagnostics;

namespace GPFlowSequenceDiagram
{
    public class DiagramDrawingContext
    {
        public Graphics Graphics { get; set; }

        private List<DiagramElement> highlightedItems = new List<DiagramElement>();
        private List<DiagramDrawingContextTransform> TransformStack = new List<DiagramDrawingContextTransform>();

        public void AddHighlighted(DiagramElement elem)
        {
            if (elem != null)
                highlightedItems.Add(elem);
        }

        public void ClearHighlighted()
        {
            highlightedItems.Clear();
        }

        public bool IsHighlighted(DiagramElement elem)
        {
            if (highlightedItems == null)
                return false;
            foreach (DiagramElement de in highlightedItems)
            {
                if (de == elem)
                    return true;
            }
            return false;
        }

        public DiagramDrawingContextTransform LastTransform
        {
            get
            {
                if (TransformStack.Count == 0)
                    return null;
                else
                    return TransformStack[TransformStack.Count - 1];
            }
        }

        public void PopTransform()
        {
            if (Graphics != null)
            {
                RemoveLastTransform();
                DiagramDrawingContextTransform gt = LastTransform;
                if (gt != null)
                    ApplyContextTransformation(gt);
            }
        }

        public void RemoveLastTransform()
        {
            if (TransformStack.Count > 0)
                TransformStack.RemoveAt(TransformStack.Count - 1);
        }

        private void ApplyContextTransformation(DiagramDrawingContextTransform gt)
        {
            //Debugger.Log(0, "", string.Format("X:{0},Y:{1},SCALE:{2}\n", gt.OffsetX, gt.OffsetY, gt.Scaling));

            Graphics.Transform = gt.MatrixPageToClient;
            //Graphics.TranslateTransform(gt.OffsetX, gt.OffsetY);
            //Graphics.ScaleTransform(gt.Scaling, gt.Scaling);
        }

        /// <summary>
        /// Set transformation by definition of source and target rectangles
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public void SetTransformation(RectangleF target, RectangleF source)
        {
            DiagramDrawingContextTransform dcf = new DiagramDrawingContextTransform(target, source);
            Graphics.Transform = dcf.MatrixPageToClient;
            TransformStack.Add(dcf);
        }

        /// <summary>
        /// Set transformation by offset and scaling
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="scale">Scaling ratio, value 1.0 means no scaling</param>
        public void SetTransformation(float offsetX, float offsetY, float scale)
        {
            DiagramDrawingContextTransform dcf = new DiagramDrawingContextTransform(offsetX, offsetY, scale);
            Graphics.Transform = dcf.MatrixPageToClient;
            TransformStack.Add(dcf);
        }

        /// <summary>
        /// Converts rectangle (in page coordinates)
        /// into rectangle (in client coordinates)
        /// </summary>
        /// <param name="relativeRect"></param>
        /// <returns></returns>
        public RectangleF LastPageRectToClient(RectangleF relativeRect)
        {
            DiagramDrawingContextTransform dcf = LastTransform;
            if (dcf == null)
                return relativeRect;
            PointF [] points = new PointF[] {
                new PointF(relativeRect.Left, relativeRect.Top),
                new PointF(relativeRect.Right, relativeRect.Bottom)
            };
            dcf.MatrixPageToClient.TransformPoints(points);

            return new RectangleF(points[0].X, points[0].Y, points[1].X - points[0].X, 
                points[1].Y - points[0].Y);
        }


    }

    /// <summary>
    /// Transformation values
    /// </summary>
    public class DiagramDrawingContextTransform
    {
        public Matrix MatrixPageToClient { get; set; }
        public Matrix MatrixClientToPage { get; set; }

        public DiagramDrawingContextTransform()
        {
        }

        public DiagramDrawingContextTransform(float offsetX, float offsetY, float scale)
        {
            MatrixPageToClient = new Matrix();
            MatrixPageToClient.Translate(offsetX, offsetY);
            MatrixPageToClient.Scale(scale, scale);

            float[] e = MatrixPageToClient.Elements;
            MatrixClientToPage = new Matrix(e[0], e[1], e[2], e[3], e[4], e[5]);
            MatrixClientToPage.Invert();
        }

        public DiagramDrawingContextTransform(RectangleF viewRect, RectangleF sourceRect)
        {
            MatrixPageToClient = new Matrix(sourceRect, new PointF[] { new PointF(viewRect.Left, viewRect.Top),
            new PointF(viewRect.Right, viewRect.Top), new PointF(viewRect.Left, viewRect.Bottom)});
            float[] e = MatrixPageToClient.Elements;
            MatrixClientToPage = new Matrix(e[0], e[1], e[2], e[3], e[4], e[5]);
            MatrixClientToPage.Invert();
        }

        public RectangleF PageToClientRect(RectangleF relativeRect)
        {
            PointF[] points = new PointF[] {
                new PointF(relativeRect.Left, relativeRect.Top),
                new PointF(relativeRect.Right, relativeRect.Bottom)
            };

            MatrixPageToClient.TransformPoints(points);

            return new RectangleF(points[0].X, points[0].Y, points[1].X - points[0].X,
                points[1].Y - points[0].Y);
        }

        public PointF PageToClientPoint(PointF relativeRect)
        {
            PointF[] points = new PointF[] { relativeRect };

            MatrixPageToClient.TransformPoints(points);

            return points[0];
        }

        public PointF ClientToPagePoint(PointF relativeRect)
        {
            PointF[] points = new PointF[] { relativeRect };

            MatrixClientToPage.TransformPoints(points);

            return points[0];
        }
    }


}
