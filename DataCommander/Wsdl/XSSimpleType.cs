using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSSimpleType: XSBase
    {
        public string ID = string.Empty;
        public XSAnnotation annotation = null;
        public XSBase content = null;

        public XSSimpleType()
        {
        }
        public XSSimpleType(XmlElement elem)
        {
            LoadXml(elem);
        }

        public XSSimpleType(XmlElement elem, string s)
        {
            LoadXml(elem);
            Namespace = s;
        }

        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id"))
                ID = elem.GetAttribute("id");
            if (elem.HasAttribute("name"))
                Name = elem.GetAttribute("name");
            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "annotation")
                {
                    annotation = new XSAnnotation(node as XmlElement);
                }
                else if (node.LocalName == "restriction")
                {
                    content = new XSRestriction(node as XmlElement);
                }
                else if (node.LocalName == "list")
                {
                    content = new XSList(node as XmlElement);
                }
                else if (node.LocalName == "union")
                {
                    content = new XSUnion(node as XmlElement);
                }
            }
        }
    }
}
