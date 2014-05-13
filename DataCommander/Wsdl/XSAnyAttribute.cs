using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSAnyAttribute: XSBase
    {
        public string Id = string.Empty;
        //public string Namespace = string.Empty;
        public string ProcessContents = string.Empty;
        public XSAnnotation annotation = null;

        public XSAnyAttribute()
        {
        }
        public XSAnyAttribute(XmlElement elem)
        {
            LoadXml(elem);
        }
        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id"))
                Id = elem.GetAttribute("id");
            if (elem.HasAttribute("namespace"))
                Namespace = elem.GetAttribute("namespace");
            if (elem.HasAttribute("processContents"))
                ProcessContents = elem.GetAttribute("processContents");
            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "annotation")
                {
                    annotation = new XSAnnotation(node as XmlElement);
                }
            }
        }
    }
}
