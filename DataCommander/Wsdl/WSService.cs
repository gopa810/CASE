using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class WSService: WSBase
    {
        public List<WSServicePort> Ports = new List<WSServicePort>();



        public WSService()
        {
        }

        public WSService(XmlElement elem)
        {
            LoadXml(elem);
        }

        public void LoadXml(XmlElement elem)
        {
            Name = elem.GetAttribute("name");
            foreach (XmlElement item in elem.ChildNodes)
            {
                if (item.LocalName == "documentation")
                    Documentation = item.InnerText;
                else if (item.LocalName == "port")
                {
                    Ports.Add(new WSServicePort(item));
                }
            }
        }
    }
}
