﻿namespace CASE.Controls
{
    partial class StateDiagramView
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
            // StateDiagramView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "StateDiagramView";
            this.Size = new System.Drawing.Size(235, 268);
            this.MouseLeave += new System.EventHandler(this.StateDiagramView_MouseLeave);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.StateDiagramView_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.StateDiagramView_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StateDiagramView_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.StateDiagramView_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
