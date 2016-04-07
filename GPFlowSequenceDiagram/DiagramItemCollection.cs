using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GPFlowSequenceDiagram
{
    public struct DiagramPoint
    {
        public float X;
        public float Y;

        public DiagramPoint(float iX, float iY)
        {
            X = iX;
            Y = iY;
        }

        public DiagramPoint(PointF point)
        {
            X = point.X;
            Y = point.Y;
        }

        public DiagramPoint DiagramPointByAdding(SizeF offset)
        {
            return new DiagramPoint(X + offset.Width, Y + offset.Height);
        }
    }

    public class DiagramItemCollection: DiagramElement, IEnumerable
    {
        private List<DiagramItem> items = new List<DiagramItem>();

        public DiagramItemCollection(DiagramElement parent)
            : base(parent)
        {
        }

        public override string ToString()
        {
            return string.Format("Collection {0}", ElementId);
        }

        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public void Add(DiagramItem dvi)
        {
            dvi.Parent = this;
            dvi.Id = Parent.DE_GetUniqueId();
            items.Add(dvi);
            Parent.DE_OnCollectionChanged();
        }

        public int Count
        {
            get { return items.Count; }
        }

        public DiagramItem this[int index]
        {
            get
            {
                return items[index];
            }
        }

        public void Clear()
        {
            items.Clear();
            Parent.DE_OnCollectionChanged();
        }

        public void Remove(DiagramItem dvi)
        {
            items.Remove(dvi);
            if (Parent != null)
            {
                Parent.DE_OnCollectionChanged();
                Parent.DE_RemoveConnectionWithItem(dvi.Id);
            }
        }

        public void ClearSelection()
        {
            foreach (DiagramItem item in items)
            {
                item.Selected = false;
            }
        }

        public DiagramItem FindItemWithId(int id)
        {
            foreach (DiagramItem dvi in items)
            {
                if (dvi.Id == id)
                    return dvi;
            }
            return null;
        }

        public override void DE_FindElements(DiagramContext context)
        {
            if (context.FoundElement != null)
                return;

            foreach (DiagramItem dvi in items)
            {
                if (context.FoundElement != null)
                    return;
                dvi.DE_FindElements(context);
            }
        }

    }
}
