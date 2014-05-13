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

        public ItemPartOutput()
        {
        }

        public ItemPartOutput(Item it)
        {
            Item = it;
        }

        public ItemPartOutput(Item it, int type)
        {
            Item = it;
            PartType = type;
        }

        public ItemPartOutput(Item it, int type, float x, float y)
        {
            Item = it;
            PartType = type;
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

        public void MergeRectangles(ref RectangleF mainRect, RectangleF addedRect)
        {
            float t, b, r, l;

            t = mainRect.Top;
            b = mainRect.Bottom;
            r = mainRect.Right;
            l = mainRect.Left;
            if (addedRect.Bottom > b)
            {
                b = addedRect.Bottom;
            }
            if (addedRect.Top < t)
            {
                t = addedRect.Top;
            }
            if (addedRect.Right > r)
            {
                r = addedRect.Right;
            }
            if (addedRect.Left < l)
            {
                l = addedRect.Left;
            }
            mainRect.X = l;
            mainRect.Y = t;
            mainRect.Width = r - l;
            mainRect.Height = b - t;
        }

        public ItemPartOutput GetLastItem()
        {
            ItemPartOutput ipi = this;

            while (ipi.RefItem != null)
            {
                Item it = ipi.RefItem.Item;
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

        public RectangleAnchored CalculateSubordinatesDrawingRectangle()
        {
            RectangleAnchored mainRect = null;
            ItemPartOutput ipi = this;

            //Debugger.Log(0, "", " ----- \n");
            while (ipi.RefItem != null)
            {
                Item it = ipi.RefItem.Item;
                if (it != null)
                {
                    RectangleAnchored addedRect = it.DrawingRectangle;
                    if (mainRect == null)
                        mainRect = addedRect;
                    else
                        mainRect.MergeRectangles(addedRect);
                    //Debugger.Log(0, "", "   -> AddedRectangle=" + addedRect.ToString() + "\n");
                    //Debugger.Log(0, "", "  --> MainRectangle=" + mainRect.ToString() + "\n");
                    ipi = it.EndPoint;
                }
                else
                {
                    ipi = null;
                }
            }

            if (mainRect == null)
                mainRect = new RectangleAnchored();

            //Debugger.Log(0, "", "MainRectangle=" + mainRect.ToString() + "\n");
            return mainRect;
        }

        public void RelayoutPreviousItems()
        {
            ItemPartInput itemPartStart = this.Item.OriginPoint;
            ItemPartOutput previousItemEnd = this;// itemPartStart.RefItem;
            Item previousItem = null;
            while (previousItemEnd != null)
            {
                previousItem = previousItemEnd.Item;
                itemPartStart = previousItem.OriginPoint;
                itemPartStart.ItemPartDidChanged();
                previousItemEnd = itemPartStart.RefItem;
            }
        }
    }
}
