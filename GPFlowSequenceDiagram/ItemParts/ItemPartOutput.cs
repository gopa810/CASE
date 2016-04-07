using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Diagnostics;

namespace GPFlowSequenceDiagram
{
    public class ItemPartOutput: ItemPartPointF
    {
        protected ItemPartInput p_ref_item = null;
        public bool MoveReferencedItemPart = true;

        public ItemPartOutput(): base(null)
        {
        }

        public ItemPartOutput(DiagramItem it): base(it)
        {
        }

        public ItemPartOutput(DiagramItem it, int type): base(it,type)
        {
        }

        public ItemPartOutput(DiagramItem it, int type, float x, float y): base(it, type)
        {
            p_pt.X = x;
            p_pt.Y = y;
        }

        public ItemPartInput RefItem
        {
            get
            {
                return p_ref_item;
            }
            set
            {
                p_ref_item = value;
                if (value != null && value.Item != null)
                {
                    p_pt.X = value.X;
                    p_pt.Y = value.Y;
                }
            }
        }

        public static ItemPartOutput FirstItemPartOutput(DiagramElement firstElem)
        {
            DiagramElement elem = firstElem;
            while (elem != null)
            {
                if (elem is ItemPartOutput)
                    return (ItemPartOutput)elem;
                elem = elem.Parent;
            }
            return null;
        }
        public ItemPartOutput GetLastOutputItem()
        {
            ItemPartOutput ipi = this;

            while (ipi.RefItem != null)
            {
                DiagramItem it = ipi.RefItem.Item;
                if (it != null)
                {
                    ipi = it.EndPoint;
                }
                else
                {
                    break;
                }
            }

            return ipi;
        }

        public SizeF CalculateSuccessorSize(DiagramDrawingContext ctx)
        {
            SizeF mainRect = new SizeF(0,0);
            ItemPartOutput ipi = this;

            while (ipi.RefItem != null)
            {
                DiagramItem it = ipi.RefItem.Item;
                if (it != null)
                {
                    SizeF addedRect = it.DE_DrawShape(ctx, HighlightType.NotDraw);
                    mainRect.Width = Math.Max(mainRect.Width, addedRect.Width);
                    mainRect.Height += addedRect.Height;

                    ipi = it.EndPoint;
                }
                else
                {
                    ipi = null;
                }
            }

            return mainRect;
        }

        public DiagramItem GetHeadItem()
        {
            ItemPartInput itemPartStart = this.Item.OriginPoint;
            ItemPartOutput previousItemEnd = this;
            DiagramItem previousItem = null;
            while (previousItemEnd != null)
            {
                previousItem = previousItemEnd.Item;
                itemPartStart = previousItem.OriginPoint;
                if (itemPartStart.RefItem == null)
                    break;
                previousItemEnd = itemPartStart.RefItem;
            }

            return previousItem;
        }

        public void SetPosition(float pX, float pY)
        {
            if (pX != X || pY != Y)
            {
                X = pX;
                Y = pY;
            }
        }
    }
}
