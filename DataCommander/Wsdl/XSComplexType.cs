using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSComplexType: XSAttributedContentBase
    {
        public bool Abstract = false;
        public bool Mixed = false;

        public XSAnnotation annotation = null;
        public XSContentBase content = null;

        public XSComplexType()
        {
        }

        public XSComplexType(XmlElement elem)
        {
            LoadXml(elem);
        }

        public XSComplexType(XmlElement elem, string s)
        {
            LoadXml(elem);
            Namespace = s;
        }

        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id"))
                Id = elem.GetAttribute("id");
            if (elem.HasAttribute("name"))
                Name = elem.GetAttribute("name");
            if (elem.HasAttribute("abstract"))
                bool.TryParse(elem.GetAttribute("abstract"), out Abstract);
            if (elem.HasAttribute("mixed"))
                bool.TryParse(elem.GetAttribute("mixed"), out Mixed);
            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "annotation")
                {
                    annotation = new XSAnnotation(node as XmlElement);
                }
                else if (node.LocalName == "simpleContent")
                {
                    content = new XSSimpleContent(node as XmlElement);
                }
                else if (node.LocalName == "complexContent")
                {
                    content = new XSComplexContent(node as XmlElement);
                }
                else if (node.LocalName == "group")
                {
                    content = new XSGroup(node as XmlElement);
                }
                else if (node.LocalName == "sequence")
                {
                    content = new XSSequence(node as XmlElement);
                }
                else if (node.LocalName == "all")
                {
                    content = new XSAll(node as XmlElement);
                }
                else if (node.LocalName == "choice")
                {
                    content = new XSChoice(node as XmlElement);
                }
                else if (node.LocalName == "attribute")
                {
                    attributes.Add(new XSAttribute(node as XmlElement));
                }
                else if (node.LocalName == "attributeGroup")
                {
                    attributes.Add(new XSAttributeGroup(node as XmlElement));
                }
                else if (node.LocalName == "anyAttribute")
                {
                    attributes.Add(new XSAnyAttribute(node as XmlElement));
                }
            }
        }
    }
}
