using System;
using CpuSimulator.Instructions;

namespace CpuSimulator.Components
{
    public class ALU
    {
        public int R
        {
            get;
            set;
        }

        public int Carry
        {
            get;
            private set;
        }

        public int Zero
        {
            get;
            private set;
        }

        public int Negative
        {
            get;
            private set;
        }

        public void Execute(int a, int b, InstructionTyp instruction)
        {
            switch (instruction)
            {
                case InstructionTyp.AND:
                    R = a & b;
                    break;
                case InstructionTyp.OR:
                    R = a | b;
                    break;
                case InstructionTyp.XOR:
                    R = a ^ b;
                    break;
                case InstructionTyp.ADD:
                    try
                    {
                        R = a + b;
                    }
                    catch
                    {
                        //Carry wenn Ergebnis Ueberlauf verursacht
                        Carry = 1;
                    }
                    
                    break;
                case InstructionTyp.SUB:
                    try
                    {
                        R = a - b;
                    }
                    catch
                    {
                        //Carry wenn Ergebnis UEberlauf verursacht
                        Carry = 1;
                    }
                    break;
                case InstructionTyp.SHR:
                    R = a >> b;
                    //TODO Carry haelt letztes rausgeschobenes Bit
                    break;
                case InstructionTyp.SHL:
                    R = a << b;
                    //TODO Carry haelt letztes rausgeschobenes Bit
                    break;
                case InstructionTyp.RR:
                    R = (a >> b) | (a << (32 - b));

                    try
                    {
                        //C haelt letztes rotiertes Bit (Result Bit 31)
                        Carry = Convert.ToInt32(Convert.ToString(R, 2)[31]);
                    }
                    catch { ;}
                    break;
                case InstructionTyp.RL:
                    R = (a << b) | (a >> (32 - b));
                    try
                    {
                        //C haelt letztes rotiertes Bit (Result Bit 0)
                        Carry = Convert.ToInt32(Convert.ToString(R, 2)[0]);
                    }
                    catch { ;}
                    break;
                case InstructionTyp.RRC:
                    //TODO rotate right with carry
                    //result = (a >> b + 1) | (a << (32 - b + 1))
                    break;
                case InstructionTyp.RLC:
                    //TODO rotate left with carry    
                    //result = (a << b + 1) | (a >> (32 - b + 1));
                    break;
            }

            Negative = (R < 0) ? 1 : 0;
            Zero = (R == 0) ? 1 : 0;
        } 

    }
}
