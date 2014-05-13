using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class WSBindingOperationAspect
    {
        public string Name = string.Empty;
        public List<XmlElement> Exts = new List<XmlElement>();

        public WSBindingOperationAspect()
        {
        }

        public WSBindingOperationAspect(XmlElement elem)
        {
            LoadXml(elem);
        }

        public void LoadXml(XmlElement elem)
        {
            Name = elem.GetAttribute("name");
            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                    Exts.Add(node as XmlElement);
            }
        }
    }
}
