using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPFlowSequenceDiagram
{
    public class ItemPartFloat: DiagramItemPart
    {
        public float Value = 0;

        public ItemPartFloat(): base(null)
        {
        }
        public ItemPartFloat(DiagramElement it): base(it)
        {
        }
        public ItemPartFloat(DiagramElement it, int type): base(it)
        {
            ElementType = type;
        }
        public ItemPartFloat(DiagramElement it, int type, float value): base(it)
        {
            ElementType = type;
            Value = value;
        }

        public override DiagramItemPart Copy()
        {
            return new ItemPartFloat(Parent, ElementType, Value);
        }
    }
}
