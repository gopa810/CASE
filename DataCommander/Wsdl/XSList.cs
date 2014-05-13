using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSList: XSBase
    {
        public string Id = string.Empty;
        public string ItemType = string.Empty;
        public XSAnnotation annotation = null;
        public XSSimpleType simpleType = new XSSimpleType();

        public XSList()
        {
        }
        public XSList(XmlElement elem)
        {
            LoadXml(elem);
        }
        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id"))
                Id = elem.GetAttribute("id");
            if (elem.HasAttribute("itemType"))
                ItemType = elem.GetAttribute("itemType");
            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "annotation")
                {
                    annotation = new XSAnnotation(node as XmlElement);
                }
                else if (node.LocalName == "simpleType")
                {
                    simpleType = new XSSimpleType(node as XmlElement);
                }
            }
        }
    }
}
