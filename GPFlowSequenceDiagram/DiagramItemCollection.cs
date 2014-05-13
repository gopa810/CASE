using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GPFlowSequenceDiagram
{
    public class DiagramItemCollection
    {
        public DiagramItemDelegate Delegate = null;

        protected List<Item> Items = new List<Item>();

        public DiagramItemCollection(DiagramItemDelegate view)
        {
            Delegate = view;
        }

        public Item this[int index]
        {
            get
            {
                return Items[index];
            }
            set
            {
                Items[index] = value;
                Delegate.OnDiagramItemsCollectionChanged();
            }
        }

        public int Count
        {
            get { return Items.Count; }
        }

        public void Add(Item dvi)
        {
            dvi.Delegate = Delegate;
            dvi.Id = Delegate.GetUniqueId();
            Items.Add(dvi);
            Delegate.OnDiagramItemsCollectionChanged();
        }

        public void Clear()
        {
            Items.Clear();
            Delegate.OnDiagramItemsCollectionChanged();
        }

        public void Remove(Item dvi)
        {
            Items.Remove(dvi);
            if (Delegate != null)
            {
                Delegate.OnDiagramItemsCollectionChanged();
                Delegate.RemoveConnectionWithItem(dvi.Id);
            }
        }

        public void ClearSelection()
        {
            foreach (Item item in Items)
            {
                item.Selected = false;
            }
        }

        public Item FindItemWithId(int id)
        {
            foreach (Item dvi in Items)
            {
                if (dvi.Id == id)
                    return dvi;
            }
            return null;
        }
        public ItemPart FindItemContainingPoint(PointF pt)
        {
            ItemPart found = null;
            foreach (Item dvi in Items)
            {
                found = dvi.GetHitItem(pt);
                if (found != null)
                    return found;
            }
            return null;
        }
        public List<ItemPart> FindItemsAtPoint(PointF pt)
        {
            List<ItemPart> found = new List<ItemPart>();
            ItemPart item = null;
            foreach (Item dvi in Items)
            {
                item = dvi.GetHitItem(pt);
                if (item != null)
                {
                    int insIndex = 0;
                    for (int i = 0; i < found.Count; i++)
                    {
                        if (found[i].PartType < item.PartType)
                            insIndex = i + 1;
                    }
                    found.Insert(insIndex, item);
                }
            }
            return found;
        }
    }
}
