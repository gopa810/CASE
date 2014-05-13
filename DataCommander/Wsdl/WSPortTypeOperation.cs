using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class WSPortTypeOperation
    {
        public class Part
        {
            public string Type = string.Empty;
            public string Name = null;
            public string Message = string.Empty;
        }

        public string Name = string.Empty;
        public string ParameterOrder = string.Empty;
        public List<Part> Parts = new List<Part>();

        public WSPortTypeOperation()
        {
        }

        public WSPortTypeOperation(XmlElement elem)
        {
            LoadXml(elem);
        }

        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("name"))
                Name = elem.GetAttribute("name");
            if (elem.HasAttribute("parameterOrder"))
                ParameterOrder = elem.GetAttribute("parameterOrder");
            foreach (XmlNode node in elem.ChildNodes)
            {
                Part part = new Part();
                XmlElement item = node as XmlElement;
                if (item.HasAttribute("name"))
                    part.Name = item.GetAttribute("name");
                if (item.HasAttribute("message"))
                    part.Message = item.GetAttribute("message");
                part.Type = item.LocalName;
                Parts.Add(part);
            }
        }
    }
}
