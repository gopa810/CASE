using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSExtension: XSAttributedContentBase
    {
        public string Base = string.Empty;

        public XSAnnotation annotation = null;
        public object choice = null;
        //public List<object> attributes = new List<object>();

        public XSExtension()
        {
        }

        public XSExtension(XmlElement elem)
        {
            LoadXml(elem);
        }

        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id")) Id = elem.GetAttribute("id");
            if (elem.HasAttribute("base")) Base = elem.GetAttribute("base");
            
            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "annotation")
                    annotation = new XSAnnotation(node as XmlElement);
                else if (node.LocalName == "element")
                    choice = new XSElement(node as XmlElement);
                else if (node.LocalName == "group")
                    choice = new XSGroup(node as XmlElement);
                else if (node.LocalName == "choice")
                    choice = new XSChoice(node as XmlElement);
                else if (node.LocalName == "sequence")
                    choice = new XSSequence(node as XmlElement);
                else if (node.LocalName == "any")
                    choice = new XSAny(node as XmlElement);
                else if (node.LocalName == "attribute")
                    attributes.Add(new XSAttribute(node as XmlElement));
                else if (node.LocalName == "attributeGroup")
                    attributes.Add(new XSAttributeGroup(node as XmlElement));
                else if (node.LocalName == "anyAttribute")
                    attributes.Add(new XSAnyAttribute(node as XmlElement));
            }
        }
    }
}
