using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSAttribute: XSBase
    {
        public enum FormValue
        {
            Qualified,
            Unqualified
        }

        public enum UseValue
        {
            Optional,
            Prohibited,
            Required
        }

        public string Ref = string.Empty;
        public string Default = string.Empty;
        public string Fixed = string.Empty;
        public FormValue Form = FormValue.Qualified;
        public string Id = string.Empty;
        public string Type = string.Empty;
        public UseValue Use = UseValue.Optional;


        public XSAnnotation annotation = null;
        public XSSimpleType simpleType = null;

        public XSAttribute()
        {
        }
        public XSAttribute(XmlElement elem)
        {
            LoadXml(elem);
        }
        public XSAttribute(XmlElement elem, string s)
        {
            LoadXml(elem);
            Namespace = s;
        }
        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("ref")) Ref = elem.GetAttribute("ref");
            if (elem.HasAttribute("default")) Default = elem.GetAttribute("default");
            if (elem.HasAttribute("fixed")) Fixed = elem.GetAttribute("fixed");
            if (elem.HasAttribute("form"))
            {
                if (elem.GetAttribute("form") == "unqualified")
                    Form = FormValue.Unqualified;
                else if (elem.GetAttribute("form") == "qualified")
                    Form = FormValue.Qualified;
            }
            if (elem.HasAttribute("id")) Id = elem.GetAttribute("id");
            if (elem.HasAttribute("name")) Name = elem.GetAttribute("name");
            if (elem.HasAttribute("type")) Type = elem.GetAttribute("type");
            if (elem.HasAttribute("use"))
            {
                if (elem.GetAttribute("use") == "optional")
                    Use = UseValue.Optional;
                else if (elem.GetAttribute("use") == "prohibited")
                    Use = UseValue.Prohibited;
                else if (elem.GetAttribute("use") == "required")
                    Use = UseValue.Required;
            }

            foreach(XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "annotation")
                    annotation = new XSAnnotation(node as XmlElement);
                else if (node.LocalName == "simpleType")
                    simpleType = new XSSimpleType(node as XmlElement);
            }
        }
    }
}
