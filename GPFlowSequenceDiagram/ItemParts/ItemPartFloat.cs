using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPFlowSequenceDiagram
{
    public class ItemPartFloat: ItemPart
    {
        public float Value = 0;

        public ItemPartFloat()
        {
        }
        public ItemPartFloat(Item it)
        {
            Item = it;
        }
        public ItemPartFloat(Item it, int type)
        {
            Item = it;
            PartType = type;
        }
        public ItemPartFloat(Item it, int type, float value)
        {
            Item = it;
            PartType = type;
            Value = value;
        }

        public override ItemPart Copy()
        {
            return new ItemPartFloat(Item, PartType, Value);
        }
    }
}
