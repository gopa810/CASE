using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class ItemRectangleArea: Item
    {
        public ItemPartFloat PartTopBorder = null;
        public ItemPartFloat PartBottomBorder = null;
        public ItemPartFloat PartLeftBorder = null;
        public ItemPartFloat PartRightBorder = null;

        public ItemPartCompoundPoint PartTopLeftCorner = null;
        public ItemPartCompoundPoint PartTopRightCorner = null;
        public ItemPartCompoundPoint PartBottomLeftCorner = null;
        public ItemPartCompoundPoint PartBottomRightCorner = null;

        public ItemPartCompoundRectangle PartArea = null;

        public ItemRectangleArea(): base()
        {
            OriginPoint.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeAll);
            OriginPoint.WantsConnect = ConnectivityWanted.EndPointWanted;
            EndPoint.WantsConnect = ConnectivityWanted.StartPointWanted;

            PartTopBorder = new ItemPartFloat(this, ItemPart.TOP_BORDER);
            PartTopBorder.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeNS);
            PartBottomBorder = new ItemPartFloat(this, ItemPart.BOTTOM_BORDER);
            PartBottomBorder.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeNS);
            PartLeftBorder = new ItemPartFloat(this, ItemPart.LEFT_BORDER);
            PartLeftBorder.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeWE);
            PartRightBorder = new ItemPartFloat(this, ItemPart.RIGHT_BORDER);
            PartRightBorder.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeWE);

            PartTopLeftCorner = new ItemPartCompoundPoint(this, ItemPart.TOPLEFT_CORNER, PartLeftBorder, PartTopBorder);
            PartTopLeftCorner.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeNWSE);
            PartTopRightCorner = new ItemPartCompoundPoint(this, ItemPart.TOPRIGHT_CORNER, PartRightBorder, PartTopBorder);
            PartTopRightCorner.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeNESW);
            PartBottomLeftCorner = new ItemPartCompoundPoint(this, ItemPart.BOTTOMLEFT_CORNER, PartLeftBorder, PartBottomBorder);
            PartBottomLeftCorner.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeNESW);
            PartBottomRightCorner = new ItemPartCompoundPoint(this, ItemPart.BOTTOMRIGHT_CORNER, PartRightBorder, PartBottomBorder);
            PartBottomRightCorner.SetCursor(System.Windows.Forms.Keys.None, System.Windows.Forms.Cursors.SizeNWSE);

            PartArea = new ItemPartCompoundRectangle(this, ItemPart.PRIMARY_AREA, PartLeftBorder, PartTopBorder, PartRightBorder, PartBottomBorder);
        }

        public override void ItemPartDidChanged(ItemPart part)
        {
            switch(part.PartType)
            {
                case ItemPart.ORIGIN_POINT:
                    {
                        float h = Height;
                        float w = Width;
                        PartTopBorder.Value = OriginPoint.Y + 16;
                        PartBottomBorder.Value = PartTopBorder.Value + h;
                        PartLeftBorder.Value = OriginPoint.X - w / 2;
                        PartRightBorder.Value = PartLeftBorder.Value + w;
                        PointF newEndPoint = new PointF(OriginPoint.X, PartBottomBorder.Value + 16);
                        SizeF newDiff = new SizeF(newEndPoint.X - EndPoint.X, newEndPoint.Y - EndPoint.Y);
                        EndPoint.X = OriginPoint.X;
                        EndPoint.Y = PartBottomBorder.Value + 16;
                        if (EndPoint.MoveReferencedItemPart && EndPoint.RefItem != null)
                        {
                            EndPoint.RefItem.Point += newDiff;
                            EndPoint.RefItem.Item.ItemPartDidChanged(EndPoint.RefItem);
                        }
                    }
                    break;
                case ItemPart.LEFT_BORDER:
                    PartRightBorder.Value = PartLeftBorder.Value + 2 * (OriginPoint.X - PartLeftBorder.Value);
                    break;
                case ItemPart.RIGHT_BORDER:
                    PartLeftBorder.Value = PartRightBorder.Value - 2 * (PartRightBorder.Value - OriginPoint.X);
                    break;
                case ItemPart.TOP_BORDER:
                    PartBottomBorder.Value = PartTopBorder.Value + 2 * (OriginPoint.Y - PartTopBorder.Value);
                    break;
                case ItemPart.BOTTOM_BORDER:
                    PartTopBorder.Value = PartBottomBorder.Value - 2 * (PartBottomBorder.Value - OriginPoint.Y);
                    break;
            }
        }

        public override void MouseMove(ItemPart item, ItemPart startValue, SizeF diff, DiagramMouseKeys keys)
        {
            switch (item.PartType)
            {
                case ItemPart.ORIGIN_POINT:
                    if (startValue is ItemPartPointF)
                    {
                        ItemPartPointF ptx = startValue as ItemPartPointF;
                        OriginPoint.X = ptx.X + diff.Width;
                        OriginPoint.Y = ptx.Y + diff.Height;
                    }
                    ItemPartDidChanged(OriginPoint);
                    break;
                case ItemPart.LEFT_BORDER:
                    if (startValue is ItemPartFloat)
                    {
                        PartLeftBorder.Value = (startValue as ItemPartFloat).Value + diff.Width;
                    }
                    ItemPartDidChanged(PartLeftBorder);
                    break;
                case ItemPart.RIGHT_BORDER:
                    if (startValue is ItemPartFloat)
                    {
                        PartRightBorder.Value = (startValue as ItemPartFloat).Value + diff.Width;
                    }
                    ItemPartDidChanged(PartRightBorder);
                    break;
                case ItemPart.TOP_BORDER:
                    if (startValue is ItemPartFloat)
                    {
                        PartTopBorder.Value = (startValue as ItemPartFloat).Value + diff.Height;
                    }
                    ItemPartDidChanged(PartTopBorder);
                    break;
                case ItemPart.BOTTOM_BORDER:
                    if (startValue is ItemPartFloat)
                    {
                        PartBottomBorder.Value = (startValue as ItemPartFloat).Value + diff.Height;
                    }
                    ItemPartDidChanged(PartBottomBorder);
                    break;
                case ItemPart.BOTTOMLEFT_CORNER:
                    if (startValue is ItemPartPointF)
                    {
                        PartLeftBorder.Value = (startValue as ItemPartFloat).Value + diff.Width;
                        PartBottomBorder.Value = (startValue as ItemPartFloat).Value + diff.Height;
                    }
                    ItemPartDidChanged(PartBottomBorder);
                    ItemPartDidChanged(PartLeftBorder);
                    break;
                case ItemPart.BOTTOMRIGHT_CORNER:
                    if (startValue is ItemPartPointF)
                    {
                        PartRightBorder.Value = (startValue as ItemPartFloat).Value + diff.Width;
                        PartBottomBorder.Value = (startValue as ItemPartFloat).Value + diff.Height;
                    }
                    ItemPartDidChanged(PartBottomBorder);
                    ItemPartDidChanged(PartRightBorder);
                    break;
                case ItemPart.TOPLEFT_CORNER:
                    if (startValue is ItemPartPointF)
                    {
                        PartTopBorder.Value = (startValue as ItemPartFloat).Value + diff.Height;
                        PartLeftBorder.Value = (startValue as ItemPartFloat).Value + diff.Width;
                    }
                    ItemPartDidChanged(PartTopBorder);
                    ItemPartDidChanged(PartLeftBorder);
                    break;
                case ItemPart.TOPRIGHT_CORNER:
                    if (startValue is ItemPartPointF)
                    {
                        PartTopBorder.Value = (startValue as ItemPartFloat).Value + diff.Height;
                        PartRightBorder.Value = (startValue as ItemPartFloat).Value + diff.Width;
                    }
                    ItemPartDidChanged(PartTopBorder);
                    ItemPartDidChanged(PartRightBorder);
                    break;
            }
        }

        public override void MouseUp(ItemPart itemPart)
        {
            switch (itemPart.PartType)
            {
                case ItemPart.LEFT_BORDER:
                case ItemPart.TOPLEFT_CORNER:
                case ItemPart.TOP_BORDER:
                case ItemPart.TOPRIGHT_CORNER:
                case ItemPart.RIGHT_BORDER:
                case ItemPart.BOTTOMRIGHT_CORNER:
                case ItemPart.BOTTOM_BORDER:
                case ItemPart.BOTTOMLEFT_CORNER:
                    ItemPartDidChanged(OriginPoint);
                    break;
            }
        }

        public override ShapeTypeEnum CurrentShapeType
        {
            get
            {
                return ShapeTypeEnum.Rectangle;
            }
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

                PartLeftBorder.Value = cp.X - w;
                PartRightBorder.Value = cp.X + w;
                PartTopBorder.Value = cp.Y - h;
                PartBottomBorder.Value = cp.Y + h;
            }
        }

        public float Width
        {
            get
            {
                return PartRightBorder.Value - PartLeftBorder.Value;
            }
        }
        public float Height
        {
            get
            {
                return PartBottomBorder.Value - PartTopBorder.Value;
            }
        }

        public PointF CenterPoint
        {
            get
            {
                return new PointF((PartLeftBorder.Value + PartRightBorder.Value) / 2, 
                    (PartBottomBorder.Value + PartTopBorder.Value) / 2);
            }
            set
            {
                float w = Width / 2;
                float h = Height / 2;
                PartLeftBorder.Value = value.X - w;
                PartRightBorder.Value = value.X + w;
                PartTopBorder.Value = value.Y - h;
                PartBottomBorder.Value = value.Y + h;
            }
        }

        public RectangleF Bounds
        {
            get
            {
                return PartArea.Bounds;
            }
        }

        public override ItemPart GetHitItem(PointF pt)
        {
            ItemPart part = base.GetHitItem(pt);
            if (part == null)
            {
                if ((pt.X >= PartLeftBorder.Value - 3) && (pt.X <= PartRightBorder.Value + 3)
                    && (pt.Y >= PartTopBorder.Value - 3) && (pt.Y <= PartBottomBorder.Value + 3))
                {
                    part = PartArea;
                    if (pt.X <= PartLeftBorder.Value + 3)
                    {
                        if (pt.Y <= PartTopBorder.Value + 3)
                        {
                            part = PartTopLeftCorner;
                        }
                        else if (pt.Y >= PartBottomBorder.Value - 3)
                        {
                            part = PartBottomLeftCorner;
                        }
                        else
                        {
                            part = PartLeftBorder;
                        }
                    }
                    else if (pt.X >= PartRightBorder.Value - 3)
                    {
                        if (pt.Y <= PartTopBorder.Value + 3)
                        {
                            part = PartTopRightCorner;
                        }
                        else if (pt.Y >= PartBottomBorder.Value - 3)
                        {
                            part = PartBottomRightCorner;
                        }
                        else
                        {
                            part = PartRightBorder;
                        }
                    }
                    else if (pt.Y <= PartTopBorder.Value + 3)
                    {
                        part = PartTopBorder;
                    }
                    else if (pt.Y >= PartBottomBorder.Value - 3)
                    {
                        part = PartBottomBorder;
                    }
                }
            }

            return part;
        }

        public override RectangleAnchored DrawingRectangle
        {
            get
            {
                return new RectangleAnchored(OriginPoint.X, OriginPoint.Y, OriginPoint.X - PartLeftBorder.Value, PartRightBorder.Value - OriginPoint.X, EndPoint.Y - OriginPoint.Y);
            }
        }
    }
}
