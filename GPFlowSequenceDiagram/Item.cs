using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;


namespace GPFlowSequenceDiagram
{
    public class Item
    {
        private DiagramItemDelegate pDelegate = null;
        private int p_id = 0;
        private string p_text = string.Empty;
        private string p_subtext = string.Empty;
        private object p_tag = null;
        private bool p_selected = false;


        public ItemPartInput OriginPoint = null;
        public ItemPartOutput EndPoint = null;


        public virtual void ItemPartDidChanged(ItemPart changeItem)
        {
        }

        public void DrawArrow(Graphics g, Pen p1, Brush b1, float x1, float y1, float x2, float y2)
        {
            g.DrawLine(p1, x1, y1, x2, y2);
            double len = Math.Sqrt(Convert.ToDouble((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
            if (len > 4.0)
            {
                float baseX = (float)((x2 - x1) / len) * 5;
                float baseY = (float)((y2 - y1) / len) * 5;
                float baseX2 = (float)((x2 - x1) / len) * 3;
                float baseY2 = (float)((y2 - y1) / len) * 3;
                PointF[] points = new PointF[] {
                    new PointF(x2 - baseX - baseY2, y2 - baseY + baseX2),
                    new PointF(x2, y2),
                    new PointF(x2 - baseX + baseY2, y2 - baseY - baseX2)
                };
                g.FillPolygon(b1, points);
                g.DrawPolygon(p1, points);
            }
        }

        public virtual ShapeTypeEnum CurrentShapeType
        {
            get
            {
                return ShapeTypeEnum.None;
            }
        }

        public virtual RectangleAnchored DrawingRectangle
        {
            get
            {
                return new RectangleAnchored(OriginPoint.X, OriginPoint.Y, 8, 8, EndPoint.Y - OriginPoint.Y);
            }
        }

        public Item PreviousItem
        {
            get
            {
                if (OriginPoint.RefItem != null)
                    return OriginPoint.RefItem.Item;
                return null;
            }
        }

        public string Text
        {
            get
            {
                return p_text;
            }
            set
            {
                p_text = value;
            }
        }
        public string Subtext
        {
            get { return p_subtext; }
            set { p_subtext = value; }
        }
        public int Id
        {
            get
            {
                return p_id;
            }
            set
            {
                p_id = value;
            }
        }
        public object Tag
        {
            get { return p_tag; }
            set { p_tag = value; }
        }
        public DiagramItemDelegate Delegate
        {
            get
            {
                return pDelegate;
            }
            set
            {
                pDelegate = value;
            }
        }
        public bool Selected
        {
            get { return p_selected; }
            set 
            {
                p_selected = value;
                Delegate.OnDiagramItemsCollectionChanged();
            }
        }

        public Item()
        {
            OriginPoint = new ItemPartInput(this, ItemPart.ORIGIN_POINT);
            EndPoint = new ItemPartOutput(this, ItemPart.ENDING_POINT);
        }

        public Item(RectangleF aBounds)
        {
            OriginPoint = new ItemPartInput(this, ItemPart.ORIGIN_POINT);
            EndPoint = new ItemPartOutput(this, ItemPart.ENDING_POINT); 
        }

        public Item(RectangleF aBounds, string text)
        {
            OriginPoint = new ItemPartInput(this, ItemPart.ORIGIN_POINT);
            EndPoint = new ItemPartOutput(this, ItemPart.ENDING_POINT); 
            p_text = text;
        }

        public virtual void MouseMove(ItemPart item, ItemPart startValue, SizeF diff, DiagramMouseKeys keys)
        {
        }

        public virtual void MouseUp(ItemPart itemPart)
        {
        }

        public virtual MouseState GetMouseStateFromPosition(PointerPosition pos, DiagramMouseKeys keys)
        {
            if (pos == PointerPosition.None)
                return MouseState.None;
            if (pos == PointerPosition.Inside)
                return MouseState.DiagramMove;
            return MouseState.ItemResize;
        }


        public virtual ItemPart GetHitItem(PointF pt)
        {
            if ((Math.Abs(pt.X - OriginPoint.X) + Math.Abs(pt.Y - OriginPoint.Y)) < 8)
            {
                return OriginPoint;
            }
            else if ((Math.Abs(pt.X - EndPoint.X) + Math.Abs(pt.Y - EndPoint.Y)) < 8)
            {
                return EndPoint;
            }

            return null;
        }

        public virtual Brush GetBrushForHighlight(HighlightType highType)
        {
            if (highType == HighlightType.Tracked)
            {
                return Brushes.LightGreen;
            }
            else if (highType == HighlightType.Selected)
            {
                return Brushes.LightYellow;
            }
            else if (highType == HighlightType.Normal)
            {
                return Brushes.White;
            }

            return Brushes.White;
        }

        public virtual Pen GetPenForHighlight(HighlightType highType)
        {
            if (highType == HighlightType.Tracked)
            {
                return DrawProperties.p_penHighlight;
            }
            else if (highType == HighlightType.Selected)
            {
                return DrawProperties.p_penBold;
            }
            else if (highType == HighlightType.Normal)
            {
                return DrawProperties.p_penNormal;
            }

            return DrawProperties.p_penNormal;
        }

        public virtual void Paint(Graphics g, HighlightType highType)
        {
            Pen p1 = GetPenForHighlight(highType);
            Brush b1 = GetBrushForHighlight(highType);

            g.DrawEllipse(p1, OriginPoint.X - 3, OriginPoint.Y - 3, 6, 6);
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 3, EndPoint.X, EndPoint.Y - 3);
            g.DrawEllipse(p1, EndPoint.X - 3, EndPoint.Y - 3, 6, 6);

        }

    }
}
