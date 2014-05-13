using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DataCommander.Wsdl
{
    public class XSRestriction: XSAttributedContentBase
    {
        public string Base = string.Empty;

        public XSAnnotation annotation = null;
        public XSSimpleType simpleType = null;
        public XSAttributedContentBase attributedContent = null;
        public List<XSRestrictionValue> restrictions = new List<XSRestrictionValue>();

        public XSRestriction()
        {
        }
        public XSRestriction(XmlElement elem)
        {
            LoadXml(elem);
        }
        public void LoadXml(XmlElement elem)
        {
            if (elem.HasAttribute("id"))
                Id = elem.GetAttribute("id");
            if (elem.HasAttribute("base"))
                Base = elem.GetAttribute("base");

            foreach (XmlNode node in elem.ChildNodes)
            {
                if (node.LocalName == "annotation")
                {
                    annotation = new XSAnnotation(node as XmlElement);
                }
                else if (node.LocalName == "simpleType")
                {
                    simpleType = new XSSimpleType(node as XmlElement);
                }
                else if (node.LocalName == "minExclusive")
                {
                    restrictions.Add(new XSRestrictionValue(node as XmlElement));
                }
                else if (node.LocalName == "minInclusive")
                {
                    restrictions.Add(new XSRestrictionValue(node as XmlElement));
                }
                else if (node.LocalName == "maxExclusive")
                {
                    restrictions.Add(new XSRestrictionValue(node as XmlElement));
                }
                else if (node.LocalName == "maxInclusive")
                {
                    restrictions.Add(new XSRestrictionValue(node as XmlElement));
                }
                else if (node.LocalName == "totalDigits")
                {
                    restrictions.Add(new XSRestrictionValue(node as XmlElement));
                }
                else if (node.LocalName == "fractionDigits")
                {
                    restrictions.Add(new XSRestrictionValue(node as XmlElement));
                }
                else if (node.LocalName == "length")
                {
                    restrictions.Add(new XSRestrictionValue(node as XmlElement));
                }
                else if (node.LocalName == "minLength")
                {
                    restrictions.Add(new XSRestrictionValue(node as XmlElement));
                }
                else if (node.LocalName == "maxLength")
                {
                    restrictions.Add(new XSRestrictionValue(node as XmlElement));
                }
                else if (node.LocalName == "enumeration")
                {
                    restrictions.Add(new XSRestrictionValue(node as XmlElement));
                }
                else if (node.LocalName == "whiteSpace")
                {
                    restrictions.Add(new XSRestrictionValue(node as XmlElement));
                }
                else if (node.LocalName == "pattern")
                {
                    restrictions.Add(new XSRestrictionValue(node as XmlElement));
                }
                else if (node.LocalName == "attribute")
                {
                    attributes.Add(new XSAttribute(node as XmlElement));
                }
                else if (node.LocalName == "attributeGroup")
                {
                    attributes.Add(new XSAttributeGroup(node as XmlElement));
                }
                else if (node.LocalName == "anyAttribute")
                {
                    attributes.Add(new XSAnyAttribute(node as XmlElement));
                }
                else if (node.LocalName == "group")
                {
                    attributedContent = new XSGroup(node as XmlElement);
                }
                else if (node.LocalName == "all")
                {
                    attributedContent = new XSAll(node as XmlElement);
                }
                else if (node.LocalName == "choice")
                {
                    attributedContent = new XSChoice(node as XmlElement);
                }
                else if (node.LocalName == "sequence")
                {
                    attributedContent = new XSSequence(node as XmlElement);
                }
            }
        }
    }
}
