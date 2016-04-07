using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPFlowSequenceDiagram.ItemParts
{
    public struct ItemPadding
    {
        public float Top;
        public float Bottom;
        public float Left;
        public float Right;

        public ItemPadding(float all)
        {
            Top = all;
            Bottom = all;
            Left = all;
            Right = all;
        }

        public ItemPadding(float horizontal, float vertical)
        {
            Top = horizontal;
            Bottom = horizontal;
            Left = vertical;
            Right = vertical;
        }

        public float All
        {
            get
            {
                if (Top == Bottom && Bottom == Left && Left == Right)
                    return Top;
                else
                    return -1;
            }
            set
            {
                Top = value;
                Bottom = value;
                Left = value;
                Right = value;
            }
        }

        public float Vertical
        {
            get
            {
                if (Left == Right)
                    return Left;
                else
                    return -1;
            }
            set
            {
                Left = value;
                Right = value;
            }
        }

        public float Horizontal
        {
            get
            {
                if (Top == Bottom)
                    return Top;
                else
                    return -1;
            }
            set
            {
                Top = value;
                Bottom = value;
            }
        }
    }
}
