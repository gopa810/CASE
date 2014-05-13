using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPFlowSequenceDiagram
{
    public class ItemPartCompoundPoint: ItemPart
    {
        public ItemPartFloat X = null;
        public ItemPartFloat Y = null;

        public ItemPartCompoundPoint()
        {
        }
        public ItemPartCompoundPoint(Item it)
        {
            Item = it;
        }
        public ItemPartCompoundPoint(Item it, int type)
        {
            Item = it;
            PartType = type;
        }
        public ItemPartCompoundPoint(Item it, int type, ItemPartFloat px, ItemPartFloat py)
        {
            Item = it;
            PartType = type;
            X = px;
            Y = py;
        }

        public override ItemPart Copy()
        {
            ItemPartPointF p = new ItemPartPointF(Item, PartType);
            if (X != null)
                p.X = X.Value;
            if (Y != null)
                p.Y = Y.Value;
            return p;
        }
    }
}
