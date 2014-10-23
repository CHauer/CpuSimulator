using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpuSimulator.Components
{
    public class Instruction
    {
        public Instruction()
        {
            Parameters = new List<Parameter>();
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

        public List<Parameter> Parameters
        {
            get;
            private set; 
        }
    }
}
