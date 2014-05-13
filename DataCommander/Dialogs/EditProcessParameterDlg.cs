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
    public partial class EditProcessParameterDlg : Form
    {
        public EditProcessParameterDlg()
        {
            InitializeComponent();
        }

        public object SelectedObject
        {
            get { return propertyGrid1.SelectedObject; }
            set { propertyGrid1.SelectedObject = value; }
        }
    }
}
