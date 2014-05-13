using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GPFlowSequenceDiagram;

namespace GPFlowSequenceDiagramView
{
    public partial class View : UserControl, DiagramItemDelegate
    {
        private Graphics measureGraphics = null;
        public delegate void MyDelegate();

        public event MyDelegate SelectedItemChanged;

        private float[] p_scale_array = new float[] { 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.85f, 1.0f, 1.5f, 2.0f, 2.5f, 3.0f, 4.0f};
        private int p_scale_index = 7;
        public IDiagramViewDataSource DataSource = null;
        private int p_uniqueId = 1;
        //private float p_scale = 1;
        private ItemPart p_trackingItem = null;
        private ItemPart p_startingValue = null;
        private PointF p_startingLogPoint = new PointF();
        private PointF p_startingPhysPoint = new PointF();
        private Item p_highlightedTemporary = null;

        private PointF p_centerOffset = new PointF(0, 0);
        private MouseState p_mouse_state = MouseState.None;
        private PointF p_prevPos = new PointF();

        private DiagramMouseKeys keys = new DiagramMouseKeys();

        private Pen p_penNormal = null;
        private Pen p_penBold = null;
        private Pen p_penHighlight = null;

        private Item p_connStartItem = null;
        private Item p_connEndItem = null;
        private PointF p_connStartLog = new PointF();
        private PointF p_connEndLog = new PointF();

        public DiagramItemCollection Items = null;
        public DiagramConnectionCollection Conns = null;

        public bool policyMoveOriginDisconnects = true;

        //private BufferedGraphicsContext context;
        //private BufferedGraphics grafx;

        public View()
        {
            Items = new DiagramItemCollection(this);
            Conns = new DiagramConnectionCollection(this);
            InitializeComponent();

            p_penNormal = new Pen(Color.Black, 1f);
            p_penBold = new Pen(Color.Black, 2f);
            p_penHighlight = new Pen(Color.Green, 2f);

            //context = BufferedGraphicsManager.Current;
            //context.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            //grafx = context.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));

            //DrawToBuffer(grafx.Graphics);
        }

        public float ScaleRatio
        {
            get
            {
                return p_scale_array[p_scale_index];
            }
        }

        public Graphics GetGraphics()
        {
            if (measureGraphics == null)
            {
                measureGraphics = this.CreateGraphics();
            }

            return measureGraphics;
        }

        private DiagramViewContext GetCurrentMouseContext(int X, int Y)
        {
            DiagramViewContext dvc = new DiagramViewContext();

            dvc.ClientLocation = new Point(X, Y);
            dvc.ScreenLocation = PointToScreen(dvc.ClientLocation);
            dvc.View = this;

            PointF logc = LogicalCoordinatesFromClientPoint(X, Y);
            List<ItemPart> dvi = Items.FindItemsAtPoint(logc);

            dvc.ItemParts = dvi;
            dvc.DiagramLocation = logc;


            return dvc;
        }

        private void DiagramView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (DataSource != null)
                {
                    DiagramViewContext dvc = GetCurrentMouseContext(e.X, e.Y);
                    ContextMenuStrip cms = DataSource.GetContextMenuStrip(dvc);
                    if (cms != null)
                    {
                        foreach (ToolStripItem tsi in cms.Items)
                        {
                            tsi.Tag = dvc;
                        }
                        cms.Show(this.PointToScreen(new Point(e.X, e.Y)));
                    }
                }
            }
        }

        private void DiagramView_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void SetCursorBasedOnHotspot(PointerPosition pos)
        {
            switch (pos)
            {
                case PointerPosition.Bottom:
                    Cursor.Current = Cursors.SizeNS;
                    break;
                case PointerPosition.Top:
                    Cursor.Current = Cursors.SizeNS;
                    break;
                case PointerPosition.TopLeft:
                    Cursor.Current = Cursors.SizeNWSE;
                    break;
                case PointerPosition.TopRight:
                    Cursor.Current = Cursors.SizeNESW;
                    break;
                case PointerPosition.Left:
                    Cursor.Current = Cursors.SizeWE;
                    break;
                case PointerPosition.Right:
                    Cursor.Current = Cursors.SizeWE;
                    break;
                case PointerPosition.BottomLeft:
                    Cursor.Current = Cursors.SizeNESW;
                    break;
                case PointerPosition.BottomRight:
                    Cursor.Current = Cursors.SizeNWSE;
                    break;
                default:
                    break;
            }
        }

        private void DiagramView_MouseMove(object sender, MouseEventArgs e)
        {
            DiagramViewContext ctx = GetCurrentMouseContext(e.X, e.Y);
            if (p_trackingItem != null)
            {
                PointF ptCurrent = ctx.DiagramLocation;
                if (p_trackingItem.Item != null)
                {
                    SizeF diff = new SizeF(ptCurrent.X - p_startingLogPoint.X, ptCurrent.Y - p_startingLogPoint.Y);
                    p_trackingItem.Item.MouseMove(p_trackingItem, p_startingValue, diff, keys);

                    if (p_trackingItem is ItemPartInput)
                    {
                        ItemPartInput ipi = p_trackingItem as ItemPartInput;
                        if (policyMoveOriginDisconnects)
                        {
                            ItemPartOutput ipo = ipi.RefItem;

                            if (ipi.RefItem != null)
                                ipi.RefItem.RefItem = null;
                            ipi.RefItem = null;

                            if (ipo != null)
                            {
                                PointF positionBeforeRelayout = ipo.Point;
                                // in the loop we update origin points to all previous nodes
                                ipo.RelayoutPreviousItems();

                                p_centerOffset.X -= (ipo.X - positionBeforeRelayout.X);
                                p_centerOffset.Y -= (ipo.Y - positionBeforeRelayout.Y);
                            }
                        }
                        else if (ipi.RefItem != null && ipi.RefItem is ItemPartOutput)
                        {
                            (ipi.RefItem as ItemPartOutput).Point = ipi.Point;
                            if (ipi.RefItem.Item != null)
                                ipi.RefItem.Item.ItemPartDidChanged(ipi.RefItem);
                        }
                    }
                    ItemPart connPart = ctx.FindConnectivityForPart(p_trackingItem);
                    if (connPart != null)
                    {
                        p_highlightedTemporary = connPart.Item;
                    }
                    else
                    {
                        p_highlightedTemporary = null;
                    }


                    RedrawClientScreen();
                }
            }
            else
            {
                if (ctx != null && ctx.ItemPart != null)
                {
                    Cursor proposedCursor = ctx.ItemPart.GetCursor(keys.KeyCode);
                    if (proposedCursor != null)
                        Cursor.Current = proposedCursor;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalItemSize">Original size of item taken during event MouseDown</param>
        /// <param name="resizeDifference">Difference between current mouse position and original mouse position during MouseDown event.</param>
        /// <param name="pos">Position of cursor within item</param>
        /// <returns></returns>
        private SizeF CalculateNewItemSize(SizeF originalItemSize, SizeF resizeDifference, PointerPosition pos)
        {
            SizeF proposedItemSize = new SizeF();
            switch (pos)
            {
                case PointerPosition.Bottom:
                    proposedItemSize.Height = originalItemSize.Height + 2 * resizeDifference.Height;
                    break;
                case PointerPosition.Top:
                    proposedItemSize.Height = originalItemSize.Height - 2 * resizeDifference.Height;
                    break;
                case PointerPosition.TopLeft:
                    proposedItemSize.Height = originalItemSize.Height - 2 * resizeDifference.Height;
                    proposedItemSize.Width = originalItemSize.Height - 2 * resizeDifference.Width;
                    break;
                case PointerPosition.TopRight:
                    proposedItemSize.Height = originalItemSize.Height - 2 * resizeDifference.Height;
                    proposedItemSize.Width = originalItemSize.Width + 2 * resizeDifference.Width;
                    break;
                case PointerPosition.Left:
                    proposedItemSize.Width = originalItemSize.Width - 2 * resizeDifference.Width;
                    break;
                case PointerPosition.Right:
                    proposedItemSize.Width = originalItemSize.Width + 2 * resizeDifference.Width;
                    break;
                case PointerPosition.BottomLeft:
                    proposedItemSize.Height = originalItemSize.Height + 2 * resizeDifference.Height;
                    proposedItemSize.Width = originalItemSize.Width - 2 * resizeDifference.Width;
                    break;
                case PointerPosition.BottomRight:
                    proposedItemSize.Height = originalItemSize.Height + 2 * resizeDifference.Height;
                    proposedItemSize.Width = originalItemSize.Width + 2 * resizeDifference.Width;
                    break;
                default:
                    break;
            }

            return proposedItemSize;
        }

        private void DiagramView_MouseDown(object sender, MouseEventArgs e)
        {
            Capture = true;
            if (e.Button == MouseButtons.Left)
            {
                p_prevPos.X = e.X;
                p_prevPos.Y = e.Y;

                DiagramViewContext ctx = GetCurrentMouseContext(e.X, e.Y);
                p_trackingItem = ctx.ItemPart;
                p_startingValue = p_trackingItem == null ? null : p_trackingItem.Copy();
                p_startingLogPoint.X = ctx.DiagramLocation.X;
                p_startingLogPoint.Y = ctx.DiagramLocation.Y;
                p_startingPhysPoint.X = e.X;
                p_startingPhysPoint.Y = e.Y;

                if (!keys.Control)
                {
                    if (!keys.Shift)
                        Items.ClearSelection();
                    if (ctx.ItemPart != null && ctx.ItemPart.Item != null)
                    {
                        ctx.ItemPart.Item.Selected = true;
                    }
                }
                if (ctx.ItemPart != null)
                {
                    Cursor cur = ctx.ItemPart.GetCursor(keys.KeyCode);
                    if (cur != null)
                        Cursor.Current = cur;
                }


            }
        }

        private void DiagramView_MouseWheel(object sender, MouseEventArgs e)
        {
            int a = e.Delta/120;

            if (p_scale_index > 0 && e.Delta < 0)
            {
                p_scale_index--;
            }
            else if (e.Delta > 0 && p_scale_index < p_scale_array.Length - 1)
            {
                p_scale_index++;
            }

            RedrawClientScreen();
        }

        private void DiagramView_MouseUp(object sender, MouseEventArgs e)
        {
            DiagramViewContext ctx = GetCurrentMouseContext(e.X, e.Y);

            if (p_trackingItem != null && p_trackingItem.Item != null)
            {
                Item trackItem = p_trackingItem.Item;
                trackItem.MouseUp(p_trackingItem);
                ItemPart partConn = ctx.FindConnectivityForPart(p_trackingItem);
                if (partConn != null)
                {
                    if (p_trackingItem.WantsConnect == ConnectivityWanted.EndPointWanted)
                    {
                        if (partConn is ItemPartOutput && p_trackingItem is ItemPartInput)
                        {
                            ItemPartInput input = p_trackingItem as ItemPartInput;
                            ItemPartOutput output = partConn as ItemPartOutput;
                            if (input.RefItem != output)
                            {
                                ItemPartInput previousInput = output.RefItem;
                                ItemPartOutput lastItem = input.GetLastItem();
                                if (previousInput != null)
                                {
                                    // we connects output to input
                                    // if output was previously connected
                                    // to another input, then we are inserting
                                    // new nodes, and node, which is now previousINput
                                    // will be concatenated at the end of appended nodes
                                    previousInput.RefItem = null;
                                }
                                input.RefItem = output;
                                input.ItemPartDidChanged();
                                output.RefItem = input;
                                output.MoveReferencedItemPart = true;
                                if (previousInput != null)
                                {
                                    previousInput.RefItem = lastItem;
                                    previousInput.ItemPartDidChanged();
                                    lastItem.RefItem = previousInput;
                                }

                                // we updated successors already
                                // now we need to update also predecessors
                                // position of currently selected (inserted) item
                                // may be changed, so we store its position here
                                PointF positionBeforeRelayout = input.Point;
                                // in the loop we update origin points to all previous nodes
                                input.RelayoutPreviousItems();

                                p_centerOffset.X -= (input.X - positionBeforeRelayout.X);
                                p_centerOffset.Y -= (input.Y - positionBeforeRelayout.Y);
                            }
                        }
                    }
                    else if (p_trackingItem.WantsConnect == ConnectivityWanted.StartPointWanted)
                    {
                        if (partConn is ItemPartInput && p_trackingItem is ItemPartOutput)
                        {
                            (p_trackingItem as ItemPartOutput).RefItem = (partConn as ItemPartInput);
                            trackItem.ItemPartDidChanged(p_trackingItem);
                            (partConn as ItemPartInput).RefItem = (p_trackingItem as ItemPartOutput);
                            (partConn as ItemPartOutput).MoveReferencedItemPart = false;
                        }
                    }
                }
                p_highlightedTemporary = null;
                RedrawClientScreen();
            }

            p_startingValue = null;
            p_trackingItem = null;
            Capture = false;
        }

        private void DiagramView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == (Keys.LButton | Keys.ShiftKey))
                keys.Control = true;
            else if (e.KeyCode == Keys.ShiftKey)
                keys.Shift = true;
            else if (e.KeyCode == Keys.Escape)
            {
                CancelMouseOperation();
            }
            Debugger.Log(0, "", "Control is " + keys.Control + "\n");
        }

        private void DiagramView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == (Keys.LButton | Keys.ShiftKey))
                keys.Control = false;
            else if (e.KeyCode == Keys.ShiftKey)
                keys.Shift = false;
            Debugger.Log(0, "", "Control is " + keys.Control + "\n");
        }

        private void DiagramView_KeyPress(object sender, KeyPressEventArgs e)
        {

        }


        private void ShortenLine(RectangleF bounds1, ref PointF s1, ref PointF s2, ShapeTypeEnum shape)
        {
            PointF news = new PointF();
            SizeF vector = new SizeF(s1.X - s2.X, s1.Y - s2.Y);

            float pico_y_ratio = 1 / (bounds1.Top - s1.Y);
            float pico_x_ratio = 1 / (bounds1.Right - s1.X);
            float ratio = 1.0f;
            float incr = 0.5f;
            for (int i = 0; i < 10; i++)
            {
                news.X = s2.X + vector.Width * ratio;
                news.Y = s2.Y + vector.Height * ratio;
                if (shape == ShapeTypeEnum.Rectangle)
                {
                    if (bounds1.Contains(news))
                        ratio -= incr;
                    else
                        ratio += incr;
                }
                else if (shape == ShapeTypeEnum.Ellipse)
                {
                    float sxx = news.X - s1.X;
                    float syy = news.Y - s1.Y;
                    float dist = sxx * sxx * pico_x_ratio + syy * syy * pico_y_ratio;
                    if (dist > 1.0)
                        ratio += incr;
                    else
                        ratio -= incr;
                }
                else if (shape == ShapeTypeEnum.Pico)
                {
                    float dist = (news.X - s1.X) * pico_x_ratio + (news.Y - s1.Y) * pico_y_ratio;
                    if (dist > 1.0)
                        ratio += incr;
                    else
                        ratio -= incr;
                }
                incr = incr / 2;
            }

            s1 = news;
        }

        /*private void Intersect(Item dvi1, Item dvi2, out PointF pt1, out PointF pt2)
        {
            RectangleF bounds1 = dvi1.Bounds;
            RectangleF bounds2 = dvi2.Bounds;

            PointF s1 = new PointF(bounds1.X + bounds1.Width / 2, bounds1.Y + bounds1.Height / 2);
            PointF s2 = new PointF(bounds2.X + bounds2.Width / 2, bounds2.Y + bounds2.Height / 2);

            ShortenLine(bounds1, ref s1, ref s2, dvi1.CurrentShapeType);
            ShortenLine(bounds2, ref s2, ref s1, dvi2.CurrentShapeType);

            pt1 = s1;
            pt2 = s2;
        }*/

        /// <summary>
        /// Drawing diagram.
        /// Contains these steps:
        /// - foreach item:
        ///      - draw item in one of the states:
        ///          - selected
        ///          - tracked
        ///          - normal
        /// </summary>
        /// <param name="g"></param>
        private void DrawToBuffer(Graphics g)
        {
            SizeF size = Size;

            g.FillRectangle(SystemBrushes.Control, 0, 0, this.Width, this.Height);

            g.TranslateTransform(size.Width / 2 + p_centerOffset.X, size.Height / 2 + p_centerOffset.Y);
            g.ScaleTransform(p_scale_array[p_scale_index], p_scale_array[p_scale_index]);

            if (p_mouse_state == MouseState.ConnectionCreate && p_connStartItem != null)
            {
                g.DrawLine(Pens.Blue, p_connStartLog, p_connEndLog);
            }

            /*for (int i = 0; i < Conns.Count; i++)
            {
                DiagramViewConnection dnc = Conns[i];
                if (!dnc.PointsValid)
                {
                    Item src = Items.FindItemWithId(dnc.SourceId);
                    Item dest = Items.FindItemWithId(dnc.DestinationId);
                    if (src != null && dest != null)
                    {
                        Intersect(src, dest, out dnc.SourcePoint, out dnc.DestinationPoint);
                        dnc.PointsValid = true;
                    }
                }

                g.DrawLine(Pens.Black, dnc.SourcePoint, dnc.DestinationPoint);
            }*/

            Item[] trackers = new Item[] { p_connEndItem, p_connStartItem };

            for (int i = 0; i < Items.Count; i++)
            {
                Item dvi = Items[i];
                if (p_connStartItem == dvi || p_connEndItem == dvi || dvi == p_highlightedTemporary)
                    dvi.Paint(g, HighlightType.Tracked);
                else if (dvi.Selected)
                    dvi.Paint(g, HighlightType.Selected);
                else
                    dvi.Paint(g, HighlightType.Normal);
            }
        }

        private void DiagramView_Paint(object sender, PaintEventArgs e)
        {
            //grafx.Render(e.Graphics);
            DrawToBuffer(e.Graphics);
        }

        public PointF LogicalCoordinatesFromClientPoint(float X, float Y)
        {
            SizeF size = Size;
            return new PointF((X - size.Width/2 - p_centerOffset.X)/ScaleRatio, (Y - size.Height/2 - p_centerOffset.Y)/ScaleRatio);
        }

        public int GetUniqueId()
        {
            p_uniqueId++;
            return p_uniqueId;
        }

        public void OnDiagramItemsCollectionChanged()
        {
            RedrawClientScreen();
        }

        public void RemoveConnectionWithItem(int itemId)
        {
            if (Conns != null)
            {
                Conns.RemoveWithItemId(itemId);
            }
        }

        public void RedrawClientScreen()
        {
            //DrawToBuffer(grafx.Graphics);
            Invalidate();
            //Refresh();
        }

        public void CancelMouseOperation()
        {
            Capture = false;
            p_mouse_state = MouseState.None;
        }

        private void DiagramView_Leave(object sender, EventArgs e)
        {
            CancelMouseOperation();
        }

        private void DiagramView_SizeChanged(object sender, EventArgs e)
        {
            //Invalidate();
        }

        private void OnResize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void OnSelectionChanged()
        {
            if (SelectedItemChanged != null)
                SelectedItemChanged();
        }
    }
}
