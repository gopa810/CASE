using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GPFlowSequenceDiagram;

namespace GPFlowSequenceDiagramView
{
    public partial class View : UserControl
    {
        private static Graphics measureGraphics = null;

        public delegate void MyDelegate();

        public event MyDelegate SelectedItemChanged;

        private float[] p_scale_array = new float[] { 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.85f, 1.0f, 1.5f, 2.0f, 2.5f, 3.0f, 4.0f };
        private int p_scale_index = 7;

        private DiagramItemPart mouseTrackedItem = null;
        private DiagramItemPart mouseTrackedStartItem = null;
        private DiagramPoint mouseTrackedStartPosition = new DiagramPoint();
        private DiagramPoint mouseTrackedStartItemOrig = new DiagramPoint();
        private DiagramContext mouseMoveContext = new DiagramContext();
        private DiagramPage mouseTrackedItemParentPage = null;
        private PointF mouseTrackedItemPageCenterClient = PointF.Empty;

        private PointF p_startingPhysPoint = new PointF();
        private DiagramItem p_highlightedTemporary = null;

        private PointF p_centerOffset = new PointF(0, 0);
        private MouseState p_mouse_state = MouseState.None;
        private PointF p_prevPos = new PointF();

        private DiagramMouseKeys keys = new DiagramMouseKeys();

        private DiagramItem p_connStartItem = null;
        private DiagramItem p_connEndItem = null;
        private PointF p_connStartLog = new PointF();
        private PointF p_connEndLog = new PointF();

        public DiagramPageProcedural currentPage = null;
        public DiagramConnectionCollection Conns = null;

        public bool policyMoveOriginDisconnects = true;

        public DiagramViewProxy proxy;

        public View()
        {
            proxy = new DiagramViewProxy(null, this);
            currentPage = new DiagramPageProcedural(proxy);
            Conns = new DiagramConnectionCollection(proxy);
            InitializeComponent();

            if (measureGraphics != null)
                measureGraphics = this.CreateGraphics();
        }

        public void SetProxyParent(DiagramElement parent)
        {
            if (proxy != null)
                proxy.Parent = parent;
        }

        public float ScaleRatio
        {
            get
            {
                return p_scale_array[p_scale_index];
            }
        }

        public static Graphics SafeGraphics
        {
            get
            {
                return measureGraphics;
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

        /// <summary>
        /// Gets mouse coordinates in all coordinate systems and gets objects under mouse pointer.
        /// </summary>
        /// <param name="X">An X value of client coordinates</param>
        /// <param name="Y">An Y value of client coordinates</param>
        /// <returns>Creates new object</returns>
        private DiagramContext GetCurrentMouseContext(int X, int Y, int elementType)
        {
            // initialize
            mouseMoveContext.ClientLocation = new Point(X, Y);
            mouseMoveContext.ScreenLocation = PointToScreen(mouseMoveContext.ClientLocation);
            mouseMoveContext.FoundElement = null;

            // find object
            mouseMoveContext.InitSearchOfCounterpartFor(elementType);
            currentPage.DE_FindElements(mouseMoveContext);
            if (mouseMoveContext.FoundElement != null)
            {
                DiagramElement de = mouseMoveContext.FoundElement.DE_FindPredecessorOfType(DiagramElement.ET_GENERAL_PAGE);
                if (de is DiagramPage)
                    mouseMoveContext.Page = de as DiagramPage;
                else
                    mouseMoveContext.Page = currentPage;
            }
            else
            {
                mouseMoveContext.FoundElement = currentPage;
                mouseMoveContext.Page = currentPage;
            }

            // return value
            return mouseMoveContext;
        }

        private void DiagramView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (currentPage != null)
                {
                    DiagramContext dvc = GetCurrentMouseContext(e.X, e.Y, DiagramElement.ET_NONE);
                    ContextMenuStrip cms = new ContextMenuStrip();
                    if (dvc.FoundElement == null)
                    {
                        currentPage.DE_PopulateContextMenu(cms, dvc);
                    }
                    else
                    {
                        dvc.FoundElement.DE_PopulateContextMenu(cms, dvc);
                    }
                    //ContextMenuStrip cms = DataSource.GetContextMenuStrip(dvc);
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
            if (mouseTrackedItem != null)
            {
                MouseMoveWithTrackedItem(e);
            }
            else
            {
                MouseMoveWithoutTracking(e);
            }
        }

        /// <summary>
        /// Moving mouse pointer without having any item tracked
        /// </summary>
        private void MouseMoveWithoutTracking(MouseEventArgs e)
        {
            GetCurrentMouseContext(e.X, e.Y, DiagramElement.ET_NONE);

            if (mouseMoveContext != null && mouseMoveContext.FoundElement != null)
            {
                Cursor proposedCursor = mouseMoveContext.FoundElement.DE_GetCursor(keys.KeyCode);
                if (proposedCursor != null)
                    Cursor.Current = proposedCursor;
            }
        }

        /// <summary>
        /// Moving mouse point with tracked item in diagram
        /// </summary>
        private void MouseMoveWithTrackedItem(MouseEventArgs e)
        {
            //GetCurrentMouseContext(e.X, e.Y, mouseTrackedItem.ElementType);
            DiagramPoint ptCurrent = mouseMoveContext.Page.ClientToPagePoint(e.X, e.Y);
            mouseMoveContext.PagePoint = ptCurrent;

            if (mouseTrackedItem.Item != null)
            {
                DiagramPoint ptNewOrigin = new DiagramPoint();

                ptNewOrigin.X = ptCurrent.X - mouseTrackedStartPosition.X + mouseTrackedStartItemOrig.X;
                ptNewOrigin.Y = ptCurrent.Y - mouseTrackedStartPosition.Y + mouseTrackedStartItemOrig.Y;

                proxy.Context.Graphics = GetGraphics();
                mouseTrackedItem.Item.SetOriginPoint(proxy.Context, ptNewOrigin.X, ptNewOrigin.Y);

                if (mouseTrackedItem is ItemPartInput)
                {
                    ItemPartInput ipi = mouseTrackedItem as ItemPartInput;
                    if (policyMoveOriginDisconnects)
                    {
                        ItemPartOutput ipo = ipi.RefItem;

                        if (ipi.RefItem != null)
                            ipi.RefItem.RefItem = null;
                        ipi.RefItem = null;

                        if (ipo != null)
                        {
                            DiagramPoint positionBeforeRelayout = ipo.Point;
                            // in the loop we update origin points to all previous nodes
                            DiagramItem headItem = ipo.GetHeadItem();
                            if (headItem != null)
                            {
                                proxy.Context.Graphics = GetGraphics();
                                headItem.RelayoutNextShapes(proxy.Context);
                            }

                            p_centerOffset.X -= (ipo.X - positionBeforeRelayout.X);
                            p_centerOffset.Y -= (ipo.Y - positionBeforeRelayout.Y);
                        }
                    }
                    else if (ipi.RefItem != null && ipi.RefItem is ItemPartOutput)
                    {
                        (ipi.RefItem as ItemPartOutput).Point = ipi.Point;
                    }
                }
                DiagramItemPart connPart = mouseMoveContext.FoundElement as DiagramItemPart;
                if (connPart != null)
                {
                    p_highlightedTemporary = connPart.Item;
                }
                else
                {
                    p_highlightedTemporary = null;
                }

                /*if (mouseTrackedItemParentPage != null
                && mouseTrackedItemParentPage != currentPage)
                {
                    RecalculatePageOriginForSubPage(mouseTrackedItemParentPage);
                }

                if (mouseTrackedItemParentPage != currentPage)
                {
                    PointF newParentPageCenter;
                    PointF oldParentPageCenter;
                    oldParentPageCenter = mouseTrackedItemParentPage.PageToClientPoint(new Point(0, 0));
                    Debugger.Log(0, "", string.Format("Old parent page {0},{1}\n", oldParentPageCenter.X, oldParentPageCenter.Y));

                    RedrawForRecalculation();

                    newParentPageCenter = mouseTrackedItemParentPage.PageToClientPoint(new Point(0, 0));
                    Debugger.Log(0, "", string.Format("New parent page {0},{1}\n", newParentPageCenter.X, newParentPageCenter.Y));
                    Debugger.Log(0, "", string.Format("Change last pag {0},{1}\n", newParentPageCenter.X - mouseTrackedItemPageCenterClient.X,
                        newParentPageCenter.Y - mouseTrackedItemPageCenterClient.Y));
                    Debugger.Log(0, "", string.Format("Change last NEW {0},{1}\n", newParentPageCenter.X - oldParentPageCenter.X,
                        newParentPageCenter.Y - oldParentPageCenter.Y));
                    //Debugger.Log(0, "", string.Format("> Chan lastitem {0},{1}\n", nextTracki.X - prevTracki.X, nextTracki.Y - prevTracki.Y));
                    //p_centerOffset.X -= (newParentPageCenter.X - mouseTrackedItemPageCenterClient.X);
                    //p_centerOffset.Y -= (newParentPageCenter.Y - mouseTrackedItemPageCenterClient.Y);
                    p_centerOffset.X -= (newParentPageCenter.X - oldParentPageCenter.X);
                    p_centerOffset.Y -= (newParentPageCenter.Y - oldParentPageCenter.Y);
                    //mouseTrackedItemPageCenterClient = newParentPageCenter;
                    

                    RedrawClientScreen();
                    
                    newParentPageCenter = mouseTrackedItemParentPage.PageToClientPoint(new Point(0, 0));
                    Debugger.Log(0, "", string.Format("--> New parent page center in client coords: {0},{1}\n", newParentPageCenter.X, newParentPageCenter.Y));
                }
                else*/
                {
                    RedrawClientScreen();
                }
            }
        }

        private void DiagramView_MouseDown(object sender, MouseEventArgs e)
        {
            Capture = true;
            if (e.Button == MouseButtons.Left)
            {
                p_prevPos.X = e.X;
                p_prevPos.Y = e.Y;

                DiagramContext ctx = GetCurrentMouseContext(e.X, e.Y, DiagramElement.ET_NONE);
                mouseTrackedItem = DiagramItemPart.FirstItemPart(ctx.FoundElement);
                mouseTrackedItemParentPage = ctx.Page;
                mouseTrackedItemPageCenterClient = ctx.Page.PageToClientPoint(new PointF(0, 0));
                mouseTrackedStartItem = ((mouseTrackedItem == null) ? null : mouseTrackedItem.Copy());
                mouseTrackedStartPosition.X = ctx.PagePoint.X;
                mouseTrackedStartPosition.Y = ctx.PagePoint.Y;
                p_startingPhysPoint.X = e.X;
                p_startingPhysPoint.Y = e.Y;

                DiagramItemPart firstPart = ItemPartInput.FirstItemPartInput(ctx.FoundElement);
                if (firstPart == null)
                    firstPart = DiagramItemPart.FirstItemPart(ctx.FoundElement);
                if (!keys.Control)
                {
                    if (!keys.Shift)
                        this.currentPage.Items.ClearSelection();
                    if (firstPart != null && firstPart.Item != null)
                    {
                        firstPart.Item.Selected = true;
                        proxy.DE_DidSelectObject(firstPart.Item);
                    }
                    else
                    {
                        proxy.DE_DidSelectObject(currentPage);
                    }
                }
                if (firstPart != null)
                {
                    if (firstPart.Item != null)
                    {
                        mouseTrackedStartItemOrig.X = firstPart.Item.OriginPoint.X;
                        mouseTrackedStartItemOrig.Y = firstPart.Item.OriginPoint.Y;
                    }
                    Cursor cur = ctx.FoundElement.DE_GetCursor(keys.KeyCode);
                    if (cur != null)
                        Cursor.Current = cur;
                }
            }
        }

        private void DiagramView_MouseWheel(object sender, MouseEventArgs e)
        {
            int a = e.Delta / 120;

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
            if (mouseTrackedItem != null && mouseTrackedItem.Item != null)
            {
                DiagramContext ctx = GetCurrentMouseContext(e.X, e.Y, mouseTrackedItem.ElementType);
                DiagramItem trackItem = mouseTrackedItem.Item;
                trackItem.MouseUp(mouseTrackedItem);
                DiagramItemPart partConn = ctx.FoundElement as DiagramItemPart;
                if (partConn != null)
                {
                    if (mouseTrackedItem.WantsConnect == ConnectivityWanted.EndPointWanted)
                    {
                        if (partConn is ItemPartOutput && mouseTrackedItem is ItemPartInput)
                        {
                            ItemPartInput input = mouseTrackedItem as ItemPartInput;
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
                                output.RefItem = input;
                                output.MoveReferencedItemPart = true;
                                if (previousInput != null)
                                {
                                    previousInput.RefItem = lastItem;
                                    lastItem.RefItem = previousInput;
                                }

                                // we updated successors already
                                // now we need to update also predecessors
                                // position of currently selected (inserted) item
                                // may be changed, so we store its position here
                                DiagramPoint positionBeforeRelayout = input.Point;
                                // in the loop we update origin points to all previous nodes
                                DiagramItem headItem = input.GetHeadItem();
                                if (headItem != null)
                                {
                                    proxy.Context.Graphics = GetGraphics();
                                    headItem.RelayoutNextShapes(proxy.Context);
                                }

                                p_centerOffset.X -= (input.X - positionBeforeRelayout.X);
                                p_centerOffset.Y -= (input.Y - positionBeforeRelayout.Y);
                            }
                        }
                    }
                    else if (mouseTrackedItem.WantsConnect == ConnectivityWanted.StartPointWanted)
                    {
                        if (partConn is ItemPartInput && mouseTrackedItem is ItemPartOutput)
                        {
                            (mouseTrackedItem as ItemPartOutput).RefItem = (partConn as ItemPartInput);
                            (partConn as ItemPartInput).RefItem = (mouseTrackedItem as ItemPartOutput);
                            (partConn as ItemPartOutput).MoveReferencedItemPart = false;
                        }
                    }
                }
                p_highlightedTemporary = null;
                RedrawClientScreen();
            }

            mouseTrackedStartItem = null;
            mouseTrackedItem = null;
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
        private void DrawToBuffer(Graphics g, HighlightType highType)
        {
            SizeF size = Size;

            g.FillRectangle(SystemBrushes.Control, 0, 0, this.Width, this.Height);

            if (proxy.Context == null)
                proxy.Context = new DiagramDrawingContext();

            proxy.Context.Graphics = g;
            proxy.Context.ClearHighlighted();
            proxy.Context.AddHighlighted(p_connEndItem);
            proxy.Context.AddHighlighted(p_connStartItem);
            proxy.Context.AddHighlighted(p_highlightedTemporary);

            proxy.Context.SetTransformation(size.Width / 2 + p_centerOffset.X, 
                size.Height / 2 + p_centerOffset.Y, 
                p_scale_array[p_scale_index]);
            currentPage.TransformMatrices = proxy.Context.LastTransform;

            if (p_mouse_state == MouseState.ConnectionCreate && p_connStartItem != null)
            {
                g.DrawLine(Pens.Blue, p_connStartLog, p_connEndLog);
            }

            currentPage.DE_DrawShape(proxy.Context, highType);

            proxy.Context.PopTransform();

            // this logic is for redrawing the whole diagram for case
            // when we are moving some item in subpage and we need to realign
            // origin point of the main page so start of coordinate system in subpage
            // is always the same point in client area of a view
            if (mouseTrackedItemParentPage != null 
                && mouseTrackedItemParentPage != currentPage)
            {
                PointF newParentPageCenter = mouseTrackedItemParentPage.PageToClientPoint(new Point(0, 0));

                // this is main this we are doing here
                // adjust origin point, so start of coordinate system of subpage
                // is the same
                p_centerOffset.X -= (newParentPageCenter.X - mouseTrackedItemPageCenterClient.X);
                p_centerOffset.Y -= (newParentPageCenter.Y - mouseTrackedItemPageCenterClient.Y);

                // redraw again if change was greater than 1 point
                if ((Math.Abs(newParentPageCenter.X - mouseTrackedItemPageCenterClient.X) + 
                    Math.Abs(newParentPageCenter.Y - mouseTrackedItemPageCenterClient.Y)) > 1f)
                    Invalidate();
            }

        }

        private void DiagramView_Paint(object sender, PaintEventArgs e)
        {
            //grafx.Render(e.Graphics);
            DrawToBuffer(e.Graphics, HighlightType.Normal);
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



    }
}
