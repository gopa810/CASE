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
    public class ItemPartSimpleRect: ItemPart
    {
        public ItemPartSimpleRect()
        {
        }

        public ItemPartSimpleRect(Item it)
        {
            Item = it;
        }

        public ItemPartSimpleRect(Item it, int type)
        {
            Item = it;
            PartType = type;
        }

        public float Top
        {
            get
            {
                return TopCenter.Y;
            }
            set
            {
                _TopCenter.Y = value;
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
                _TopCenter.X = value;
            }
        }

        protected PointF _TopCenter;

        public PointF TopCenter { get { return _TopCenter; } set { _TopCenter = value; } }
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

        public bool Contains(PointF pt)
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

    }
}
