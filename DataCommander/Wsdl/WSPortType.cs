using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class WSPortType: WSBase
    {
        public List<WSPortTypeOperation> Operations = new List<WSPortTypeOperation>();

        public WSPortType()
        {
        }

        public WSPortType(XmlElement elem)
        {
            LoadXml(elem);
        }

        public void LoadXml(XmlElement elem)
        {
            Name = elem.GetAttribute("name");
            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "operation")
                    Operations.Add(new WSPortTypeOperation(node as XmlElement));
            }
        }
    }
}
