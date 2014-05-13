namespace GPFlowSequenceDiagramView
{
    partial class View
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DiagramView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "DiagramView";
            this.Size = new System.Drawing.Size(437, 311);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.DiagramView_MouseWheel);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DiagramView_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DiagramView_MouseMove);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DiagramView_MouseDoubleClick);
            this.Leave += new System.EventHandler(this.DiagramView_Leave);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DiagramView_KeyUp);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DiagramView_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DiagramView_MouseDown);
            this.Resize += new System.EventHandler(this.OnResize);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DiagramView_KeyPress);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DiagramView_MouseUp);
            this.SizeChanged += new System.EventHandler(this.DiagramView_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DiagramView_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
