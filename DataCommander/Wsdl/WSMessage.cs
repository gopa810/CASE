using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class WSMessage
    {
        public string Name = string.Empty;
        public List<WSMessagePart> Parts = new List<WSMessagePart>();

        public WSMessage()
        {
        }

        public WSMessage(XmlElement elem)
        {
            LoadXml(elem);
        }

        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("name"))
                Name = elem.GetAttribute("name");
            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "part")
                    Parts.Add(new WSMessagePart(node as XmlElement));
            }
        }
    }
}
