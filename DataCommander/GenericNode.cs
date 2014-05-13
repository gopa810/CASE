using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CASE.Model
{
    public class GenericNode
    {
        public bool RootNode = false;
        public string Name = string.Empty;
        public string Schema = string.Empty;
        public string Type = string.Empty;
        public string Min = "0";
        public string Max = "unbounded";
        public string Value = null;

        private Dictionary<string, string> myNamespaces = null;
        private List<GenericNode> myNodes = null;
        private List<GenericNode> myAttributes = null;

        public GenericNode()
        {
        }

        public GenericNode(string aName, int min, int max)
        {
            Name = aName;
            if (min >= 0)
                Min = min.ToString();
            if (max >= 0)
                Max = max.ToString();
        }

        public GenericNode AddChild(string aName, string aMin, string aMax)
        {
            GenericNode nn = new GenericNode();
            if (aName != null) nn.Name = aName;
            if (aMin != null) nn.Min = aMin;
            if (aMax != null) nn.Max = aMax;

            Nodes.Add(nn);

            return nn;
        }

        public GenericNode AddChild(string aName, int min, int max)
        {
            GenericNode nn = new GenericNode(aName, min, max);

            nn.Schema = this.Schema;
            Nodes.Add(nn);

            return nn;
        }

        public void Clear()
        {
            Nodes.Clear();
        }

        public List<GenericNode> Nodes
        {
            get
            {
                if (myNodes == null)
                    myNodes = new List<GenericNode>();
                return myNodes;
            }
        }

        public Dictionary<string, string> Namespaces
        {
            get
            {
                if (myNamespaces == null)
                    myNamespaces = new Dictionary<string, string>();
                return myNamespaces;
            }
        }

        public List<GenericNode> Attributes
        {
            get
            {
                if (myAttributes == null)
                    myAttributes = new List<GenericNode>();
                return myAttributes;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            MakeString(builder, 0);
            return builder.ToString();
        }

        public void MakeString(StringBuilder sb, int level)
        {
            bool hasSubnode = false;
            if (this.RootNode)
            {
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            }
            sb.Append(string.Empty.PadLeft(level));
            sb.AppendFormat("<{0}:{1}", Schema, Name);
            if (myAttributes != null && myAttributes.Count > 0)
            {
                foreach (GenericNode attr in myAttributes)
                {
                    sb.AppendFormat(" {0}=\"\"", attr.Name);
                }
            }

            if (myNamespaces != null && myNamespaces.Count > 0)
            {
                foreach (string keydn in myNamespaces.Keys)
                {
                    sb.AppendFormat(" {0}=\"{1}\"", keydn, myNamespaces[keydn]);
                }
            }

            if (myNodes != null && myNodes.Count > 0)
                hasSubnode = true;

            if (hasSubnode)
            {
                sb.AppendLine(">");
                foreach (GenericNode nod in myNodes)
                {
                    nod.MakeString(sb, level + 1);
                }
                sb.Append(string.Empty.PadLeft(level));
                sb.AppendLine(string.Format("</{0}:{1}>", Schema, Name));
            }
            else if (this.Value != null)
            {
                sb.Append(">");
                sb.Append(this.Value);
                sb.AppendLine(string.Format("</{0}:{1}>", Schema, Name));
            }
            else
            {
                sb.AppendLine(" />");
            }
        }
    }
}
