using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using GPFlowSequenceDiagram.DataFlow;

namespace GPFlowSequenceDiagram
{
    public class DiagramPageDataflow: DiagramPage
    {
        public DiagramPageDataflow(DiagramElement parent)
            : base(parent)
        {
        }

        public override void DE_PopulateContextMenu(System.Windows.Forms.ContextMenuStrip cms, DiagramContext context)
        {
            ToolStripMenuItem tsi;

            tsi = new ToolStripMenuItem("Add Process", null, new EventHandler(menuItem_Click));
            tsi.Name = "new process";
            tsi.Tag = context;
            cms.Items.Add(tsi);


            tsi = new ToolStripMenuItem("Add Data Source", null, new EventHandler(menuItem_Click));
            tsi.Name = "new data source";
            tsi.Tag = context;
            cms.Items.Add(tsi);

        }

        void menuItem_Click(object sender, EventArgs args)
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            DiagramContext context = tsi.Tag as DiagramContext;
            context.PagePoint = ClientToPagePoint(context.ClientLocation.X, context.ClientLocation.Y);

            if (tsi.Name == "new process")
            {
                DFDIProcess process = new DFDIProcess(this);
                process.Name.Text = DE_GetUniqueEntityName("process");
                DE_InsertNewItem(process, context.PagePoint);
            }
            else if (tsi.Name == "new data source")
            {
                DE_InsertNewItem(new CFDIExpression(this), context.PagePoint);
            }

        }

        public override SizeF DE_DrawShape(DiagramDrawingContext ctx, HighlightType highType)
        {
            Graphics g = ctx.Graphics;

            g.DrawLine(Pens.Gray, 0, 1000, 0, -1000);
            g.DrawLine(Pens.Gray, 1000, 0, -1000, 0);

            //TransformMatrices = ctx.LastTransform;

            UsedRectangle = RectangleF.Empty;

            for (int i = 0; i < Items.Count; i++)
            {
                DiagramItem dvi = Items[i];
                if (highType == HighlightType.NotDraw)
                {
                    dvi.DE_DrawShape(ctx, highType);
                }
                else
                {
                    if (ctx != null && ctx.IsHighlighted(dvi))
                        dvi.DE_DrawShape(ctx, HighlightType.Tracked);
                    else if (dvi.Selected)
                        dvi.DE_DrawShape(ctx, HighlightType.Selected);
                    else
                        dvi.DE_DrawShape(ctx, HighlightType.Normal);
                }

                UsedRectangle = DiagramElement.MergeRectangles(UsedRectangle, dvi.UsedRectangle);
            }

            UsedRectangle.X -= 32;
            UsedRectangle.Y -= 24;
            UsedRectangle.Width += 64;
            UsedRectangle.Height += 48;

            return UsedRectangle.Size;
        }

    }
}
