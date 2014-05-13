using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCommander.Wsdl
{
    public class XSAttributedContentBase: XSContentBase
    {
        public string Id = string.Empty;
        public List<XSBase> attributes = new List<XSBase>();

        public void GetAllAttributes(List<XSAttribute> array)
        {
            foreach (object obj in attributes)
            {
                if (obj is XSAttribute)
                {
                    array.Add(obj as XSAttribute);
                }
                else if (obj is XSAttributeGroup)
                {
                    (obj as XSAttributeGroup).GetAllAttributes(array);
                }
            }
        }

    }
}
