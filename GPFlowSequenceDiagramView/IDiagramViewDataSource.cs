using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GPFlowSequenceDiagramView
{
    public interface IDiagramViewDataSource
    {
        ContextMenuStrip GetContextMenuStrip(DiagramViewContext ctx);
    }
}
