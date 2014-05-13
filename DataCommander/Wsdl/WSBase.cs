using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCommander.Wsdl
{
    public class WSBase
    {
        private string baseName = string.Empty;
        private string baseDocumentation = string.Empty;
        
        public string Name
        {
            get { return baseName; }
            set { baseName = value; }
        }

        public string Documentation
        {
            get { return baseDocumentation; }
            set { baseDocumentation = value; }
        }

    }
}
