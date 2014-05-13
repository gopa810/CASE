using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Net;

using CASE.Dialogs;
using CASE.Model;
using CASE.Forms;

namespace CASE
{
    public partial class MainForm : Form
    {
        public class ProjectCollection
        {
            private List<Project> projects = new List<Project>();

            internal ProjectCollection()
            {
            }

            internal ProjectCollection(List<Project> list)
            {
                projects = list;
            }

            public Project this[int index]
            {
                get
                {
                    return projects[index];
                }
            }

            public int Count
            {
                get
                {
                    return projects.Count;
                }
            }
        }

        private List<Project> projects = new List<Project>();
        public readonly ProjectCollection Projects = new ProjectCollection();

        public MainForm()
        {
            InitializeComponent();

            InitializeData();

            Projects = new ProjectCollection(projects);

        }

        public Project GetProject(int i)
        {
            return projects[i];
        }


        /// <summary>
        /// loading projects and other stuff
        /// </summary>
        private void InitializeData()
        {
            string[] files = Directory.GetFiles(AppServant.ProjectsDirectory);

            foreach (string name in files)
            {
                Project proj = new Project();
                if (proj.LoadFile(name))
                {
                    AddProjectToUI(proj);
                }
            }

            if (projects.Count > 0)
                this.noProjectsToolStripMenuItem.Visible = false;
        }

        public void AddProjectToUI(Project proj)
        {
            projects.Add(proj);

            // menu item
            ToolStripMenuItem menuItem;

            menuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            menuItem.Name = "noProjectsToolStripMenuItem" + proj.FileName;
            menuItem.Size = new System.Drawing.Size(152, 22);
            menuItem.Tag = proj;
            menuItem.Text = proj.ProjectName;
            menuItem.Click += new System.EventHandler(this.noProjectsToolStripMenuItem_Click);
            this.projectsToolStripMenuItem.DropDownItems.Add(menuItem);
            if (this.projectsToolStripMenuItem.DropDownItems.Count > 1)
                this.noProjectsToolStripMenuItem.Visible = false;
        }

        /// <summary>
        /// new project file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFile dlg = new NewFile();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Project proj = new Project();
                proj.ProjectName = dlg.ProjectName;
                proj.FileName = AppServant.GetUniqueFileName(AppServant.ProjectsDirectory, "proj-", ".xml");
                proj.SaveFile();

                DataDiagramForm projectForm = new DataDiagramForm();
                projectForm.MdiParent = this;
                projectForm.project = proj;
                projectForm.Show();

                AddProjectToUI(proj);
            }
        }

        /// <summary>
        /// choosing project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripDropDownItem)
            {
                ToolStripDropDownItem mi = sender as ToolStripDropDownItem;
                if (mi.Tag is Project)
                {
                    DataDiagramForm projectForm = FindProjectMainForm(mi.Tag as Project);
                    if (projectForm != null)
                    {
                        projectForm.BringToFront();
                        projectForm.Activate();
                    }
                    else
                    {
                        projectForm = new DataDiagramForm();
                        projectForm.MdiParent = this;
                        projectForm.project = mi.Tag as Project;
                        projectForm.Show();
                    }
                }
            }
        }

        private DataDiagramForm FindProjectMainForm(Project p)
        {
            foreach (Form ff in MdiChildren)
            {
                if (ff is DataDiagramForm)
                {
                    DataDiagramForm pf = ff as DataDiagramForm;
                    if (pf.project != null && pf.project.FileName == p.FileName)
                    {
                        return pf as DataDiagramForm;
                    }
                }
            }
            return null;
        }

        private void projectsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem menuItem in projectsToolStripMenuItem.DropDownItems)
            {
                if (menuItem != null && menuItem.Tag is Project)
                {
                    Project p = menuItem.Tag as Project;
                    if (menuItem.Text != p.ProjectName)
                        menuItem.Text = p.ProjectName;
                    menuItem.Checked = (FindProjectMainForm(p) != null);
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Project p in projects)
            {
                if (p.Modified)
                {
                    p.SaveFile();
                }
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Create new state diagram window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StateDiagramForm form = new StateDiagramForm();
            form.MdiParent = this;
            form.Show();
        }

    }
}
