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
    public partial class DataDiagramForm : Form
    {
        public Project project = null;
        public DataDiagramFormProxy Proxy { get; set; }

        public DataDiagramForm()
        {
            InitializeComponent();
            Proxy = new DataDiagramFormProxy(this);
            diagramView1.SetProxyParent(Proxy);
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
        /// Provide diagram context stored in Tag property of ToolStripItem
        /// </summary>
        /// <param name="sender">ToolStripItem object</param>
        /// <returns>diagram context object</returns>
        private DiagramContext ContextFromMenuItem(object sender)
        {
            if (sender is ToolStripItem)
            {
                ToolStripItem tsi = sender as ToolStripItem;
                if (tsi.Tag != null && tsi.Tag is DiagramContext)
                {
                    return tsi.Tag as DiagramContext;
                }
            }
            return null;
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


        private void eventHandlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        public PropertyGrid PropertyGridView
        {
            get
            {
                return propertyGrid1;
            }
        }
    }
}
