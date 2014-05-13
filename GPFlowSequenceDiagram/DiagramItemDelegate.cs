using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GPFlowSequenceDiagram
{
    public interface DiagramItemDelegate
    {
        void OnDiagramItemsCollectionChanged();
        int GetUniqueId();
        void RemoveConnectionWithItem(int itemId);
        Graphics GetGraphics();
    }
}
