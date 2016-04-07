using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace GPFlowSequenceDiagram
{
    public class DiagramPageProcedural: DiagramPage
    {
        public DiagramPageProcedural(DiagramElement el)
            : base(el)
        {
            Items = new DiagramItemCollection(this);
        }

        public override void DE_PopulateContextMenu(ContextMenuStrip cms, DiagramContext context)
        {
            ToolStripMenuItem tsi;

            tsi = new ToolStripMenuItem("New Process", null, new EventHandler(menuItem_Click));
            tsi.Name = "new process";
            tsi.Tag = context;
            cms.Items.Add(tsi);


            tsi = new ToolStripMenuItem("New Expression", null, new EventHandler(menuItem_Click));
            tsi.Name = "new expression";
            tsi.Tag = context;
            cms.Items.Add(tsi);

            tsi = new ToolStripMenuItem("New IF", null, new EventHandler(menuItem_Click));
            tsi.Name = "new if";
            tsi.Tag = context;
            cms.Items.Add(tsi);

            tsi = new ToolStripMenuItem("New WHILE", null, new EventHandler(menuItem_Click));
            tsi.Name = "new while";
            tsi.Tag = context;
            cms.Items.Add(tsi);

            tsi = new ToolStripMenuItem("New FOREACH", null, new EventHandler(menuItem_Click));
            tsi.Name = "new foreach";
            tsi.Tag = context;
            cms.Items.Add(tsi);

            tsi = new ToolStripMenuItem("New BREAK", null, new EventHandler(menuItem_Click));
            tsi.Name = "new break";
            tsi.Tag = context;
            cms.Items.Add(tsi);

            tsi = new ToolStripMenuItem("New CONTINUE", null, new EventHandler(menuItem_Click));
            tsi.Name = "new continue";
            tsi.Tag = context;
            cms.Items.Add(tsi);

            tsi = new ToolStripMenuItem("New RETURN", null, new EventHandler(menuItem_Click));
            tsi.Name = "new return";
            tsi.Tag = context;
            cms.Items.Add(tsi);

            tsi = new ToolStripMenuItem("New START", null, new EventHandler(menuItem_Click));
            tsi.Name = "new start";
            tsi.Tag = context;
            cms.Items.Add(tsi);


        }

        void menuItem_Click(object sender, EventArgs args)
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            DiagramContext context = tsi.Tag as DiagramContext;
            if (tsi.Name == "new process")
            {
                CFDIProcesss process = new CFDIProcesss(this);
                process.ProcessName.Text = DE_GetUniqueEntityName("process");
                DE_InsertNewItem(process, context.PagePoint);
            }
            else if (tsi.Name == "new expression")
            {
                DE_InsertNewItem(new CFDIExpression(this), context.PagePoint);
            }
            else if (tsi.Name == "new if")
            {
                DE_InsertNewItem(new CFDIConditionIf(this), context.PagePoint);
            }
            else if (tsi.Name == "new while")
            {
                DE_InsertNewItem(new CFDILoopWhile(this), context.PagePoint);
            }
            else if (tsi.Name == "new foreach")
            {
                DE_InsertNewItem(new CFDILoopForeach(this), context.PagePoint);
            }
            else if (tsi.Name == "new break")
            {
                DE_InsertNewItem(new CFDILoopBreak(this), context.PagePoint);
            }
            else if (tsi.Name == "new continue")
            {
                DE_InsertNewItem(new CFDILoopContinue(this), context.PagePoint);
            }
            else if (tsi.Name == "new return")
            {
                DE_InsertNewItem(new CFDIReturn(this), context.PagePoint);
            }
            else if (tsi.Name == "new start")
            {
                DE_InsertNewItem(new CFDIStart(this), context.PagePoint);
            }
        }

        public override SizeF DE_DrawShape(DiagramDrawingContext ctx, HighlightType highType)
        {
            Graphics g = ctx.Graphics;

            //TransformMatrices = ctx.LastTransform;

            UsedRectangle = RectangleF.Empty;

            g.DrawLine(Pens.Gray, 0, 1000, 0, -1000);
            g.DrawLine(Pens.Gray, 1000, 0, -1000, 0);

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

            return UsedRectangle.Size;
        }
    }
}
