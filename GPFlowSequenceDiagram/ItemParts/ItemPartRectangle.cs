﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class ItemPartRectangle : ItemPart
    {
        public ItemPartFloat Top = null;
        public ItemPartFloat Bottom = null;
        public ItemPartFloat Left = null;
        public ItemPartFloat Right = null;

        public ItemPartCompoundPoint TopLeft = null;
        public ItemPartCompoundPoint TopRight = null;
        public ItemPartCompoundPoint BottomLeft = null;
        public ItemPartCompoundPoint BottomRight = null;

        public ItemPartCompoundRectangle PartArea = null;

        private void InitializeMembers()
        {
            Top = new ItemPartFloat(Item, ItemPart.TOP_BORDER);
            Top.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeNS);
            Bottom = new ItemPartFloat(Item, ItemPart.BOTTOM_BORDER);
            Bottom.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeNS);
            Left = new ItemPartFloat(Item, ItemPart.LEFT_BORDER);
            Left.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeWE);
            Right = new ItemPartFloat(Item, ItemPart.RIGHT_BORDER);
            Right.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeWE);

            TopLeft = new ItemPartCompoundPoint(Item, ItemPart.TOPLEFT_CORNER, Left, Top);
            TopLeft.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeNWSE);
            TopRight = new ItemPartCompoundPoint(Item, ItemPart.TOPRIGHT_CORNER, Right, Top);
            TopRight.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeNESW);
            BottomLeft = new ItemPartCompoundPoint(Item, ItemPart.BOTTOMLEFT_CORNER, Left, Bottom);
            BottomLeft.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeNESW);
            BottomRight = new ItemPartCompoundPoint(Item, ItemPart.BOTTOMRIGHT_CORNER, Right, Bottom);
            BottomRight.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeNWSE);

            PartArea = new ItemPartCompoundRectangle(Item, ItemPart.PRIMARY_AREA, Left, Top, Right, Bottom);

        }

        public ItemPartRectangle(Item it)
        {
            Item = it;
            InitializeMembers();
        }

        public SizeF Size
        {
            get
            {
                return PartArea.Bounds.Size;
            }
            set
            {
                PointF cp = CenterPoint;
                float w = value.Width / 2;
                float h = value.Height / 2;

                Left.Value = cp.X - w;
                Right.Value = cp.X + w;
                Top.Value = cp.Y - h;
                Bottom.Value = cp.Y + h;
            }
        }

        public float Width
        {
            get
            {
                return Right.Value - Left.Value;
            }
        }
        public float Height
        {
            get
            {
                return Bottom.Value - Top.Value;
            }
        }

        public PointF CenterPoint
        {
            get
            {
                return new PointF((Left.Value + Right.Value) / 2,
                    (Bottom.Value + Top.Value) / 2);
            }
            set
            {
                float w = Width / 2;
                float h = Height / 2;
                Left.Value = value.X - w;
                Right.Value = value.X + w;
                Top.Value = value.Y - h;
                Bottom.Value = value.Y + h;
            }
        }

        public RectangleF Bounds
        {
            get
            {
                return PartArea.Bounds;
            }
        }

        public virtual ItemPart GetHitItem(PointF pt)
        {
            ItemPart part = null;
            if ((pt.X >= Left.Value - 3) && (pt.X <= Right.Value + 3)
                && (pt.Y >= Top.Value - 3) && (pt.Y <= Bottom.Value + 3))
            {
                part = PartArea;
                if (pt.X <= Left.Value + 3)
                {
                    if (pt.Y <= Top.Value + 3)
                    {
                        part = TopLeft;
                    }
                    else if (pt.Y >= Bottom.Value - 3)
                    {
                        part = BottomLeft;
                    }
                    else
                    {
                        part = Left;
                    }
                }
                else if (pt.X >= Right.Value - 3)
                {
                    if (pt.Y <= Top.Value + 3)
                    {
                        part = TopRight;
                    }
                    else if (pt.Y >= Bottom.Value - 3)
                    {
                        part = BottomRight;
                    }
                    else
                    {
                        part = Right;
                    }
                }
                else if (pt.Y <= Top.Value + 3)
                {
                    part = Top;
                }
                else if (pt.Y >= Bottom.Value - 3)
                {
                    part = Bottom;
                }
            }

            return part;
        }

        public virtual void MouseMove(ItemPart item, ItemPart startValue, SizeF diff, DiagramMouseKeys keys)
        {
            if (item == Left)
            {
                if (startValue is ItemPartFloat)
                {
                    PointF OriginPoint = new PointF((Left.Value + Right.Value) / 2,
                                (Top.Value + Bottom.Value) / 2);
                    Left.Value = (startValue as ItemPartFloat).Value + diff.Width;
                    Right.Value = Left.Value + 2 * (OriginPoint.X - Left.Value);
                }
            }
            else if (item == Right)
            {
                if (startValue is ItemPartFloat)
                {
                    PointF OriginPoint = new PointF((Left.Value + Right.Value) / 2,
                                (Top.Value + Bottom.Value) / 2);
                    Right.Value = (startValue as ItemPartFloat).Value + diff.Width;
                    Left.Value = Right.Value - 2 * (Right.Value - OriginPoint.X);
                }
            }
            else if (item == Top)
            {
                if (startValue is ItemPartFloat)
                {
                    PointF OriginPoint = new PointF((Left.Value + Right.Value) / 2,
                                (Top.Value + Bottom.Value) / 2);
                    Top.Value = (startValue as ItemPartFloat).Value + diff.Height;
                    Bottom.Value = Top.Value + 2 * (OriginPoint.Y - Top.Value);
                }
            }
            else if (item == Bottom)
            {
                if (startValue is ItemPartFloat)
                {
                    PointF OriginPoint = new PointF((Left.Value + Right.Value) / 2,
                                (Top.Value + Bottom.Value) / 2);
                    Bottom.Value = (startValue as ItemPartFloat).Value + diff.Height;
                    Top.Value = Bottom.Value - 2 * (Bottom.Value - OriginPoint.Y);
                }
            }
            else if (item == BottomLeft)
            {
                if (startValue is ItemPartPointF)
                {
                    PointF OriginPoint = new PointF((Left.Value + Right.Value) / 2,
                                (Top.Value + Bottom.Value) / 2);
                    Left.Value = (startValue as ItemPartFloat).Value + diff.Width;
                    Bottom.Value = (startValue as ItemPartFloat).Value + diff.Height;
                    Right.Value = Left.Value + 2 * (OriginPoint.X - Left.Value);
                    Top.Value = Bottom.Value - 2 * (Bottom.Value - OriginPoint.Y);
                }
            }
            else if (item == BottomRight)
            {
                if (startValue is ItemPartPointF)
                {
                    PointF OriginPoint = new PointF((Left.Value + Right.Value) / 2,
                                (Top.Value + Bottom.Value) / 2);
                    Right.Value = (startValue as ItemPartFloat).Value + diff.Width;
                    Bottom.Value = (startValue as ItemPartFloat).Value + diff.Height;
                    Left.Value = Right.Value - 2 * (Right.Value - OriginPoint.X);
                    Top.Value = Bottom.Value - 2 * (Bottom.Value - OriginPoint.Y);
                }
            }
            else if (item == TopLeft)
            {
                if (startValue is ItemPartPointF)
                {
                    PointF OriginPoint = new PointF((Left.Value + Right.Value) / 2,
                                (Top.Value + Bottom.Value) / 2);
                    Top.Value = (startValue as ItemPartFloat).Value + diff.Height;
                    Left.Value = (startValue as ItemPartFloat).Value + diff.Width;
                    Right.Value = Left.Value + 2 * (OriginPoint.X - Left.Value);
                    Bottom.Value = Top.Value + 2 * (OriginPoint.Y - Top.Value);
                }
            }
            else if (item == TopRight)
            {
                if (startValue is ItemPartPointF)
                {
                    PointF OriginPoint = new PointF((Left.Value + Right.Value) / 2,
                                (Top.Value + Bottom.Value) / 2);
                    Top.Value = (startValue as ItemPartFloat).Value + diff.Height;
                    Right.Value = (startValue as ItemPartFloat).Value + diff.Width;
                    Left.Value = Right.Value - 2 * (Right.Value - OriginPoint.X);
                    Bottom.Value = Top.Value + 2 * (OriginPoint.Y - Top.Value);
                }
            }
        }



        public bool IsMember(ItemPart itemPart)
        {
            return (itemPart == PartArea || itemPart == Bottom
                || itemPart == BottomLeft || itemPart == BottomRight
                || itemPart == Left || itemPart == Right
                || itemPart == Top || itemPart == TopLeft
                || itemPart == TopRight);
        }

        public void SetTopCenter(float x, float y)
        {
            SizeF sz = Size;

            Left.Value = x - sz.Width / 2;
            Right.Value = Left.Value + sz.Width;
            Top.Value = y;
            Bottom.Value = y + sz.Height;
        }

        public void SetBottomCenter(float x, float y)
        {
            SizeF sz = Size;

            Left.Value = x - sz.Width / 2;
            Right.Value = Left.Value + sz.Width;
            Top.Value = y - sz.Height;
            Bottom.Value = y;
        }

        public void SetRightCenter(float x, float y)
        {
            SizeF sz = Size;

            Left.Value = x - sz.Width;
            Right.Value = x;
            Top.Value = y - sz.Height / 2;
            Bottom.Value = Top.Value + sz.Height;
        }

        public void SetLeftCenter(float x, float y)
        {
            SizeF sz = Size;

            Left.Value = x;
            Right.Value = x + sz.Width;
            Top.Value = y - sz.Height / 2;
            Bottom.Value = Top.Value + sz.Height;
        }
    }
}
