using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSAny: XSBase
    {
        public string Id = string.Empty;
        public string Maxoccurs = string.Empty;
        public string Minoccurs = string.Empty;
        //public string Namespace = string.Empty;
        public string Processcontents = string.Empty;

        public XSAnnotation annotation = null;

        public XSAny()
        {
        }

        public XSAny(XmlElement elem)
        {
            LoadXml(elem);
        }

        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id")) Id = elem.GetAttribute("id");
            if (elem.HasAttribute("maxOccurs")) Maxoccurs = elem.GetAttribute("maxOccurs");
            if (elem.HasAttribute("minOccurs")) Minoccurs = elem.GetAttribute("minOccurs");
            if (elem.HasAttribute("namespace")) Namespace = elem.GetAttribute("namespace");
            if (elem.HasAttribute("processContents")) Processcontents = elem.GetAttribute("processContents");

            if (elem.ChildNodes.Count > 0 && elem.ChildNodes[0].LocalName == "annotation")
                annotation = new XSAnnotation(elem.ChildNodes[0] as XmlElement);
        }
    }
}
