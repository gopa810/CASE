using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSChoice:XSAttributedContentBase
    {
        //public string Id = string.Empty;
        public string Maxoccurs = string.Empty;
        public string Minoccurs = string.Empty;

        public XSAnnotation annotation = null;
        public List<object> choices = new List<object>();

        public XSChoice()
        {
        }
        public XSChoice(XmlElement elem)
        {
            LoadXml(elem);
        }
        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id")) Id = elem.GetAttribute("id");
            if (elem.HasAttribute("maxOccurs")) Maxoccurs = elem.GetAttribute("maxOccurs");
            if (elem.HasAttribute("minOccurs")) Minoccurs = elem.GetAttribute("minOccurs");

            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "annotation")
                    annotation = new XSAnnotation(node as XmlElement);
                else if (node.LocalName == "element")
                    choices.Add(new XSElement(node as XmlElement));
                else if (node.LocalName == "group")
                    choices.Add(new XSGroup(node as XmlElement));
                else if (node.LocalName == "choice")
                    choices.Add(new XSChoice(node as XmlElement));
                else if (node.LocalName == "sequence")
                    choices.Add(new XSSequence(node as XmlElement));
                else if (node.LocalName == "any")
                    choices.Add(new XSAny(node as XmlElement));
            }
        }
    }
}
