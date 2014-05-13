using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CASE.Controls.StateDiagramHelpers;

namespace CASE.Controls
{
    public partial class StateDiagramView : UserControl
    {
        public StateDiagramView()
        {
            InitializeComponent();
        }

        private PointF p_centerOffset = new PointF(0, 0);
        private float[] p_scale_array = new float[] { 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.85f, 1.0f, 1.5f, 2.0f, 2.5f, 3.0f, 4.0f };
        private int p_scale_index = 7;
        public SDItem TrackStartItem = null;
        public SDItem TrackEndItem = null;
        public SDItem TrackTempItem = null;

        /// <summary>
        /// properties of diagram
        /// </summary>
        public List<SDItem> Items = new List<SDItem>();
        public List<SDTransition> Transitions = new List<SDTransition>();
        private int IdCounter = 1;

        /// <summary>
        /// painting of diagram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StateDiagramView_Paint(object sender, PaintEventArgs e)
        {
            DrawToBuffer(e.Graphics);
        }

        private void DrawToBuffer(Graphics g)
        {
            SizeF size = Size;

            g.FillRectangle(SystemBrushes.Control, 0, 0, this.Width, this.Height);

            g.TranslateTransform(size.Width / 2 + p_centerOffset.X, size.Height / 2 + p_centerOffset.Y);
            g.ScaleTransform(p_scale_array[p_scale_index], p_scale_array[p_scale_index]);

            SDItem[] trackers = new SDItem[] { TrackStartItem, TrackEndItem };

            for (int i = 0; i < Items.Count; i++)
            {
                SDItem dvi = Items[i];
                if (TrackStartItem == dvi || TrackEndItem == dvi || dvi == TrackTempItem)
                    dvi.Paint(g, SDItemState.Tracked);
                else if (dvi.Selected)
                    dvi.Paint(g, SDItemState.Selected);
                else
                    dvi.Paint(g, SDItemState.Normal);
            }
        }

        public float ScaleRatio
        {
            get
            {
                return p_scale_array[p_scale_index];
            }
        }

        public PointF LogicalCoordinatesFromClientPoint(float X, float Y)
        {
            SizeF size = Size;
            return new PointF((X - size.Width / 2 - p_centerOffset.X) / ScaleRatio, (Y - size.Height / 2 - p_centerOffset.Y) / ScaleRatio);
        }

        public void AddNewStateItem()
        {
            SDItem sdi = new SDItem();
            sdi.ID = IdCounter++;

            RectangleF rect = new RectangleF();
            if (CalculateStateItemsBounds(ref rect))
            {
                sdi.Location.X = (rect.Left + rect.Right) / 2;
                sdi.Location.Y = rect.Bottom + 64;
            }
            else
            {
                sdi.Location = new PointF(0, 0);
            }
            Items.Add(sdi);
        }

        public bool CalculateStateItemsBounds(ref RectangleF rect)
        {
            int stateDiameter = 16;
            bool leftInit = false;
            bool rightInit = false;
            bool topInit = false;
            bool bottomInit = false;

            foreach (SDItem item in Items)
            {
                if (item.Location.X - stateDiameter < rect.Left || leftInit == false)
                {
                    leftInit = true;
                    rect.X = item.Location.X - stateDiameter;
                }
                if (item.Location.X + stateDiameter > rect.Right || rightInit == false)
                {
                    rightInit = true;
                    rect.Width = item.Location.X + stateDiameter - rect.X;
                }
                if (item.Location.Y - stateDiameter < rect.Top || topInit == false)
                {
                    topInit = true;
                    rect.Y = item.Location.Y - stateDiameter;
                }
                if (item.Location.Y + stateDiameter > rect.Bottom || bottomInit == false)
                {
                    bottomInit = true;
                    rect.Height = item.Location.Y + stateDiameter - rect.Y;
                }
            }

            return bottomInit;
        }

        private void StateDiagramView_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void StateDiagramView_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void StateDiagramView_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void StateDiagramView_MouseLeave(object sender, EventArgs e)
        {

        }

    }
}
