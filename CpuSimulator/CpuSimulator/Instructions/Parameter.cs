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
    }
}
