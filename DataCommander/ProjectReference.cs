using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CASE.Model
{
    public class ProjectReference
    {
        protected bool modified = false;
        protected string fileName = string.Empty;
        protected string projectName = string.Empty;

        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                modified = true;
            }
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                modified = true;
            }
        }

        public bool Modified
        {
            get { return modified; }
            set { modified = value; }
        }
    }
}
