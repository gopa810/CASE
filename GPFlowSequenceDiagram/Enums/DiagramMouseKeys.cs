using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class DiagramMouseKeys
    {
        public bool Control = false;
        public bool Shift = false;
        public bool Alt = false;

        public Keys KeyCode
        {
            get
            {
                return (Control ? Keys.ControlKey : (Shift ? Keys.ShiftKey : Keys.None));
            }
        }
    }
}
