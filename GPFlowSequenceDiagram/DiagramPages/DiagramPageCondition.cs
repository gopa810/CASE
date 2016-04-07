using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace GPFlowSequenceDiagram
{
    public class DiagramPageCondition: DiagramPage
    {
        [Browsable(false)]
        public DiagramItemCollection Items;

        public DiagramPageCondition(DiagramElement parent)
            : base(parent)
        {
        }



        public override void DE_PopulateContextMenu(ContextMenuStrip cms, DiagramContext context)
        {
            ToolStripMenuItem tsi;

            tsi = new ToolStripMenuItem("New AND", null, new EventHandler(menuItem_Click));
            tsi.Name = "new and";
            tsi.Tag = context;
            cms.Items.Add(tsi);


            tsi = new ToolStripMenuItem("New OR", null, new EventHandler(menuItem_Click));
            tsi.Name = "new or";
            tsi.Tag = context;
            cms.Items.Add(tsi);

            tsi = new ToolStripMenuItem("New XOR", null, new EventHandler(menuItem_Click));
            tsi.Name = "new if";
            tsi.Tag = context;
            cms.Items.Add(tsi);

            tsi = new ToolStripMenuItem("New Equal", null, new EventHandler(menuItem_Click));
            tsi.Name = "new while";
            tsi.Tag = context;
            cms.Items.Add(tsi);

            tsi = new ToolStripMenuItem("New Not Equal", null, new EventHandler(menuItem_Click));
            tsi.Name = "new foreach";
            tsi.Tag = context;
            cms.Items.Add(tsi);

            tsi = new ToolStripMenuItem("New Greater", null, new EventHandler(menuItem_Click));
            tsi.Name = "new break";
            tsi.Tag = context;
            cms.Items.Add(tsi);


        }

        void menuItem_Click(object sender, EventArgs args)
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            DiagramContext context = tsi.Tag as DiagramContext;
            if (tsi.Name == "new and")
            {
                CFDIProcesss process = new CFDIProcesss(this);
                process.ProcessName.Text = DE_GetUniqueEntityName("process");
                DE_InsertNewItem(process, context.PagePoint);
            }
            else if (tsi.Name == "new or")
            {
                DE_InsertNewItem(new CFDIExpression(this), context.PagePoint);
            }
        }

    }
}
