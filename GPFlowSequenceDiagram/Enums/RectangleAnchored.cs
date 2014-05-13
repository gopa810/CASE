using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPFlowSequenceDiagram
{
    public class RectangleAnchored
    {
        public float OriginX = 0;
        public float OriginY = 0;
        public float LeftSideWidth = 0;
        public float RightSideWidth = 0;
        public float Height = 0;

        public RectangleAnchored()
        {
        }

        public RectangleAnchored(float origX, float origY, float leftSide, float rightSide, float height)
        {
            OriginX = origX;
            OriginY = origY;
            LeftSideWidth = leftSide;
            RightSideWidth = rightSide;
            Height = height;
        }

        public float Width
        {
            get
            {
                return LeftSideWidth + RightSideWidth;
            }
        }

        public float Left
        {
            get 
            {
                return OriginX - LeftSideWidth;
            }
        }

        public float Right
        {
            get
            {
                return OriginX + RightSideWidth;
            }
        }

        public float Top
        {
            get
            {
                return OriginY;
            }
        }

        public float Bottom
        {
            get
            {
                return OriginY + Height;
            }
        }

        public override string ToString()
        {
            return string.Format("{0},{1} [ {2};{3};{4} ]", OriginX, OriginY, LeftSideWidth, RightSideWidth, Height);
        }

        public void MergeRectangles(RectangleAnchored addedRect)
        {
            RectangleAnchored mainRect = this;
            float ox, oy, b, r, l;

            if (mainRect.Top <= addedRect.Top)
            {
                ox = mainRect.OriginX;
                oy = mainRect.OriginY;
            }
            else
            {
                ox = addedRect.OriginX;
                oy = addedRect.OriginY;
            }

            b = Math.Max(mainRect.Bottom, addedRect.Bottom) - oy;
            r = Math.Max(mainRect.Right, addedRect.Right) - ox;
            l = ox - Math.Min(mainRect.Left, addedRect.Left);
            mainRect.OriginX = ox;
            mainRect.OriginY = oy;
            mainRect.LeftSideWidth = l;
            mainRect.RightSideWidth = r;
            mainRect.Height = b;
        }


    }
}
