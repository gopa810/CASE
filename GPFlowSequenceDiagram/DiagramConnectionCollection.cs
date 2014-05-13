using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPFlowSequenceDiagram
{
    public class DiagramConnectionCollection
    {
        private List<DiagramViewConnection> Items = new List<DiagramViewConnection>();
        public DiagramItemDelegate Delegate;
        public DiagramConnectionCollection(DiagramItemDelegate aView)
        {
            Delegate = aView;
        }

        public int Count
        {
            get
            {
                return Items.Count;
            }
        }

        public DiagramViewConnection this[int i]
        {
            get { return Items[i]; }
            set { Items[i] = value; }
        }

        public void Add(DiagramViewConnection dvc)
        {
            dvc.Collection = this;
            if (Delegate != null)
                dvc.Id = Delegate.GetUniqueId();
            Items.Add(dvc);
            Delegate.OnDiagramItemsCollectionChanged();
        }

        public void Remove(DiagramViewConnection dvc)
        {
            Items.Remove(dvc);
            Delegate.OnDiagramItemsCollectionChanged();
        }

        public void RemoveWithItemId(int id)
        {
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                if (Items[i].SourceId == id || Items[i].DestinationId == id)
                {
                    Items.RemoveAt(i);
                }
            }
        }

        public void InvalidatePointsWithItemId(int id)
        {
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                if (Items[i].SourceId == id || Items[i].DestinationId == id)
                {
                    Items[i].PointsValid = false;
                }
            }
        }

        public void Clear()
        {
            Items.Clear();
            Delegate.OnDiagramItemsCollectionChanged();
        }
    }
}
