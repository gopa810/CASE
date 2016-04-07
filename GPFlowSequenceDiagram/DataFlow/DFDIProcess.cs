using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using GPFlowSequenceDiagram.ItemParts;

namespace GPFlowSequenceDiagram.DataFlow
{
    public class DFDIProcess: DiagramItem
    {
        public List<NamedPropertyType> Inputs;
        public List<NamedPropertyType> Outputs;
        public float PaddingSides;
        public StringItemArea Name;
        //public ItemPartRectangle mainArea;

        public DFDIProcess(DiagramElement parent)
            : base(parent)
        {
            PaddingSides = 8;
            Name = new StringItemArea();
            Inputs = new List<NamedPropertyType>();
            Outputs = new List<NamedPropertyType>();
            //mainArea = new ItemPartRectangle(this);
            OriginPoint.DE_SetCursor(Keys.None, Cursors.SizeAll);
        }

        public override SizeF DE_DrawShape(DiagramDrawingContext ctx, HighlightType highType)
        {
            Graphics g = ctx.Graphics;

            SizeF origSize = Name.GetSize(g);
            SizeF nameSize = new SizeF(origSize.Width + 2 * PaddingSides, origSize.Height + 2 * PaddingSides);
            UsedRectangle = new RectangleF(OriginPoint.X - nameSize.Width / 2, OriginPoint.Y,
                nameSize.Width, nameSize.Height);

            if (highType == HighlightType.NotDraw)
                return UsedRectangle.Size;

            Pen p1 = GetPenForHighlight(highType);
            Brush b1 = Brushes.Green;

            RectangleF rc = new RectangleF(OriginPoint.X - nameSize.Width / 2 + PaddingSides, OriginPoint.Y + PaddingSides,
                origSize.Width, origSize.Height);

            g.FillRectangle(b1, UsedRectangle);
            g.DrawRectangle(p1, UsedRectangle.X, UsedRectangle.Y, UsedRectangle.Width, UsedRectangle.Height);
            Name.DrawAtPoint(g, OriginPoint.X - nameSize.Width / 2 + PaddingSides, OriginPoint.Y + PaddingSides);

            return UsedRectangle.Size;
        }

        public override void DE_FindElements(DiagramContext context)
        {
            if (context.FoundElement != null)
                return;

            if (UsedRectangle.Contains(context.PagePoint.X, context.PagePoint.Y))
            {
                context.InsertElement(OriginPoint);
            }
        }

        public string ProcessName
        {
            get
            {
                return Name.Text;
            }
            set
            {
                Name.Text = value;
                DE_OnCollectionChanged();
            }
        }
    }
}
