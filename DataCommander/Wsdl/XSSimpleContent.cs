using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSSimpleContent: XSContentBase
    {
        public string Id = string.Empty;

        public XSAnnotation annotation = null;
        public object Content = null;

        public XSSimpleContent()
        {
        }
        public XSSimpleContent(XmlElement elem)
        {
            LoadXml(elem);
        }
        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id")) Id = elem.GetAttribute("id");

            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "annotation")
                    annotation = new XSAnnotation(node as XmlElement);
                else if (node.LocalName == "restriction")
                    Content = new XSElement(node as XmlElement);
                else if (node.LocalName == "extension")
                    Content = new XSGroup(node as XmlElement);
            }
        }
    }
}
