using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSAttributeGroup: XSAttributedContentBase
    {
        public string Ref = string.Empty;

        public XSAnnotation annotation = null;
        public XSAnyAttribute anyAttribute = null;

        public XSAttributeGroup()
        {
        }
        public XSAttributeGroup(XmlElement elem)
        {
            LoadXml(elem);
        }
        public XSAttributeGroup(XmlElement elem, string s)
        {
            LoadXml(elem);
            Namespace = s;
        }
        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id")) Id = elem.GetAttribute("id");
            if (elem.HasAttribute("name")) Name = elem.GetAttribute("name");
            if (elem.HasAttribute("ref")) Ref = elem.GetAttribute("ref");

            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "annotation")
                    annotation = new XSAnnotation(node as XmlElement);
                else if (node.LocalName == "attribute")
                    attributes.Add(new XSAttribute(node as XmlElement));
                else if (node.LocalName == "attributeGroup")
                    attributes.Add(new XSAttributeGroup(node as XmlElement));
                else if (node.LocalName == "anyAttribute")
                    anyAttribute = new XSAnyAttribute(node as XmlElement);
            }
        }

    }
}
