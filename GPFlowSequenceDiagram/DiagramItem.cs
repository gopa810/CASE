using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;

namespace GPFlowSequenceDiagram
{
    public class DiagramItem: DiagramElement
    {
        private int p_id = 0;
        private string p_text = string.Empty;
        private string p_subtext = string.Empty;
        private object p_tag = null;
        private bool p_selected = false;


        public ItemPartInput OriginPoint = null;
        public ItemPartOutput EndPoint = null;


        public DiagramItem(DiagramElement parent)
            : base(parent)
        {
            ElementType = ET_GENERAL_OBJECT;
            OriginPoint = new ItemPartInput(this, DiagramItemPart.ET_ORIGIN_POINT);
            EndPoint = new ItemPartOutput(this, DiagramItemPart.ET_ENDING_POINT);
        }

        public DiagramItem(DiagramElement parent, RectangleF aBounds)
            : base(parent)
        {
            ElementType = ET_GENERAL_OBJECT;
            OriginPoint = new ItemPartInput(this, DiagramItemPart.ET_ORIGIN_POINT);
            EndPoint = new ItemPartOutput(this, DiagramItemPart.ET_ENDING_POINT);
        }

        public DiagramItem(DiagramElement parent, RectangleF aBounds, string text)
            : base(parent)
        {
            ElementType = ET_GENERAL_OBJECT;
            OriginPoint = new ItemPartInput(this, DiagramItemPart.ET_ORIGIN_POINT);
            EndPoint = new ItemPartOutput(this, DiagramItemPart.ET_ENDING_POINT);
            p_text = text;
        }

        public override string ToString()
        {
            return string.Format("Item {0}", ElementId);
        }

        /// <summary>
        /// Adds subitem to given item. 
        /// </summary>
        /// <param name="newItem">New item to be added</param>
        /// <param name="diagramLocation">DiagramLocation is location within the whole diagram.
        /// Most probably they needs to be converted into relative coordinates relative
        /// to parent items' original point.</param>
        public virtual void AddSubitem(DiagramItem newItem, DiagramPoint diagramLocation)
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

        [Browsable(false)]
        public DiagramItem PreviousItem
        {
            get
            {
                if (OriginPoint.RefItem != null)
                    return OriginPoint.RefItem.Item;
                return null;
            }
        }

        [Category("Descriptive")]
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

        [Category("Descriptive")]
        public string Subtext
        {
            get { return p_subtext; }
            set { p_subtext = value; }
        }

        [ReadOnly(true)]
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

        [Browsable(false)]
        public object Tag
        {
            get { return p_tag; }
            set { p_tag = value; }
        }

        [ReadOnly(true)]
        public bool Selected
        {
            get { return p_selected; }
            set 
            {
                p_selected = value;
                if (Parent is DiagramItemCollection)
                {
                    ((DiagramItemCollection)Parent).DE_OnItemSelected(this);
                }
            }
        }

        public virtual void MouseMove(DiagramItemPart item, DiagramItemPart startValue, SizeF diff, DiagramMouseKeys keys)
        {
        }

        public virtual void MouseUp(DiagramItemPart itemPart)
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


        public override void DE_FindElements(DiagramContext context)
        {
            DiagramPoint pt = context.PagePoint;
            if (context.FoundElement != null)
                return;

            if ((Math.Abs(pt.X - OriginPoint.X) + Math.Abs(pt.Y - OriginPoint.Y)) < 8)
            {
                context.InsertElement(OriginPoint);
            }
            if (context.FoundElement != null)
                return;

            if ((Math.Abs(pt.X - EndPoint.X) + Math.Abs(pt.Y - EndPoint.Y)) < 8)
            {
                context.InsertElement(EndPoint);
            }
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

        public virtual Pen GetDashPenForHighlight(HighlightType highType)
        {
            if (highType == HighlightType.Tracked)
            {
                return DrawProperties.p_penHighlightDash;
            }
            else if (highType == HighlightType.Selected)
            {
                return DrawProperties.p_penBoldDash;
            }
            else if (highType == HighlightType.Normal)
            {
                return DrawProperties.p_penNormalDash;
            }

            return DrawProperties.p_penNormalDash;
        }

        public override SizeF DE_DrawShape(DiagramDrawingContext ctx, HighlightType highType)
        {
            Graphics g = ctx.Graphics;

            Pen p1 = GetPenForHighlight(highType);
            Brush b1 = GetBrushForHighlight(highType);

            g.DrawEllipse(p1, OriginPoint.X - 3, OriginPoint.Y - 3, 6, 6);
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 3, EndPoint.X, EndPoint.Y - 3);
            g.DrawEllipse(p1, EndPoint.X - 3, EndPoint.Y - 3, 6, 6);

            return new SizeF(32, EndPoint.Y - OriginPoint.Y);
        }

        public virtual void SetOriginPoint(DiagramDrawingContext ctx, float x, float y)
        {
            if (x != OriginPoint.X || y != OriginPoint.Y)
            {
                OriginPoint.X = x;
                OriginPoint.Y = y;
                RelayoutNextShapes(ctx);
            }
        }

        public virtual void RelayoutNextShapes(DiagramDrawingContext ctx)
        {
            DE_DrawShape(ctx, HighlightType.NotDraw);

            if (EndPoint.RefItem != null && EndPoint.RefItem.Item != null)
            {
                EndPoint.RefItem.Item.SetOriginPoint(ctx, EndPoint.X, EndPoint.Y);
            }
        }

        public virtual DiagramPage ParentPage
        {
            get
            {
                DiagramElement elem = DE_FindPredecessorOfType(DiagramElement.ET_GENERAL_PAGE);
                if (elem is DiagramPage)
                    return elem as DiagramPage;
                return null;
            }
        }
    }
}
