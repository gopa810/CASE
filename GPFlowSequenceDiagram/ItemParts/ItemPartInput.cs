using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPFlowSequenceDiagram
{
    public class ItemPartInput: ItemPartPointF
    {
        protected ItemPartOutput p_ref_item = null;

        public ItemPartInput()
        {
        }

        public ItemPartInput(DiagramItem it): base(it)
        {
        }

        public ItemPartInput(DiagramItem it, int type): base(it,type)
        {
        }

        public ItemPartInput(DiagramItem it, int type, float x, float y): base(it)
        {
            ElementType = type;
            p_pt.X = x;
            p_pt.Y = y;
        }

        public ItemPartOutput RefItem
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

        public static ItemPartInput FirstItemPartInput(DiagramElement firstElem)
        {
            DiagramElement elem = firstElem;
            while (elem != null)
            {
                if (elem is ItemPartInput)
                    return (ItemPartInput)elem;
                elem = elem.Parent;
            }
            return null;
        }

        public ItemPartOutput GetLastItem()
        {
            return Item.EndPoint.GetLastOutputItem();
        }

        public DiagramItem GetHeadItem()
        {
            ItemPartInput itemPartStart = this.Item.OriginPoint;
            ItemPartOutput previousItemEnd = itemPartStart.RefItem;
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
    }
}
