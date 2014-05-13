using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSElement: XSBase
    {
        public string Id = string.Empty;
        public string Maxoccurs = string.Empty;
        public string Minoccurs = string.Empty;
        public string Ref = string.Empty;
        public string Type = string.Empty;
        public string Substitutiongroup = string.Empty;
        public string Default = string.Empty;
        public string Fixed = string.Empty;
        public string Form = string.Empty;
        public bool Nillable = false;
        public bool Abstract = false;
        public string Block = string.Empty;
        public string Final = string.Empty;

        public XSAnnotation annotation = null;
        public XSSimpleType simpleType = null;
        public XSComplexType complexType = null;
        public List<object> keys = new List<object>();

        public XSElement()
        {
        }
        public XSElement(XmlElement elem)
        {
            LoadXml(elem);
        }
        public XSElement(XmlElement elem, string s)
        {
            LoadXml(elem);
            Namespace = s;
        }
        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id")) Id = elem.GetAttribute("id");
            if (elem.HasAttribute("maxOccurs")) Maxoccurs = elem.GetAttribute("maxOccurs");
            if (elem.HasAttribute("minOccurs")) Minoccurs = elem.GetAttribute("minOccurs");
            if (elem.HasAttribute("name")) Name = elem.GetAttribute("name");
            if (elem.HasAttribute("ref")) Ref = elem.GetAttribute("ref");
            if (elem.HasAttribute("type")) Type = elem.GetAttribute("type");
            if (elem.HasAttribute("substitutionGroup")) Substitutiongroup = elem.GetAttribute("substitutionGroup");
            if (elem.HasAttribute("default")) Default = elem.GetAttribute("default");
            if (elem.HasAttribute("fixed")) Fixed = elem.GetAttribute("fixed");
            if (elem.HasAttribute("form")) Form = elem.GetAttribute("form");
            if (elem.HasAttribute("nillable")) bool.TryParse(elem.GetAttribute("nillable"), out Nillable);
            if (elem.HasAttribute("abstract")) bool.TryParse(elem.GetAttribute("abstract"), out Abstract);
            if (elem.HasAttribute("block")) Block = elem.GetAttribute("block");
            if (elem.HasAttribute("final")) Final = elem.GetAttribute("final");

            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "annotation")
                    annotation = new XSAnnotation(node as XmlElement);
                else if (node.LocalName == "simpleType")
                    simpleType = new XSSimpleType(node as XmlElement);
                else if (node.LocalName == "complexType")
                    complexType = new XSComplexType(node as XmlElement);
                else if (node.LocalName == "unique")
                    keys.Add(new XSUnique(node as XmlElement));
                else if (node.LocalName == "key")
                    keys.Add(new XSKey(node as XmlElement));
                else if (node.LocalName == "keyref")
                    keys.Add(new XSKeyref(node as XmlElement));
            }
        }
    }
}
