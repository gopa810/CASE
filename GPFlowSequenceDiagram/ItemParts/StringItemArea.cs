using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GPFlowSequenceDiagram.ItemParts
{
    public class StringItemArea
    {
        private StringFormat p_format = new StringFormat();
        private string p_text = string.Empty;
        private SizeF p_text_size = SizeF.Empty;
        private Font p_font = DrawProperties.fontSmallTitles;

        public ItemPadding Padding = new ItemPadding(5);

        public SizeF MinimumSize { get; set; }
        public StringAlignment VerticalAlign { get { return p_format.Alignment; } set { p_format.Alignment = value; } }
        public StringAlignment HorizontalAlign { get { return p_format.LineAlignment; } set { p_format.LineAlignment = value; } }


        public StringItemArea()
        {
            MinimumSize = new SizeF(16, 16);
            VerticalAlign = StringAlignment.Center;
            HorizontalAlign = StringAlignment.Center;
        }

        public Font Font
        {
            get 
            {
                return p_font;
            }
            set
            {
                if (value != null)
                    p_font = value;
                p_text_size = SizeF.Empty;
            }
        }

        public string Text
        {
            get
            {
                return p_text;
            }
            set
            {
                if (value == null)
                    p_text = String.Empty;
                else
                    p_text = value;
                p_text_size = SizeF.Empty;
            }
        }

        public SizeF GetSize(Graphics g)
        {
            if (p_text_size.Height < 1)
            {
                p_text_size = g.MeasureString(p_text, p_font);
            }

            return new SizeF(Math.Max(MinimumSize.Width, p_text_size.Width + Padding.Left + Padding.Right),
                Math.Max(MinimumSize.Height, p_text_size.Height + Padding.Top + Padding.Bottom));
        }

        public void DrawAtPoint(Graphics g, float x, float y)
        {
            SizeF rawSize = GetSize(g);
            RectangleF rcf = new RectangleF(x + Padding.Left, y + Padding.Top, 
                rawSize.Width - Padding.Left - Padding.Right, rawSize.Height - Padding.Top - Padding.Bottom);

            g.DrawString(p_text, p_font, Brushes.Black, rcf, p_format);
        }

        public void DrawAtPointWithWidth(Graphics g, float x, float y, float width)
        {
            SizeF rawSize = GetSize(g);
            RectangleF rcf = new RectangleF(x + Padding.Left, y + Padding.Top,
                rawSize.Width - Padding.Left - Padding.Right, rawSize.Height - Padding.Top - Padding.Bottom);
            rcf.Width = Math.Max(rcf.Width, width);

            g.DrawString(p_text, p_font, Brushes.Black, rcf, p_format);
        }

    }
}
