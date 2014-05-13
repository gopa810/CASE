using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSGroup:XSAttributedContentBase
    {
        public XSGroup()
        {
        }

        public XSGroup(XmlElement elem)
        {
            LoadXml(elem);
        }
        public XSGroup(XmlElement elem, string s)
        {
            LoadXml(elem);
            Namespace = s;
        }

        public void LoadXml(XmlElement elem)
        {
        }
    }
}
