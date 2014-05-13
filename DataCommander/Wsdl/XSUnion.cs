using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSUnion: XSBase
    {
        public string Id = string.Empty;
        public string MemberTypes = string.Empty;
        public XSAnnotation annotation = new XSAnnotation();
        public List<XSSimpleType> simpleTypes = new List<XSSimpleType>();

        public XSUnion()
        {
        }
        public XSUnion(XmlElement elem)
        {
            LoadXml(elem);
        }
        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id"))
                Id = elem.GetAttribute("id");
            if (elem.HasAttribute("memberTypes"))
                MemberTypes = elem.GetAttribute("memberTypes");
            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "annotation")
                {
                    annotation = new XSAnnotation(node as XmlElement);
                }
                else if (node.LocalName == "simpleType")
                {
                    simpleTypes.Add(new XSSimpleType(node as XmlElement));
                }
            }
        }
    }
}
