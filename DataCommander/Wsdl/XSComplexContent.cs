using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSComplexContent: XSContentBase
    {
        public XSComplexContent()
        {
        }
        public XSComplexContent(XmlElement elem)
        {
            LoadXml(elem);
        }
        public void LoadXml(XmlElement elem)
        {
            throw new Exception("Not implemented");
        }
    }
}
