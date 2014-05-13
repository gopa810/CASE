namespace CASE.Forms
{
    partial class DataDiagramForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.diagramView1 = new GPFlowSequenceDiagramView.View();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.insertTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertWebserviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertTerminatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertIfconditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertEndToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.insertWhileLoopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertForeachLoopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertFORLoopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertBreakforLoopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertContinueforLoopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertSwitchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(194, 343);
            this.propertyGrid1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.diagramView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer1.Size = new System.Drawing.Size(643, 343);
            this.splitContainer1.SplitterDistance = 445;
            this.splitContainer1.TabIndex = 2;
            // 
            // diagramView1
            // 
            this.diagramView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramView1.Location = new System.Drawing.Point(0, 0);
            this.diagramView1.Name = "diagramView1";
            this.diagramView1.Size = new System.Drawing.Size(445, 343);
            this.diagramView1.TabIndex = 0;
            this.diagramView1.SelectedItemChanged += new GPFlowSequenceDiagramView.View.MyDelegate(this.diagramView1_SelectedItemChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertTableToolStripMenuItem,
            this.insertWebserviceToolStripMenuItem,
            this.insertTerminatorToolStripMenuItem,
            this.insertIfconditionToolStripMenuItem,
            this.insertStartToolStripMenuItem,
            this.insertEndToolStripMenuItem,
            this.toolStripMenuItem1,
            this.insertWhileLoopToolStripMenuItem,
            this.insertForeachLoopToolStripMenuItem,
            this.insertFORLoopToolStripMenuItem,
            this.insertBreakforLoopToolStripMenuItem,
            this.insertContinueforLoopToolStripMenuItem,
            this.insertSwitchToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(209, 296);
            // 
            // insertTableToolStripMenuItem
            // 
            this.insertTableToolStripMenuItem.Name = "insertTableToolStripMenuItem";
            this.insertTableToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.insertTableToolStripMenuItem.Text = "Insert Process";
            this.insertTableToolStripMenuItem.Click += new System.EventHandler(this.insertProcessMenuItem_Click);
            // 
            // insertWebserviceToolStripMenuItem
            // 
            this.insertWebserviceToolStripMenuItem.Name = "insertWebserviceToolStripMenuItem";
            this.insertWebserviceToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.insertWebserviceToolStripMenuItem.Text = "Insert Data Storage";
            this.insertWebserviceToolStripMenuItem.Click += new System.EventHandler(this.insertWebserviceToolStripMenuItem_Click);
            // 
            // insertTerminatorToolStripMenuItem
            // 
            this.insertTerminatorToolStripMenuItem.Name = "insertTerminatorToolStripMenuItem";
            this.insertTerminatorToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.insertTerminatorToolStripMenuItem.Text = "Insert Terminator";
            // 
            // insertIfconditionToolStripMenuItem
            // 
            this.insertIfconditionToolStripMenuItem.Name = "insertIfconditionToolStripMenuItem";
            this.insertIfconditionToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.insertIfconditionToolStripMenuItem.Text = "Insert If (condition)";
            this.insertIfconditionToolStripMenuItem.Click += new System.EventHandler(this.insertIfconditionToolStripMenuItem_Click);
            // 
            // insertStartToolStripMenuItem
            // 
            this.insertStartToolStripMenuItem.Name = "insertStartToolStripMenuItem";
            this.insertStartToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.insertStartToolStripMenuItem.Text = "Insert Start";
            this.insertStartToolStripMenuItem.Click += new System.EventHandler(this.insertStartToolStripMenuItem_Click);
            // 
            // insertEndToolStripMenuItem
            // 
            this.insertEndToolStripMenuItem.Name = "insertEndToolStripMenuItem";
            this.insertEndToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.insertEndToolStripMenuItem.Text = "Insert End";
            this.insertEndToolStripMenuItem.Click += new System.EventHandler(this.insertEndToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(205, 6);
            // 
            // insertWhileLoopToolStripMenuItem
            // 
            this.insertWhileLoopToolStripMenuItem.Name = "insertWhileLoopToolStripMenuItem";
            this.insertWhileLoopToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.insertWhileLoopToolStripMenuItem.Text = "Insert WHILE loop";
            this.insertWhileLoopToolStripMenuItem.Click += new System.EventHandler(this.insertWhileLoopToolStripMenuItem_Click);
            // 
            // insertForeachLoopToolStripMenuItem
            // 
            this.insertForeachLoopToolStripMenuItem.Name = "insertForeachLoopToolStripMenuItem";
            this.insertForeachLoopToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.insertForeachLoopToolStripMenuItem.Text = "Insert FOREACH loop";
            this.insertForeachLoopToolStripMenuItem.Click += new System.EventHandler(this.insertForeachLoopToolStripMenuItem_Click);
            // 
            // insertFORLoopToolStripMenuItem
            // 
            this.insertFORLoopToolStripMenuItem.Name = "insertFORLoopToolStripMenuItem";
            this.insertFORLoopToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.insertFORLoopToolStripMenuItem.Text = "Insert FOR Loop";
            this.insertFORLoopToolStripMenuItem.Click += new System.EventHandler(this.insertFORLoopToolStripMenuItem_Click);
            // 
            // insertBreakforLoopToolStripMenuItem
            // 
            this.insertBreakforLoopToolStripMenuItem.Name = "insertBreakforLoopToolStripMenuItem";
            this.insertBreakforLoopToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.insertBreakforLoopToolStripMenuItem.Text = "Insert Break (for loop)";
            this.insertBreakforLoopToolStripMenuItem.Click += new System.EventHandler(this.insertBreakforLoopToolStripMenuItem_Click);
            // 
            // insertContinueforLoopToolStripMenuItem
            // 
            this.insertContinueforLoopToolStripMenuItem.Name = "insertContinueforLoopToolStripMenuItem";
            this.insertContinueforLoopToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.insertContinueforLoopToolStripMenuItem.Text = "Insert Continue (for loop)";
            this.insertContinueforLoopToolStripMenuItem.Click += new System.EventHandler(this.insertContinueforLoopToolStripMenuItem_Click);
            // 
            // insertSwitchToolStripMenuItem
            // 
            this.insertSwitchToolStripMenuItem.Name = "insertSwitchToolStripMenuItem";
            this.insertSwitchToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.insertSwitchToolStripMenuItem.Text = "Insert Switch";
            this.insertSwitchToolStripMenuItem.Click += new System.EventHandler(this.insertSwitchToolStripMenuItem_Click);
            // 
            // DataDiagramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 367);
            this.Controls.Add(this.splitContainer1);
            this.Name = "DataDiagramForm";
            this.Text = "Data Diagram";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DataDiagramForm_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GPFlowSequenceDiagramView.View diagramView1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem insertTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertWebserviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertTerminatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertIfconditionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertStartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertEndToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertWhileLoopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertBreakforLoopToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem insertFORLoopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertContinueforLoopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertForeachLoopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertSwitchToolStripMenuItem;
    }
}