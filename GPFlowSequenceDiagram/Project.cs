using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;

namespace GPFlowSequenceDiagram
{
    public class Project: ProjectReference
    {
        public static Project CurrentProject = null;

        private bool useProxy = false;
        private string proxyUri = string.Empty;
        private string proxyUser = string.Empty;
        private string proxyPassword = string.Empty;

        private int lastObjectId = 0;
        private DiagramItemCollection allItems;

        /// <summary>
        /// Plain constructor
        /// </summary>
        public Project(): base()
        {
            Clear();
            allItems = new DiagramItemCollection(this);
        }

        /// <summary>
        /// Constructor with reading content from file.
        /// </summary>
        /// <param name="aFileName">Name of the file.</param>
        public Project(string aFileName)
        {
            LoadFile(aFileName);
        }

        public bool UseProxy
        {
            get { return useProxy; }
            set { useProxy = value; modified = true; }
        }

        public string ProxyUri
        {
            get { return proxyUri; }
            set { proxyUri = value; modified = true; }
        }

        public string ProxyUser
        {
            get { return proxyUser; }
            set { proxyUser = value; modified = true; }
        }

        public string ProxyPassword
        {
            get { return proxyPassword; }
            set { proxyPassword = value; modified = true; }
        }

        /// <summary>
        /// Loading content of file into memory.
        /// </summary>
        /// <param name="aFileName">Name of the file.</param>
        public bool LoadFile(string aFileName)
        {
            // before loading
            Clear();
            fileName = aFileName;

            // loading
            XmlDocument doc = new XmlDocument();
            doc.Load(aFileName);

            // after loading
            return LoadDocument(doc);
        }

        /// <summary>
        /// Saves content of this object to file. 
        /// Uses private attribute fileName.
        /// </summary>
        /// <returns></returns>
        public bool SaveFile()
        {
            if (fileName == null || fileName.Length == 0)
                return false;

            return SaveFile(fileName);
        }

        /// <summary>
        /// Saves file to permanent storage. Requires name of the file on input.
        /// </summary>
        /// <param name="aFileName">Name of the file.</param>
        public bool SaveFile(string aFileName)
        {
            // before saving
            XmlDocument doc = SaveXmlDocument();

            // saving
            doc.Save(aFileName);

            // after saving
            if (fileName != aFileName)
                fileName = aFileName;
            return true;
        }

        /// <summary>
        /// Clearing the content of this object
        /// </summary>
        public void Clear()
        {
        }

        public bool LoadDocument(XmlDocument doc)
        {
            if (doc == null)
                return false;
            XmlElement root = null;
            foreach (XmlNode node in doc.ChildNodes)
            {
                if (node.Name == "dcroot")
                {
                    root = node as XmlElement;
                    break;
                }
            }

            if (root == null)
                return false;
            XmlElement elem = null;
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name == "ProjectName")
                    projectName = node.InnerText;
                else if (node.Name == "Proxy")
                {
                    elem = node as XmlElement;
                    bool.TryParse(elem.GetAttribute("Use"), out useProxy);
                    proxyPassword = elem.GetAttribute("Password");
                    proxyUri = elem.GetAttribute("Uri");
                    proxyUser = elem.GetAttribute("User");
                }
            }

            return true;
        }

        public XmlDocument SaveXmlDocument()
        {
            XmlDocument doc = new XmlDocument();

            XmlElement root = doc.CreateElement("dcroot");
            XmlElement leaf;

            doc.AppendChild(root);

            leaf = doc.CreateElement("ProjectName");
            leaf.InnerText = projectName;
            root.AppendChild(leaf);

            leaf = doc.CreateElement("Proxy");
            leaf.SetAttribute("Use", UseProxy.ToString());
            leaf.SetAttribute("Uri", ProxyUri);
            leaf.SetAttribute("User", ProxyUser);
            leaf.SetAttribute("Password", ProxyPassword);
            root.AppendChild(leaf);


            return doc;
        }

        public override int DE_GetUniqueId()
        {
            lastObjectId++;
            return lastObjectId;
        }

        public override string DE_GetUniqueEntityName(string typeElement)
        {
            if (typeElement == "process")
                return GetNewProcessName();
            return base.DE_GetUniqueEntityName(typeElement);
        }

        /// <summary>
        /// Generates unique name for new process instance
        /// </summary>
        /// <returns></returns>
        public string GetNewProcessName()
        {
            for (int i = 1; i < 1000; i++)
            {
                string name = string.Format("Process {0}", i);
                CFDIProcesss proc = FindProcessInstanceByName(name);
                if (proc == null)
                    return name;
            }

            return "Process";
        }

        /// <summary>
        /// Finds process instance in this project by process name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CFDIProcesss FindProcessInstanceByName(string name)
        {
            foreach (DiagramItem proc in allItems)
            {
                if (proc is CFDIProcesss)
                {
                    CFDIProcesss procA = (CFDIProcesss)proc;
                    if (name.Equals(procA.ProcessName.Text))
                        return procA;
                }
            }

            return null;
        }
    }
}
