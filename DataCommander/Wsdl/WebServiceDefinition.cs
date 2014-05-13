using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml;
using System.Net;
using System.IO;

using DataCommander.Model;

namespace DataCommander.Wsdl
{
    /// <summary>
    /// How to use methods in this class.
    /// There are few main scenarios:
    /// <list>
    /// * loading WSDL definition
    /// * call webservice
    /// </list>
    /// 
    /// LOADING WSDL DEFINITION
    /// ----------------------------
    /// 1) call LoadWsdlXmlFile
    ///    - wsdl is loaded into memory
    /// 
    /// CALL WEBSERVICE
    /// ----------------------------
    /// How to call webservice?
    /// We should know these (user will select these info):
    ///  - WSDL.service
    ///  - WSDL.port
    ///  - WSDL.operation
    /// 
    /// 1) call PrepareXmlDocument(operationName, portName)
    /// 2) fill XML doc with values from user
    /// 3) call GetPort(serviceName, portName)
    ///     - you get port address by this
    /// 4) call CallWebService(address,operation,xmlDoc)
    ///     - you get string response
    /// 5) convert string to Xml doc
    ///     - analyze response by parsing xml
    /// 
    /// 
    /// 
    /// </summary>
    public class WebServiceDefinition
    {
        private int buildLevel = 0;
        public string Documentation = string.Empty;
        public List<object> types = new List<object>();
        public List<WSMessage> messages = new List<WSMessage>();
        public List<WSPortType> portTypes = new List<WSPortType>();
        public List<WSBinding> bindings = new List<WSBinding>();
        public List<WSService> services = new List<WSService>();
        public Dictionary<string, string> namespaces = new Dictionary<string, string>();

        public Dictionary<string, string> generatedNamespaces = new Dictionary<string, string>();

        // c:\\Telfort\\MyTools\\DataCommander\\DataCommander\\bin\\
        public bool LoadWsdlXmlFile(string fileName)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(fileName);

            return ProcessXml(doc);
        }

        public bool ProcessXml(XmlDocument doc)
        {
            if (doc.ChildNodes.Count < 1)
                return false;

            foreach (XmlNode node in doc.ChildNodes)
            {
                if (node.LocalName == "definitions" && node.NodeType == XmlNodeType.Element)
                {
                    XmlElement elemNode = node as XmlElement;
                    foreach (XmlAttribute a in elemNode.Attributes)
                    {
                        if (a.Prefix == "xmlns")
                        {
                            namespaces.Add(a.LocalName, a.InnerText);
                        }
                    }
                    foreach (XmlNode item in node.ChildNodes)
                    {
                        if (item.LocalName == "documentation")
                        {
                            Documentation = node.InnerText;
                        }
                        else if (item.LocalName == "types")
                        {
                            ProcessXmlTypes(item.ChildNodes);
                        }
                        else if (item.LocalName == "message")
                        {
                            messages.Add(new WSMessage(item as XmlElement));
                        }
                        else if (item.LocalName == "portType")
                        {
                            portTypes.Add(new WSPortType(item as XmlElement));
                        }
                        else if (item.LocalName == "binding")
                        {
                            bindings.Add(new WSBinding(item as XmlElement));
                        }
                        else if (item.LocalName == "service")
                        {
                            services.Add(new WSService(item as XmlElement));
                        }
                        else
                        {
                            Debugger.Log(0, "", "Unknonwn tag " + item.LocalName + "\n");
                        }
                    }
                }
            }
            return true;
        }

        public bool ProcessXmlTypes(XmlNodeList childNodes)
        {
            string nameSpace = "";
            foreach (XmlNode sch in childNodes)
            {
                if (sch.LocalName == "schema")
                {
                    try
                    {
                        nameSpace = (sch as XmlElement).GetAttribute("targetNamespace");
                    }
                    catch
                    {
                        nameSpace = "";
                    }
                    foreach (XmlNode node in sch.ChildNodes)
                    {
                        if (node.LocalName == "import")
                        {
                        }
                        else if (node.LocalName == "element")
                        {
                            types.Add(new XSElement(node as XmlElement, nameSpace));
                        }
                        else if (node.LocalName == "complexType")
                        {
                            types.Add(new XSComplexType(node as XmlElement, nameSpace));
                        }
                        else if (node.LocalName == "simpleType")
                        {
                            types.Add(new XSSimpleType(node as XmlElement, nameSpace));
                        }
                        else if (node.LocalName == "group")
                        {
                            types.Add(new XSGroup(node as XmlElement, nameSpace));
                        }
                        else if (node.LocalName == "attributeGroup")
                        {
                            types.Add(new XSAttributeGroup(node as XmlElement, nameSpace));
                        }
                        else if (node.LocalName == "attribute")
                        {
                            types.Add(new XSAttribute(node as XmlElement, nameSpace));
                        }
                        else if (node.LocalName == "notation")
                        {
                            types.Add(new XSNotation(node as XmlElement, nameSpace));
                        }
                        else
                        {
                            Debugger.Log(0, "", "unknown type " + node.LocalName + "\n");
                        }
                    }
                }
            }

            return true;
        }

        public WSServicePort GetPort(string serviceName, string portName)
        {
            List<string> ports = new List<string>();
            foreach (WSService ws in services)
            {
                if (ws.Name == serviceName)
                {
                    foreach (WSServicePort port in ws.Ports)
                    {
                        if (port.Name == portName)
                        {
                            return port;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationName"></param>
        /// <param name="portTypeName"></param>
        /// <param name="bRequest"></param>
        /// <param name="bMapping">true - we want mapping, false - we want real call</param>
        /// <returns></returns>
        public XmlDocument PrepareXmlDocument(string operationName, string portTypeName, bool bRequest, bool bMapping)
        {
            string s = PrepareRequestResponse(operationName, portTypeName, bRequest, bMapping);

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(s);

            return doc;
        }


        public DSMNode PrepareModel(string name, string portTypeName, bool bRequest)
        {
            DSMNode envelope = new DSMNode("Envelope", 1, 1);
            envelope.Schema = "soapenv";
            envelope.RootNode = true;
            DSMNode body = envelope.AddChild("Body", 1, 1);

            generatedNamespaces.Clear();

            string partTypeString = bRequest ? "input" : "output";
            string messageName = "";
            WSPortTypeOperation operation = FindPortTypeOperation(portTypeName, name);
            if (operation != null)
            {
                foreach (WSPortTypeOperation.Part part in operation.Parts)
                {
                    if (part.Type == partTypeString)
                    {
                        messageName = part.Message;
                        break;
                    }
                }
            }
            if (messageName.Length > 0)
            {
                WSMessage message = FindMessage(LocalNameFromString(messageName));
                if (message != null)
                {
                    foreach (WSMessagePart mp in message.Parts)
                    {
                        if (mp.Element != null && mp.Element.Length > 0)
                        {
                            PrepareModelDataType(mp.Element, body, null, null);
                        }
                    }
                }
            }

            envelope.Namespaces.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            foreach (string gnkey in generatedNamespaces.Keys)
            {
                envelope.Namespaces.Add(generatedNamespaces[gnkey], gnkey);
            }

            return envelope;
        }

        protected void PrepareModelDataType(string fullTypeName, DSMNode node, string schema, string name)
        {
            string typeName = LocalNameFromString(fullTypeName);
            string schemaName = SchemaFromString(fullTypeName);
            if (schema == null && schemaName.Length > 0)
            {
                schema = GetSchema(namespaces[schemaName]);
            }
            XSBase type = FindDataType(typeName);
            Debugger.Log(0, "", "Namespace=" + type.Namespace + "\n");
            if (type != null)
            {
                if (type is XSComplexType)
                    PrepareModelComplexType(type as XSComplexType, null, node, schema, name);
                else
                    PrepareModelItem(type, null, node, schema, name);
            }
            else
            {
                node.AddChild("Not found: " + typeName, -1, -1);
            }
        }

        protected void PrepareModelDataReference(string refName, DSMNode node, string schema, string name)
        {
            string typeName = LocalNameFromString(refName);
            string schemaName = SchemaFromString(refName);
            if (schemaName.Length > 0)
            {
                string namespacee = namespaces[schemaName];
            }
            if (schemaName.Length > 0)
            {
                schema = GetSchema(namespaces[schemaName]);
            }

            XSBase type = FindDataType(typeName);
            if (type != null)
            {
                if (type is XSComplexType)
                    PrepareModelComplexType(type as XSComplexType, null, node, schema, name);
                else
                    PrepareModelItem(type, null, node, schema, name);
            }
            else
            {
                node.AddChild("Not found: " + typeName, -1, -1);
            }
        }

        protected void PrepareModelItem(XSBase ct, List<XSBase> attributes, DSMNode node, string schema, string name)
        {
            if (ct is XSSequence)
            {
                XSSequence seq = ct as XSSequence;
                foreach (XSBase ob in seq.sequence)
                {
                    PrepareModelItem(ob, null, node, schema, null);
                }
            }
            else if (ct is XSElement)
            {
                string localType;
                XSElement xselem = ct as XSElement;
                //builder.AppendFormat("<!-- min: {0}, max: {1} -->\r\n", xselem.Minoccurs, xselem.Maxoccurs);
                if (xselem.Name.Length > 0)
                    name = xselem.Name;
                localType = LocalNameFromString(xselem.Type);
                if (localType == "string" || localType == "date" || localType == "int"
                    || localType == "dateTime" || localType == "boolean" || localType == "long"
                    || localType == "integer" || localType == "decimal" || localType == "short"
                    || localType == "byte" || localType == "time" || localType == "duration"
                    || localType == "gYearMonth" || localType == "gYear" || localType == "gMonthDay"
                    || localType == "gDay" || localType == "gMonth" || localType == "base64Binary"
                    || localType == "hexBinary" || localType == "float" || localType == "double"
                    || localType == "anyURI" || localType == "QName" || localType == "NOTATION"
                    || localType == "normalizedString" || localType == "token"
                    || localType == "language" || localType == "nonPositiveInteger"
                    || localType == "nonNegativeInteger" || localType == "negativeInteger"
                    || localType == "positiveInteger" || localType == "unsignedLong"
                    || localType == "unsignedInt" || localType == "unsignedShort"
                    || localType == "umsignedByte")
                {
                    DSMNode nn = node.AddChild(name, xselem.Minoccurs, xselem.Maxoccurs);
                    nn.Schema = schema;
                    nn.Type = localType;
                    nn.Value = xselem.Default;
                }
                else if (xselem.complexType != null)
                {
                    XSComplexType ct2 = xselem.complexType;
                    PrepareModelComplexType(ct2, xselem, node, schema, name);
                }
                else if (xselem.simpleType != null)
                {
                    PrepareModelSimpleType(xselem, node, schema, name);
                }
                else
                {
                    if (xselem.Type.Length > 0)
                    {
                        PrepareModelDataType(xselem.Type, node, schema, name);
                    }
                    else if (xselem.Ref.Length > 0)
                    {
                        PrepareModelDataReference(xselem.Ref, node, schema, name);
                    }
                }
            }
            /*else if (ct is XSComplexType)
            {
                XSComplexType compType = (ct as XSComplexType);
            }*/
            else if (ct is XSSimpleType)
            {
                PrepareModelItem((ct as XSSimpleType).content, null, node, schema, null);
            }
            else
            {
                node.AddChild("TODO-PrepareItem-" + ct.ToString(), -1, -1);
            }
        }

        protected void PrepareModelSimpleType(XSElement xselem, DSMNode node, string schema, string name)
        {
            DSMNode nn = node.AddChild(name, (xselem != null ? xselem.Minoccurs : "-1"), (xselem != null ? xselem.Maxoccurs : "-1"));
            nn.Schema = schema;
            if (xselem != null)
                nn.Type = LocalNameFromString(xselem.Type);
            XSSimpleType st = xselem.simpleType;
            PrepareModelItem(st.content, null, node, schema, name);
        }

        protected void PrepareModelComplexType(XSComplexType compType, XSElement xselem, DSMNode node, string schema, string name)
        {
            DSMNode nn = node.AddChild(name, (xselem != null ? xselem.Minoccurs : "-1"), (xselem != null ? xselem.Maxoccurs : "-1"));
            nn.Schema = schema;
            if (xselem != null)
                nn.Type = LocalNameFromString(xselem.Type);
            if (compType.content == null)
            {
                if (compType.attributes != null)
                {
                    List<XSAttribute> attrs = GetAllAttributes(compType.attributes);
                    foreach (XSAttribute obj in attrs)
                    {
                        nn.Attributes.Add(new DSMNode(obj.Name, -1, -1));
                    }
                }
            }
            else
            {
                PrepareModelItem(compType.content, compType.attributes, nn, schema, name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="portTypeName"></param>
        /// <param name="bRequest">true -> request is prepared, false -> response is prepared</param>
        /// <param name="bMapping"></param>
        /// <returns></returns>
        public string PrepareRequestResponse(string name, string portTypeName, bool bRequest, bool bMapping)
        {
            XmlDocument doc = new XmlDocument();
            StringBuilder builder = new StringBuilder();
            buildLevel = 0;
            builder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.AppendLine("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" XMLNAMESPACES00>");
            builder.AppendLine("<soapenv:Body>");
            generatedNamespaces.Clear();
            buildLevel++;
            string partTypeString = bRequest ? "input" : "output";
            string messageName = "";
            WSPortTypeOperation operation = FindPortTypeOperation(portTypeName, name);
            if (operation != null)
            {
                foreach (WSPortTypeOperation.Part part in operation.Parts)
                {
                    if (part.Type == partTypeString)
                    {
                        messageName = part.Message;
                        break;
                    }
                }
            }
            if (messageName.Length > 0)
            {
                WSMessage message = FindMessage(LocalNameFromString(messageName));
                if (message != null)
                {
                    foreach (WSMessagePart mp in message.Parts)
                    {
                        if (mp.Element != null && mp.Element.Length > 0)
                        {
                            PrepareDataType(mp.Element, builder, null, null, bMapping);
                        }
                    }
                }
                Debugger.Log(0, "", "Message is \n");
            }

            buildLevel--;
            builder.AppendLine("</soapenv:Body>");
            builder.AppendLine("</soapenv:Envelope>");

            StringBuilder ns = new StringBuilder();
            foreach (string gnkey in generatedNamespaces.Keys)
            {
                ns.AppendFormat(" xmlns:{0}=\"{1}\"", generatedNamespaces[gnkey], gnkey);
            }
            builder.Replace("XMLNAMESPACES00", ns.ToString());

            return builder.ToString();
        }

        protected string GetSchema(string nameSpaceString)
        {
            if (generatedNamespaces.ContainsKey(nameSpaceString))
            {
                return generatedNamespaces[nameSpaceString];
            }

            string propOrder = "ref";
            string prop = propOrder;
            string[] parts = nameSpaceString.Split('/');
            if (parts.Length > 0)
            {
                prop = parts[parts.Length - 1].Substring(0, 3).ToLower();
                propOrder = prop;
            }
            int i = 1;
            while (i < 1000)
            {
                if (generatedNamespaces.ContainsValue(propOrder))
                {
                    propOrder = prop + i;
                    i++;
                }
                else
                {
                    break;
                }
            }
            generatedNamespaces[nameSpaceString] = propOrder;
            return propOrder;
        }

        protected void PrepareDataType(string fullTypeName, StringBuilder builder, string schema, string name, bool bMapping)
        {
            string typeName = LocalNameFromString(fullTypeName);
            string schemaName = SchemaFromString(fullTypeName);
            if (schema == null && schemaName.Length > 0)
            {
                schema = GetSchema(namespaces[schemaName]);
            }
            XSBase type = FindDataType(typeName);
            Debugger.Log(0,"","Namespace=" + type.Namespace + "\n");
            if (type != null)
            {
                if (type is XSComplexType)
                    PrepareComplexType(type as XSComplexType, null, builder, schema, name, bMapping);
                else
                    PrepareItem(type, null, builder, schema, name, bMapping);
            }
            else
            {
                builder.AppendLine("Not found: " + typeName);
            }
        }

        protected void PrepareDataReference(string refName, StringBuilder builder, string schema, string name, bool bMapping)
        {
            string typeName = LocalNameFromString(refName);
            string schemaName = SchemaFromString(refName);
            if (schemaName.Length > 0)
            {
                string namespacee = namespaces[schemaName];
            }
            if (schemaName.Length > 0)
            {
                schema = GetSchema(namespaces[schemaName]);
            }

            XSBase type = FindDataType(typeName);
            if (type != null)
            {
                if (type is XSComplexType)
                    PrepareComplexType(type as XSComplexType, null, builder, schema, name, bMapping);
                else
                    PrepareItem(type, null, builder, schema, name, bMapping);
            }
            else
            {
                builder.AppendLine("Not found: " + typeName);
            }
        }

        protected void PrepareItem(XSBase ct, List<XSBase> attributes, StringBuilder builder, string schema, string name, bool bMapping)
        {
            if (ct is XSSequence)
            {
                XSSequence seq = ct as XSSequence;
                foreach (XSBase ob in seq.sequence)
                {
                    PrepareItem(ob, null, builder, schema, null, bMapping);
                }
            }
            else if (ct is XSElement)
            {
                string localType;
                XSElement xselem = ct as XSElement;
                //builder.AppendFormat("<!-- min: {0}, max: {1} -->\r\n", xselem.Minoccurs, xselem.Maxoccurs);
                if (xselem.Name.Length > 0)
                    name = xselem.Name;
                localType = LocalNameFromString(xselem.Type);
                if (localType == "string" || localType == "date" || localType == "int"
                    || localType == "dateTime" || localType == "boolean" || localType == "long"
                    || localType == "integer" || localType == "decimal" || localType == "short"
                    || localType == "byte" || localType == "time" || localType == "duration"
                    || localType == "gYearMonth" || localType == "gYear" || localType == "gMonthDay"
                    || localType == "gDay" || localType == "gMonth" || localType == "base64Binary"
                    || localType == "hexBinary" || localType == "float" || localType == "double"
                    || localType == "anyURI" || localType == "QName" || localType == "NOTATION"
                    || localType == "normalizedString" || localType == "token"
                    || localType == "language" || localType == "nonPositiveInteger"
                    || localType == "nonNegativeInteger" || localType == "negativeInteger"
                    || localType == "positiveInteger" || localType == "unsignedLong"
                    || localType == "unsignedInt" || localType == "unsignedShort"
                    || localType == "umsignedByte")
                {
                    if (bMapping)
                    {
                        builder.AppendFormat("<{0}:{1} min=\"{2}\" max=\"{3}\" type=\"{4}\" />", schema, name, xselem.Minoccurs, xselem.Maxoccurs, localType);
                    }
                    else
                    {
                        if (buildLevel > 0)
                            builder.Append(string.Empty.PadLeft(buildLevel));
                        builder.Append("<" + schema + ":" + name + ">");
                        builder.AppendLine("</" + schema + ":" + name + ">");
                    }
                }
                else if (xselem.complexType != null)
                {
                    XSComplexType ct2 = xselem.complexType;
                    PrepareComplexType(ct2, xselem, builder, schema, name, bMapping);
                }
                else if (xselem.simpleType != null)
                {
                    PrepareSimpleType(xselem, builder, schema, name, bMapping);
                }
                else
                {
                    if (xselem.Type.Length > 0)
                    {
                        PrepareDataType(xselem.Type, builder, schema, name, bMapping);
                    }
                    else if (xselem.Ref.Length > 0)
                    {
                        PrepareDataReference(xselem.Ref, builder, schema, name, bMapping);
                    }
                }
            }
            /*else if (ct is XSComplexType)
            {
                XSComplexType compType = (ct as XSComplexType);
            }*/
            else if (ct is XSSimpleType)
            {
                PrepareItem((ct as XSSimpleType).content, null, builder, schema, null, bMapping);
            }
            else
            {
                builder.AppendLine("TODO: PrepareItem: " + ct.ToString());
            }
        }

        protected List<XSAttribute> GetAllAttributes(List<XSBase> objs)
        {
            List<XSAttribute> ret = new List<XSAttribute>();

            foreach (object obj in objs)
            {
                if (obj is XSAttribute)
                {
                    ret.Add(obj as XSAttribute);
                }
                else if (obj is XSAttributeGroup)
                {
                    (obj as XSAttributeGroup).GetAllAttributes(ret);
                }
            }
            return ret;
        }

        protected void PrepareSimpleType(XSElement xselem, StringBuilder builder, string schema, string name, bool bMapping)
        {
            if (buildLevel > 0)
                builder.Append(string.Empty.PadLeft(buildLevel));
            XSSimpleType st = xselem.simpleType;
            if (bMapping)
                builder.AppendFormat("<" + schema + ":{0} min=\"{1}\" max=\"{2}\" type=\"{3}\">\r\n", name, xselem.Minoccurs, xselem.Maxoccurs, LocalNameFromString(xselem.Type));
            else
                builder.AppendLine("<" + schema + ":" + name + ">");
            buildLevel++;
            PrepareItem(st.content, null, builder, schema, name, bMapping);
            buildLevel--;
            if (buildLevel > 0)
                builder.Append(string.Empty.PadLeft(buildLevel));
            builder.AppendLine("</" + schema + ":" + name + ">");
        }

        protected void PrepareComplexType(XSComplexType compType, XSElement xselem, StringBuilder builder, string schema, string name, bool bMapping)
        {
            if (compType.content == null)
            {
                if (buildLevel > 0)
                    builder.Append(string.Empty.PadLeft(buildLevel));
                builder.Append("<" + schema + ":" + name);
                if (compType.attributes != null)
                {
                    List<XSAttribute> attrs = GetAllAttributes(compType.attributes);
                    foreach (XSAttribute obj in attrs)
                    {
                        builder.AppendFormat(" {0}=\"\"", obj.Name);
                    }
                }
                builder.Append(" />\n");
            }
            else
            {
                if (buildLevel > 0)
                    builder.Append(string.Empty.PadLeft(buildLevel));
                if (bMapping)
                {
                    builder.AppendFormat("<" + schema + ":{0}", name);
                    if (xselem != null)
                        builder.AppendFormat(" min=\"{0}\" max=\"{1}\" type=\"{2}\"", xselem.Minoccurs, xselem.Maxoccurs, LocalNameFromString(xselem.Type));
                    else
                        builder.AppendFormat(" min=\"1\" max=\"1\" type=\"\"");
                    builder.AppendFormat(">\r\n");
                }
                else
                    builder.AppendLine("<" + schema + ":" + name + ">");
                buildLevel++;
                PrepareItem(compType.content, compType.attributes, builder, schema, name, bMapping);
                buildLevel--;
                if (buildLevel > 0)
                    builder.Append(string.Empty.PadLeft(buildLevel));
                builder.AppendLine("</" + schema + ":" + name + ">");
            }
        }

        protected XSBase FindDataType(string typeName)
        {
            foreach (XSBase type in types)
            {
                if (type.Name == typeName)
                    return type;
            }
            return null;
        }

        public string SchemaFromString(string s)
        {
            if (s.IndexOf(":") >= 0)
                return s.Substring(0, s.IndexOf(":"));
            return "";
        }

        public string LocalNameFromString(string s)
        {
            if (s.IndexOf(":") >= 0)
                return s.Substring(s.IndexOf(":") + 1);
            return s;
        }

        public WSMessage FindMessage(string name)
        {
            foreach (WSMessage msg in messages)
            {
                if (msg.Name == name)
                    return msg;
            }
            return null;
        }

        public WSPortTypeOperation FindPortTypeOperation(string portType, string operation)
        {
            foreach (WSPortType pt in portTypes)
            {
                if (pt.Name == portType)
                {
                    foreach (WSPortTypeOperation oper in pt.Operations)
                    {
                        if (oper.Name == operation)
                        {
                            return oper;
                        }
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Service (selected by user) -> port (selected by user)
        /// port.address => parameter url
        /// port -> operation (selected by user)
        /// operation.name => action
        /// </summary>
        /// <param name="UrlString">Address of webservice (taken for selected port from selected service)</param>
        /// <param name="ActionString">Name of operation (selected from operations for given binding)</param>
        /// <param name="soapEnvelopeXml">XML generated by PrepareXmlDoc and filled by user's data</param>
        public string CallWebService(string UrlString, string ActionString, XmlDocument soapEnvelopeXml, WebProxy proxy)
        {
            //var _url = "http://xxxxxxxxx/Service1.asmx";
            //var _action = "http://xxxxxxxx/Service1.asmx?op=HelloWorld";

            //string ActionUrl = UrlString + "?op=" + ActionString;
            //string ActionUrl = ;
            //string ActionUrl = "/GetProductInstancesByIspNumber";

            //WebRequest test = WebRequest.Create("http://nso-app302.nl.tiscali.com/PartnerSOAP/DslOrder/DslOrder.asmx");
            //if (proxy != null)
            //    test.Proxy = proxy;
            //test.GetResponse();

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://nso-app302.nl.tiscali.com/PartnerSOAP/DslOrder/DslOrder.asmx");
            webRequest.Proxy = proxy;
            webRequest.Headers.Add("SOAPAction", ActionString);
            webRequest.ContentType = "text/xml;charset=UTF-8";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";

            string cont = 
@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:dsl=""http://nl.tiscali.com/bun/PartnerSOAP/DslOrder"">
   <soapenv:Header/>
   <soapenv:Body>
      <dsl:GetProductInstancesByIspNumber>
         <!--Optional:-->
         <dsl:cobsUser>devtest</dsl:cobsUser>
         <!--Optional:-->
         <dsl:cobsPassword>devtest</dsl:cobsPassword>
         <!--Optional:-->
         <dsl:ispProductInstanceNumber>TVBV8TEW44</dsl:ispProductInstanceNumber>
      </dsl:GetProductInstancesByIspNumber>
   </soapenv:Body>
</soapenv:Envelope>";
            /*webRequest.ProtocolVersion = HttpVersion.Version11;
            string s = soapEnvelopeXml.OuterXml;
            webRequest.ContentLength = s.Length;*/
            using (StreamWriter stream = new StreamWriter(webRequest.GetRequestStream()))
            {
                //stream.Write(cont);
                soapEnvelopeXml.Save(stream);
            }
            //HttpWebRequest webRequest = CreateWebRequest(UrlString, ActionUrl);
            //InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
//            if (proxy != null)
            
            // begin async call to web request.
            //IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            //asyncResult.AsyncWaitHandle.WaitOne();

            WebResponse webResponse = webRequest.GetResponse();
            //WebResponse webResponse = test.GetResponse();

            // get the response from the completed web request.
            string soapResult;
            //using (WebResponse webResponse =  new  webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                return soapResult;
            }
        }

        /// <summary>
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction: " + action);
            webRequest.ContentType = "text/xml;charset=UTF-8";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.ProtocolVersion = HttpVersion.Version11;
//            webRequest.Credentials = CredentialCache.DefaultNetworkCredentials;
            return webRequest;
        }

        /*private static XmlDocument CreateSoapEnvelope()
        {
            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(@"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/1999/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/1999/XMLSchema""><SOAP-ENV:Body><HelloWorld xmlns=""http://tempuri.org/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/""><int1 xsi:type=""xsd:integer"">12</int1><int2 xsi:type=""xsd:integer"">32</int2></HelloWorld></SOAP-ENV:Body></SOAP-ENV:Envelope>");
            return soapEnvelop;
        }*/

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            //string s = soapEnvelopeXml.OuterXml;
            //webRequest.ContentLength = s.Length;
            //webRequest.
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        public static void SplitStringToNameAndIndex(string str, out string name, out int index)
        {
            if (str.EndsWith("]"))
            {
                int bs = str.IndexOf("[");
                if (bs >= 0)
                {
                    if (int.TryParse(str.Substring(bs + 1, str.Length - bs - 2), out index))
                    {
                    }
                    else
                    {
                        throw new Exception("index in part " + str + " cannot be interpreted as integer");
                    }
                }
                else
                {
                    throw new Exception("Missing opening bracket in part " + str);
                }
                name = str.Substring(0, bs);
            }
            else
            {
                name = str;
                index = 0;
            }
        }

        public static XmlNode FindCountedNode(XmlNodeList list, string name, int index)
        {
            int i = 0;
            foreach (XmlNode node in list)
            {
                if (node.LocalName == name)
                {
                    if (i == index)
                        return node;
                    else
                        i++;
                }
            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="path">String with values separated by / (slash) character
        /// </param>
        /// <returns></returns>
        public static XmlNode GetXmlNodeImpl(XmlNode doc, string path)
        {
            string name = string.Empty;
            int index = 0;
            string[] parts = path.Split('/');
            XmlNode node = doc;
            for (int i = 0; i < parts.Length; i++ )
            {
                SplitStringToNameAndIndex(parts[i], out name, out index);
                node = FindCountedNode(node.ChildNodes, name, index);
                if (node == null)
                {
                    throw new Exception("Node " + parts[i] + "not found");
                }
            }
            return node;
        }

        public static XmlNode GetXmlPath(XmlNode doc, string path)
        {
            XmlNode node = null;
            try
            {
                node = GetXmlNodeImpl(doc, path);
            }
            catch(Exception ex)
            {
                Debugger.Log(0, "", "ERROR OCCURED: " + ex.Message); 
            }
            return node;
        }

        public WSBinding FindBinding(string name)
        {
            string localName = LocalNameFromString(name);
            foreach (WSBinding bind in bindings)
            {
                if (localName == bind.Name)
                {
                    return bind;
                }
            }
            return null;
        }

    }
}
