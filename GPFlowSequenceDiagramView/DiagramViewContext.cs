using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using GPFlowSequenceDiagram;

namespace GPFlowSequenceDiagramView
{
    public enum PointerPosition
    {
        None,
        Inside,
        Bottom,
        Top,
        Left,
        Right,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        OriginAnchor,
        EndingAnchor
    }
    
    public class DiagramViewContext
    {
        public Point ClientLocation = new Point(0, 0);
        public Point ScreenLocation = new Point(0, 0);
        public View View = null;
        public List<ItemPart> ItemParts = null;
        public PointF DiagramLocation = new PointF(0, 0);
        public ItemPart ItemPart
        {
            get
            {
                if (ItemParts == null || ItemParts.Count == 0)
                    return null;
                return ItemParts[0];
            }
        }

        public ItemPart FindConnectivityForPart(ItemPart part)
        {
            if (part.WantsConnect == ConnectivityWanted.StartPointWanted)
            {
                foreach(ItemPart item in ItemParts)
                {
                    if (item != part && item.PartType == ItemPart.ORIGIN_POINT)
                        return item;
                }
            }
            else if (part.WantsConnect == ConnectivityWanted.EndPointWanted)
            {
                foreach (ItemPart item in ItemParts)
                {
                    if (item != part && item.PartType == ItemPart.ENDING_POINT)
                        return item;
                }
            }

            return null;
        }
    }
}
