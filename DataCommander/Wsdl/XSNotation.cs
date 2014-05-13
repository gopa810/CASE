using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSNotation: XSBase
    {
        public string Id = string.Empty;
        public string Public = string.Empty;
        public string System = string.Empty;

        public XSAnnotation annotation = null;

        public XSNotation()
        {
        }
        public XSNotation(XmlElement elem)
        {
            LoadXml(elem);
        }
        public XSNotation(XmlElement elem, string s)
        {
            LoadXml(elem);
            Namespace = s;
        }
        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id")) Id = elem.GetAttribute("id");
            if (elem.HasAttribute("name")) Name = elem.GetAttribute("name");
            if (elem.HasAttribute("public")) Public = elem.GetAttribute("public");
            if (elem.HasAttribute("system")) System = elem.GetAttribute("system");

            if (elem.ChildNodes.Count > 0 && elem.ChildNodes[0].LocalName == "annotation")
                annotation = new XSAnnotation(elem.ChildNodes[0] as XmlElement);
        }    
    }
}
