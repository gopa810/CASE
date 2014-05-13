using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSAnnotation: XSBase
    {
        public string Id = string.Empty;
        public List<XSNamedText> infos = new List<XSNamedText>();

        public XSAnnotation()
        {
        }
        public XSAnnotation(XmlElement elem)
        {
            LoadXml(elem);
        }
        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id"))
                Id = elem.GetAttribute("id");
            foreach (XmlNode node in elem.ChildNodes)
            {
                infos.Add(new XSNamedText(node as XmlElement));
            }
        }
    }
}
