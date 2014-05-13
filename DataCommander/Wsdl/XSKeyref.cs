using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSKeyref: XSBase
    {
        public string Id = string.Empty;
        public string Refer = string.Empty;
        public XSAnnotation annotation = null;
        public List<object> defs = new List<object>();


        public XSKeyref()
        {
        }

        public XSKeyref(XmlElement elem)
        {
            LoadXml(elem);
        }

        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id")) Id = elem.GetAttribute("id");
            if (elem.HasAttribute("name")) Name = elem.GetAttribute("name");
            if (elem.HasAttribute("refer")) Refer = elem.GetAttribute("refer");

            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "annotation")
                {
                    annotation = new XSAnnotation(node as XmlElement);
                }
                else if (node.LocalName == "selector")
                {
                    defs.Add(new XSSelector(node as XmlElement));
                }
                else if (node.LocalName == "field")
                {
                    defs.Add(new XSField(node as XmlElement));
                }
            }
        }
    }
}
