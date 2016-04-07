using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GPFlowSequenceDiagram
{
    /// <summary>
    /// This part of item acts as rectangle with main anchor point on top line in the center.
    /// That means we can set these values:
    ///  - Top
    ///  - CenterY
    ///  - Width
    ///  - Height
    /// </summary>
    public class ItemPartSimpleRect: DiagramItemPart
    {
        public ItemPartSimpleRect(): base(null)
        {
        }

        public ItemPartSimpleRect(DiagramItem it): base(it)
        {
        }

        public ItemPartSimpleRect(DiagramItem it, int type): base(it)
        {
            ElementType = type;
        }

        public float Top
        {
            get
            {
                return TopCenter.Y;
            }
            set
            {
                TopCenter = new PointF(TopCenter.X, value);
            }
        }

        public float CenterX
        {
            get
            {
                return TopCenter.X;
            }
            set
            {
                TopCenter = new PointF(value, TopCenter.Y);
            }
        }

        public PointF TopCenter { get; set; }
        public SizeF Size { get; set; }

        public float Bottom
        {
            get
            {
                return Top + Size.Height;
            }
        }

        public float Left
        {
            get { return CenterX - Size.Width / 2; }
        }

        public float Right
        {
            get { return CenterX + Size.Width / 2; }
        }

        public float Width
        {
            get { return Size.Width; }
        }

        public float Height
        {
            get { return Size.Height; }
        }

        public bool Contains(DiagramPoint pt)
        {
            return (pt.X >= Left && pt.X <= Right && pt.Y >= Top && pt.Y <= Bottom);
        }

        public float LeftSideWidth
        {
            get { return Width / 2;  }
        }

        public float RightSideWidth
        {
            get { return Width / 2; }
        }


        public void SetRectangle(RectangleF box)
        {
            TopCenter = new PointF((box.Left + box.Right) / 2, box.Top);
            Size = box.Size;
        }
    }
}
