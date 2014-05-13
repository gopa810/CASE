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

        public ItemPartInput(Item it)
        {
            Item = it;
        }

        public ItemPartInput(Item it, int type)
        {
            Item = it;
            PartType = type;
        }

        public ItemPartInput(Item it, int type, float x, float y)
        {
            Item = it;
            PartType = type;
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

        public ItemPartOutput GetLastItem()
        {
            return Item.EndPoint.GetLastItem();
        }

        public void RelayoutPreviousItems()
        {
            ItemPartInput itemPartStart = this.Item.OriginPoint;
            ItemPartOutput previousItemEnd = itemPartStart.RefItem;
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
