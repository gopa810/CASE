namespace CASE.Forms
{
    partial class StateDiagramForm
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
            this.stateDiagramView1 = new CASE.Controls.StateDiagramView();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // stateDiagramView1
            // 
            this.stateDiagramView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.stateDiagramView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stateDiagramView1.Location = new System.Drawing.Point(12, 12);
            this.stateDiagramView1.Name = "stateDiagramView1";
            this.stateDiagramView1.Size = new System.Drawing.Size(515, 370);
            this.stateDiagramView1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(533, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 34);
            this.button1.TabIndex = 1;
            this.button1.Text = "New State";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // StateDiagramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 394);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.stateDiagramView1);
            this.Name = "StateDiagramForm";
            this.Text = "StateDiagramForm";
            this.ResumeLayout(false);

        }

        #endregion

        private CASE.Controls.StateDiagramView stateDiagramView1;
        private System.Windows.Forms.Button button1;
    }
}