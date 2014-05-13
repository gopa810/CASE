using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CASE.Model;

namespace CASE.Dialogs
{
    public partial class EditProcessProperties : Form
    {
        private Project prj = null;

        public EditProcessProperties()
        {
            InitializeComponent();
        }

        public string ProcessName
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public string ProcessReturnDataType
        {
            get
            {
                return comboBox1.Text;
            }
            set
            {
                comboBox1.Text = value;
            }
        }

        public List<FunctionParameterDesc> Parameters
        {
            get
            {
                List<FunctionParameterDesc> args = new List<FunctionParameterDesc>();
                foreach (ListViewItem lvi in listView1.Items)
                {
                    args.Add(new FunctionParameterDesc(lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text));
                }
                return args;
            }
            set
            {
                listView1.Items.Clear();
                foreach (FunctionParameterDesc sp in value)
                {
                    AddParameter(sp);
                }
            }
        }

        public Project Project
        {
            get
            {
                return prj;
            }
            set
            {
                prj = value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void AddParameter(FunctionParameterDesc sp)
        {
            listView1.Items.Add(new ListViewItem(new string[] {
                        sp.DataType,
                        sp.Name,
                        sp.InOut}));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FunctionParameterDesc desc = new FunctionParameterDesc();
            desc.Name = "New";
            desc.DataType = "string";
            desc.InOutType = FunctionParameterDesc.InOutTypeEnum.In;

            EditProcessParameterDlg dlg = new EditProcessParameterDlg();
            dlg.SelectedObject = desc;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                AddParameter(desc);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Delete selected parameters?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    listView1.Items.RemoveAt(listView1.SelectedItems[0].Index);
                }
            }
        }

    }
}
