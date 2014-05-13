using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class WSMessagePart
    {
        public string Name = string.Empty;
        public string Element = null;
        public string Type = null;

        public WSMessagePart()
        {
        }

        public WSMessagePart(XmlElement elem)
        {
            LoadXml(elem);
        }

        public void LoadXml(XmlElement elem)
        {
            Name = elem.GetAttribute("name");
            if (elem.HasAttribute("element"))
                Element = elem.GetAttribute("element");
            if (elem.HasAttribute("type"))
                Type = elem.GetAttribute("type");
        }
    }
}
