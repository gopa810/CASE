using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CASE.Forms
{
    public partial class StateDiagramForm : Form
    {
        public StateDiagramForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stateDiagramView1.AddNewStateItem();
            stateDiagramView1.Invalidate();
            stateDiagramView1.Update();
        }
    }
}
