using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class DiagramItemPart: DiagramElement
    {
        public const int TOP_BORDER = 31;
        public const int BOTTOM_BORDER = 32;
        public const int RIGHT_BORDER = 33;
        public const int LEFT_BORDER = 34;
        public const int TOPLEFT_CORNER = 35;
        public const int TOPRIGHT_CORNER = 36;
        public const int BOTTOMLEFT_CORNER = 37;
        public const int BOTTOMRIGHT_CORNER = 38;

        public ConnectivityWanted WantsConnect = ConnectivityWanted.None;

        public DiagramItemPart(DiagramElement it): base(it)
        {
            ElementType = ET_GENERAL_PART;
        }

        public DiagramItemPart(DiagramElement it, int type): base(it)
        {
            ElementType = type;
        }

        public override string ToString()
        {
            return string.Format("Item {0}", ElementId);
        }

        public virtual DiagramItemPart Copy()
        {
            return new DiagramItemPart(Parent, ElementType);
        }

        public DiagramItem Item
        {
            get { return FirstItem(this); }
        }

        public static DiagramItem FirstItem(DiagramElement firstElem)
        {
            DiagramElement elem = firstElem;
            while (elem != null)
            {
                if (elem is DiagramItem)
                    return (DiagramItem)elem;
                elem = elem.Parent;
            }
            return null;
        }

        public static DiagramItemPart FirstItemPart(DiagramElement firstElem)
        {
            DiagramElement elem = firstElem;
            while (elem != null)
            {
                if (elem is DiagramItemPart)
                    return (DiagramItemPart)elem;
                elem = elem.Parent;
            }
            return null;
        }
    }
}
