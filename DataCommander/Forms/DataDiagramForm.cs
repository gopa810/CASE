using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CASE.Model;
using CASE.Controls;
using CASE.Dialogs;

using GPFlowSequenceDiagram;
using GPFlowSequenceDiagramView;

namespace CASE.Forms
{
    public partial class DataDiagramForm : Form, IDiagramViewDataSource
    {
        public Project project = null;

        public DataDiagramForm()
        {
            InitializeComponent();
            diagramView1.DataSource = this;
        }

        private static Dictionary<Project,DataDiagramForm> shared_forms = new Dictionary<Project,DataDiagramForm>();

        /// <summary>
        /// Allowed is only one Diagram Form for one project, because this form shows
        /// global diagram hierarchy.
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        public static DataDiagramForm SharedForm(Project proj)
        {
            if (!shared_forms.ContainsKey(proj))
            {
                DataDiagramForm newForm = new DataDiagramForm();
                newForm.project = proj;
                shared_forms.Add(proj, newForm);
            }
            return shared_forms[proj];
        }

        /// <summary>
        /// Provide context menu based on given diagram context
        /// </summary>
        /// <param name="ctx">Diagram context</param>
        /// <returns>Context menu</returns>
        public ContextMenuStrip GetContextMenuStrip(DiagramViewContext ctx)
        {
            return contextMenuStrip1;
        }

        /// <summary>
        /// Provide diagram context stored in Tag property of ToolStripItem
        /// </summary>
        /// <param name="sender">ToolStripItem object</param>
        /// <returns>diagram context object</returns>
        private DiagramViewContext ContextFromMenuItem(object sender)
        {
            if (sender is ToolStripItem)
            {
                ToolStripItem tsi = sender as ToolStripItem;
                if (tsi.Tag != null && tsi.Tag is DiagramViewContext)
                {
                    return tsi.Tag as DiagramViewContext;
                }
            }
            return null;
        }

        private void insertProcessMenuItem_Click(object sender, EventArgs e)
        {
            DiagramViewContext dvc = ContextFromMenuItem(sender);
            if (dvc == null)
                return;

            EditProcessProperties dlg = new EditProcessProperties();
            dlg.Project = project;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DiagramItemProcess dvi = new DiagramItemProcess();
                PointF centerItem = diagramView1.LogicalCoordinatesFromClientPoint(dvc.ClientLocation.X, dvc.ClientLocation.Y);

                dvi.Delegate = diagramView1;
                dvi.ProcessName = dlg.ProcessName;
                dvi.OriginPoint.Point = centerItem;
                dvi.Size = new SizeF(100, 35);
                dvi.ItemPartDidChanged(dvi.OriginPoint);
                dvi.Text = dlg.ProcessName;
                dvi.Subtext = dlg.ProcessReturnDataType;

                diagramView1.Items.Add(dvi);
            }
        }

        private void insertWebserviceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Notification after window form is closed.
        /// Remove assignment from shared forms dictionary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataDiagramForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            shared_forms.Remove(project);
        }

        private void diagramView1_SelectedItemChanged()
        {
        }

        private void insertIfconditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiagramViewContext dvc = ContextFromMenuItem(sender);
            if (dvc == null)
                return;

            DiagramItemIfCondition dvi = new DiagramItemIfCondition();
            PointF centerItem = diagramView1.LogicalCoordinatesFromClientPoint(dvc.ClientLocation.X, dvc.ClientLocation.Y);

            dvi.Delegate = diagramView1;
            dvi.OriginPoint.Point = centerItem;
            dvi.ItemPartDidChanged(dvi.OriginPoint);

            diagramView1.Items.Add(dvi);
        }

        private void insertStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiagramViewContext dvc = ContextFromMenuItem(sender);
            if (dvc == null)
                return;

            DiagramItemStart dvi = new DiagramItemStart();
            PointF centerItem = diagramView1.LogicalCoordinatesFromClientPoint(dvc.ClientLocation.X, dvc.ClientLocation.Y);

            dvi.Delegate = diagramView1;
            dvi.OriginPoint.Point = centerItem;
            dvi.ItemPartDidChanged(dvi.OriginPoint);

            diagramView1.Items.Add(dvi);
        }

        private void insertEndToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertNewItem(sender, new DiagramItemReturn());
/*            DiagramViewContext dvc = ContextFromMenuItem(sender);
            if (dvc == null)
                return;

            DiagramItemReturn dvi = new DiagramItemReturn();
            PointF centerItem = diagramView1.LogicalCoordinatesFromClientPoint(dvc.ClientLocation.X, dvc.ClientLocation.Y);

            dvi.Delegate = diagramView1;
            dvi.OriginPoint.Point = centerItem;
            dvi.ItemPartDidChanged(dvi.OriginPoint);

            diagramView1.Items.Add(dvi);*/
        }

        private void insertWhileLoopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertNewItem(sender, new DiagramItemWhileLoop());
        }

        private void InsertNewItem(object sender, Item newItem)
        {
            DiagramViewContext dvc = ContextFromMenuItem(sender);
            if (dvc == null)
                return;

            PointF centerItem = diagramView1.LogicalCoordinatesFromClientPoint(dvc.ClientLocation.X, dvc.ClientLocation.Y);

            newItem.Delegate = diagramView1;
            newItem.OriginPoint.Point = centerItem;
            newItem.OriginPoint.ItemPartDidChanged();

            diagramView1.Items.Add(newItem);
        }

        private void insertBreakforLoopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertNewItem(sender, new DiagramItemLoopBreak());
        }

        private void insertFORLoopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertNewItem(sender, new DiagramItemForLoop());
        }

        private void insertContinueforLoopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertNewItem(sender, new DiagramItemLoopContinue());
        }

        private void insertForeachLoopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertNewItem(sender, new DiagramItemForeach());
        }

        private void insertSwitchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertNewItem(sender, new DiagramItemSwitch());
        }
    }
}
