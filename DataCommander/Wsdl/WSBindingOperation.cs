using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class WSBindingOperation: WSBase
    {
        public List<XmlElement> Exts = new List<XmlElement>();
        public WSBindingOperationAspect input = null;
        public WSBindingOperationAspect output = null;
        public List<WSBindingOperationAspect> faults = new List<WSBindingOperationAspect>();

        public WSBindingOperation()
        {
        }

        public WSBindingOperation(XmlElement elem)
        {
            LoadXml(elem);
        }

        public void LoadXml(XmlElement elem)
        {
            Name = elem.GetAttribute("name");
            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "input")
                    input = new WSBindingOperationAspect(node as XmlElement);
                else if (node.LocalName == "output")
                    output = new WSBindingOperationAspect(node as XmlElement);
                else if (node.LocalName == "fault")
                    faults.Add(new WSBindingOperationAspect(node as XmlElement));
                else if (node.NodeType == XmlNodeType.Element)
                    Exts.Add(node as XmlElement);
            }
        }
    }
}
