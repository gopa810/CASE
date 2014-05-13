using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CASE.Forms
{
    public class ABCListItemSorter: IComparer
    {
        private int col;
        public ABCListItemSorter()
        {
            col = 0;
        }
        public ABCListItemSorter(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
        }
    }
}
