using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CASE.Controls.StateDiagramHelpers
{
    public class SDTransition: SDEntity
    {
        public int FromState = 0;
        public int ToState = 0;
        public PointF Location = new PointF();
    }
}
