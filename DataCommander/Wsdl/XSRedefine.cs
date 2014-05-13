using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSRedefine
    {
        public string Id = string.Empty;
        public string Schemalocation = string.Empty;

        public List<object> defs = new List<object>();

        public XSRedefine()
        {
        }

        public XSRedefine(XmlElement elem)
        {
            LoadXml(elem);
        }

        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id")) Id = elem.GetAttribute("id");
            if (elem.HasAttribute("schemaLocation")) Schemalocation = elem.GetAttribute("schemaLocation");

            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "annotation")
                {
                    defs.Add(new XSAnnotation(node as XmlElement));
                }
                else if (node.LocalName == "simpleType")
                {
                    defs.Add(new XSSimpleType(node as XmlElement));
                }
                else if (node.LocalName == "complexType")
                {
                    defs.Add(new XSComplexType(node as XmlElement));
                }
                else if (node.LocalName == "group")
                {
                    defs.Add(new XSGroup(node as XmlElement));
                }
                else if (node.LocalName == "attributeGroup")
                {
                    defs.Add(new XSAttributeGroup(node as XmlElement));
                }
            }
        }
    }
}
