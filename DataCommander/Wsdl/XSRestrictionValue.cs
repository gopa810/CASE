using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSRestrictionValue
    {
        public string Name = string.Empty;
        public string Value = string.Empty;

        public XSRestrictionValue()
        {
        }
        public XSRestrictionValue(XmlElement elem)
        {
            LoadXml(elem);
        }
        public void LoadXml(XmlElement elem)
        {
            Name = elem.LocalName;
            if (elem.HasAttribute("value"))
                Value = elem.GetAttribute("value");
        }
    }
}
