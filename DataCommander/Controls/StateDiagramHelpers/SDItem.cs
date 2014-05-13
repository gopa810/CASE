using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CASE.Controls.StateDiagramHelpers
{
    public class SDItem: SDEntity
    {
        public static float StateItemDiameter = 16;
        public PointF Location = new PointF(0,0);
        public bool Selected = false;

        public SDItemPart PartArea;
        public SDItemPart PartConnector;

        public SDItem()
        {
            PartArea = new SDItemPart(this);
            PartConnector = new SDItemPart(this);
        }

        public virtual void Paint(Graphics g, SDItemState state)
        {
            g.DrawEllipse(CurrentPen(state), Location.X - StateItemDiameter,
                Location.Y - StateItemDiameter, StateItemDiameter * 2, StateItemDiameter * 2);
            g.DrawString(this.Name, SystemFonts.MenuFont, Brushes.Black, Location.X + StateItemDiameter + 8, Location.Y - 8);
            if (state == SDItemState.Selected)
            {
                float x = Location.X + 24;
                float y = Location.Y - 32;
                g.DrawLine(StatePens[2], x, y, x + 16, y);
                g.DrawLine(StatePens[2], x + 16, y, x + 16, y + 16);
                g.DrawLine(StatePens[2], x + 16, y + 16, x, y + 16);
                g.DrawLine(StatePens[2], x, y + 16, x, y);
                g.DrawLine(StatePens[2], x + 3, y + 3, x + 13, y + 13);
                g.DrawLine(StatePens[2], x + 13, y + 10, x + 13, y + 13);
                g.DrawLine(StatePens[2], x + 13, y + 13, x + 10, y + 13);
            }
        }

        public SDItemPart HitPart(PointF mouse)
        {
            if ((mouse.X - Location.X) * (mouse.X - Location.X) + (mouse.Y - Location.Y) * (mouse.Y - Location.Y) < StateItemDiameter*StateItemDiameter)
            {
                return PartArea;
            }

            if ((mouse.X > Location.X + 24) && (mouse.X < Location.X + 40)
                && (mouse.Y > Location.Y - 32) && (mouse.Y < Location.Y - 16))
            {
                return PartConnector;
            }

            return null;
        }
    }
}
