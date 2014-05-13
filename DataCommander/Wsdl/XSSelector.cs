using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSSelector
    {
        public string Id = string.Empty;
        public string Xpath = string.Empty;

        public XSAnnotation annotation = null;

        public XSSelector()
        {
        }

        public XSSelector(XmlElement elem)
        {
            LoadXml(elem);
        }

        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id")) Id = elem.GetAttribute("id");
            if (elem.HasAttribute("xpath")) Xpath = elem.GetAttribute("xpath");

            if (elem.ChildNodes.Count > 0 && elem.ChildNodes[0].LocalName == "annotation")
                annotation = new XSAnnotation(elem.ChildNodes[0] as XmlElement);

        }
    }
}
