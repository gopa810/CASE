using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CASE.Controls.StateDiagramHelpers
{
    public class SDEntity
    {
        public int ID = 0;
        public string Name = string.Empty;
        public static Pen[] StatePens = new Pen[] {
            Pens.Black,
            new Pen(Color.Black, 2),
            Pens.Green
        };

        public Pen CurrentPen(SDItemState state)
        {
            switch (state)
            {
                case SDItemState.Selected:
                    return StatePens[1];
                case SDItemState.Tracked:
                    return StatePens[2];
                default:
                    return StatePens[0];
            }
        }
    }
}
