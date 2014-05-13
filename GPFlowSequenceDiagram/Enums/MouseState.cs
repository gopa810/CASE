using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPFlowSequenceDiagram
{
    public enum MouseState
    {
        None,
        // moving whole diagram
        DiagramMove,
        // moving selected items
        ItemMove,
        // resizing only one item
        ItemResize,
        // creating connection
        ConnectionCreate
    }
}
