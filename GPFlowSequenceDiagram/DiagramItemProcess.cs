using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;


namespace GPFlowSequenceDiagram
{
    public class DiagramItemProcess: ItemRectangleArea
    {
        public string ProcessName;
        public string ReturnDataType;

        public override void Paint(Graphics g, HighlightType highType)
        {
            RectangleF r = this.Bounds;

            Pen p1 = GetPenForHighlight(highType);
            Brush b1 = GetBrushForHighlight(highType);

            // draw origin
            g.DrawEllipse(p1, OriginPoint.X - 3, OriginPoint.Y - 3, 6, 6);
            g.DrawLine(p1, OriginPoint.X, OriginPoint.Y + 3, OriginPoint.X, r.Y);
            // draw rectangle
            g.FillRectangle(b1, this.Bounds);
            g.DrawRectangle(p1, r.X, r.Y, r.Width, r.Height);
            g.DrawString(this.Text, SystemFonts.DialogFont, Brushes.Black, r.X + 3, r.Y + 3);
            // draw ending line
            g.DrawLine(p1, OriginPoint.X, PartBottomBorder.Value, EndPoint.X, EndPoint.Y - 3);
            g.DrawEllipse(p1, EndPoint.X - 3, EndPoint.Y - 3, 6, 6);

            if (this.Subtext.Length > 0)
            {
                g.DrawString(this.Subtext, SystemFonts.MenuFont, Brushes.Gray, r.X + 3, r.Y + SystemInformation.MenuFont.Height + 2);
            }
        }

    }
}
