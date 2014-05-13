using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSNamedText: XSBase
    {
        //public string Name = string.Empty;
        public string Text = string.Empty;

        public XSNamedText()
        {
        }

        public XSNamedText(XmlElement elem)
        {
            LoadXml(elem);
        }

        public void LoadXml(XmlElement elem)
        {
            Name = elem.Name;
            Text = elem.InnerText;
        }
    }
}
