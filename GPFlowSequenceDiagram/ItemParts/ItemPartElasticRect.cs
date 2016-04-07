using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GPFlowSequenceDiagram
{
    public class ItemPartElasticRect : DiagramItemPart
    {
        public DiagramPageCondition Content;

        public ItemPartElasticRect(DiagramItem it): base(it)
        {
            Content = new DiagramPageCondition(this);
        }

        public ItemPartElasticRect(DiagramItem it, int type): base(it, type)
        {
            Content = new DiagramPageCondition(this);
        }


        public SizeF GetContentSize()
        {
            return new SizeF(64, 48);
        }
    }
}
