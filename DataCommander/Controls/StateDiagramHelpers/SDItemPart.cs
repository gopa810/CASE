using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CASE.Controls.StateDiagramHelpers
{
    public class SDItemPart
    {
        public SDItem Item = null;
        public Cursor Cursor = null;

        public SDItemPart(SDItem item)
        {
            Item = item;
        }
        public SDItemPart(SDItem item, Cursor cur)
        {
            Item = item;
            Cursor = cur;
        }
    }
}
