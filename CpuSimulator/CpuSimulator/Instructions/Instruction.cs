using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpuSimulator.Instructions
{
    public class Instruction
    {
        private Parameter parameterOne;
        private Parameter parameterTwo;

        public Instruction()
        {
        }

        public string PlainInstruction
        {
            get;
            set;
        }

        public InstructionTyp Type
        {
            get;
            set;
        }

        public InstructionGroupTyp GroupType
        {
            get
            {
                try
                {
                    if ((int)Type >= 10 && (int)Type <= 14)
                    {
                        return InstructionGroupTyp.Debugging;
                    }
                    else if ((int)Type >= 20 && (int)Type <= 26)
                    {
                        return InstructionGroupTyp.Processor;
                    }
                    else if ((int)Type >= 30 && (int)Type <= 34)
                    {
                        return InstructionGroupTyp.Jump;
                    }
                    else if ((int)Type >= 40)
                    {
                        return InstructionGroupTyp.Arithmetic;
                    }
                }
                catch { ;}

                return InstructionGroupTyp.None;
            }
        }

        public Parameter SourceParameter
        {
            get
            {
                return parameterTwo;
            }
            set
            {
                parameterTwo = value;
            }
        }

        public Parameter TargetParameter
        {
            get
            {
                return parameterOne;
            }
            set
            {
                parameterOne = value;
            }
        }

        public Parameter ParameterOne
        {
            get
            {
                return parameterOne;
            }
            set
            {
                parameterOne = value;
            }
        }

        public Parameter ParameterTwo
        {
            get
            {
                return parameterTwo;
            }
            set
            {
                parameterTwo = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            //Instruction
            builder.AppendLine(String.Format(@"    - Instructiontyp: {0} ({1})", Type.ToString("g"), (int)Type));

            if (parameterOne != null)
            {
                builder.Append(String.Format("       Parameter 1: {0}", parameterOne.ToString()));
            }
            if (parameterTwo != null)
            {
                builder.AppendLine();
                builder.Append(String.Format("       Parameter 2: {0}", parameterTwo.ToString()));
            }

            return builder.ToString();
        }
    }
}
