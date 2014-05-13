using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CASE.Model
{
    public class FunctionParameterDesc
    {
        public enum InOutTypeEnum
        {
            In,
            Out,
            InOut
        }

        public FunctionParameterDesc()
        {
        }

        public FunctionParameterDesc(string dtype, string value, string inout)
        {
            DataType = dtype;
            Name = value;
            InOut = inout;
        }

        public string p_dataType = string.Empty;
        public string p_name = string.Empty;
        public string p_inOut = string.Empty;

        [DisplayName("Data Type"),Category("A")]
        public string DataType
        {
            get { return p_dataType; }
            set { p_dataType = value; }
        }

        [DisplayName("Parameter Name"),Category("A")]
        public string Name
        {
            get { return p_name; }
            set { p_name = value; }
        }

        [Browsable(false)]
        public string InOut
        {
            get { return p_inOut; }
            set { p_inOut = value; }
        }

        [DisplayName("In/Out Type"),Category("B")]
        public InOutTypeEnum InOutType
        {
            get
            {
                switch (p_inOut)
                {
                    case "In": return InOutTypeEnum.In;
                    case "Out": return InOutTypeEnum.Out;
                    case "InOut": return InOutTypeEnum.InOut;
                }
                return InOutTypeEnum.In;
            }
            set
            {
                p_inOut = value.ToString();
            }
        }

    }
}
