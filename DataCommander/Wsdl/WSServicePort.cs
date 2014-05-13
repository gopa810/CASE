using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class WSServicePort: WSBase
    {
        public string baseBinding = string.Empty;
        public string baseAddress = string.Empty;
        public List<XmlElement> Exts = new List<XmlElement>();

        public WSServicePort()
        {
        }

        public WSServicePort(XmlElement elem)
        {
            LoadXml(elem);
        }

        public string Binding
        {
            get { return baseBinding; }
            set { baseBinding = value; }
        }

        public string Address
        {
            get { return baseAddress; }
            set { baseAddress = value; }
        }


        public void LoadXml(XmlElement elem)
        {
            Name = elem.GetAttribute("name");
            if (elem.HasAttribute("binding"))
                Binding = elem.GetAttribute("binding");
            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    XmlElement el = node as XmlElement;
                    Exts.Add(el);
                    if (el.LocalName == "address" && el.HasAttribute("location"))
                    {
                        Address = el.GetAttribute("location");
                    }
                }
            }
        }
    }
}
