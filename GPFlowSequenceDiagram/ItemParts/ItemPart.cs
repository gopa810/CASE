using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class ItemPart
    {
        public const int ORIGIN_POINT = 1;
        public const int ENDING_POINT = 2;
        public const int DATA_IN = 11;
        public const int DATA_OUT = 12;
        public const int DATA_INOUT = 13;
        public const int PRIMARY_AREA = 21;
        public const int SECONDARY_AREA = 22;
        public const int TERCIARY_AREA = 23;
        public const int TOP_BORDER = 31;
        public const int BOTTOM_BORDER = 32;
        public const int RIGHT_BORDER = 33;
        public const int LEFT_BORDER = 34;
        public const int TOPLEFT_CORNER = 35;
        public const int TOPRIGHT_CORNER = 36;
        public const int BOTTOMLEFT_CORNER = 37;
        public const int BOTTOMRIGHT_CORNER = 38;

        public int PartType = 0;
        public Item Item = null;
        public Dictionary<Keys,Cursor> Cursors = null;
        public ConnectivityWanted WantsConnect = ConnectivityWanted.None;

        public ItemPart()
        {
        }

        public ItemPart(Item it)
        {
            Item = it;
        }

        public ItemPart(Item it, int type)
        {
            Item = it;
            PartType = type;
        }

        public Cursor GetCursor(Keys k)
        {
            if (Cursors == null || Cursors.ContainsKey(k) == false)
                return null;
            return Cursors[k];
        }

        public void SetCursor(Keys k, Cursor cr)
        {
            if (Cursors == null)
                Cursors = new Dictionary<Keys, Cursor>();
            Cursors[k] = cr;
        }

        public virtual ItemPart Copy()
        {
            return new ItemPart(Item, PartType);
        }

        public void ItemPartDidChanged()
        {
            if (Item != null)
                Item.ItemPartDidChanged(this);
        }

    }
}
