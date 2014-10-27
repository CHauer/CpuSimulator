using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpuSimulator.Instructions
{
    public class Parameter
    {
        public int Content
        {
            get;
            set;
        }

        public ParameterTyp Type
        {
            get;
            set;
        }

        public char Register
        {
            get;
            set;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case ParameterTyp.Address:
                case ParameterTyp.Data:
                case ParameterTyp.StackOffset:
                    return String.Format("Typ: {0} Value: {1}", Type.ToString("g").PadRight(10), Content.ToString().PadRight(15));
                case ParameterTyp.Register:
                case ParameterTyp.RegisterAddress:
                    return String.Format("Typ: {0} Value: {1}", Type.ToString("g").PadRight(10), Register.ToString().PadRight(15));
            }

            return string.Empty;
        }
    }
}
