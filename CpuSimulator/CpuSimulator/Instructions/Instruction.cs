using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpuSimulator.Instructions
{
    public class Instruction
    {
        private Parameter parameterOne;

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


        Parameter SourceParameter
        {
            get;
            set;
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

        public Parameter Parameter
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
    }
}
