using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CASE.Dialogs
{
    public partial class NewFile : Form
    {
        public NewFile()
        {
            InitializeComponent();
        }

        public string ProjectName
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
                textBox1.SelectAll();
            }
        }
    }
}
