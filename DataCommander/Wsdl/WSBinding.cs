using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class WSBinding
    {
        public string Name = string.Empty;
        public string Type = string.Empty;
        public List<XmlElement> Exts = new List<XmlElement>();
        public List<WSBindingOperation> Operations = new List<WSBindingOperation>();

        public WSBinding()
        {
        }

        public WSBinding(XmlElement elem)
        {
            LoadXml(elem);
        }

        public void LoadXml(XmlElement elem)
        {
            Name = elem.GetAttribute("name");
            Type = elem.GetAttribute("type");
            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "operation")
                    Operations.Add(new WSBindingOperation(node as XmlElement));
                else if (node.NodeType == XmlNodeType.Element)
                    Exts.Add(node as XmlElement);
            }

        }
    }
}
