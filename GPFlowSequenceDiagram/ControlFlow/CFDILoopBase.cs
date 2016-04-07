using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPFlowSequenceDiagram
{
    public class CFDILoopBase: DiagramItem
    {
        public float ExitWayX;
        public float ReturnWayX;

        public CFDILoopBase(DiagramElement de)
            : base(de)
        {
        }
    }
}
