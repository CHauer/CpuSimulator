using System;
using CpuSimulator.Instructions;
using System.Collections;

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
                        Carry = 0;
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
                        Carry = 0;
                    }
                    catch
                    {
                        //Carry wenn Ergebnis UEberlauf verursacht
                        Carry = 1;
                    }
                    break;
                case InstructionTyp.SHR:
                    R = a >> b;
                    //Carry haelt letztes rausgeschobenes Bit
                    try
                    {
                        Carry = Convert.ToString(a, 2).ToCharArray()[(b - 1)];
                    }
                    catch { ;}
                    break;
                case InstructionTyp.SHL:
                    R = a << b;
                    //Carry haelt letztes rausgeschobenes Bit
                    try
                    {
                        string bits = Convert.ToString(a, 2);
                        Carry = bits.ToCharArray()[bits.Length - b];
                    }
                    catch { ;}
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
                    //rotate right with carry
                    //result = (a >> b + 1) | (a << (32 - b + 1))
                    R = RotateRightWithCarry(a, b);
                    break;
                case InstructionTyp.RLC:
                    // rotate left with carry    
                    //result = (a << b + 1) | (a >> (32 - b + 1));
                    R = RotateLeftWithCarry(a, b);
                    break;
                case InstructionTyp.DIV:
                    double d = (double)a / b;
                    int even = (int)d;

                    Carry = 0;
                    if (d - even > 0)
                    {
                        Carry = 1;
                    }

                    //int / int => int 
                    R = a / b;
                    break;
                case InstructionTyp.MUL:
                    try
                    {
                        R = a * b;
                        Carry = 0;
                    }
                    catch 
                    {
                        Carry = 1;
                    }
                    
                    break;
            }

            Negative = (R < 0) ? 1 : 0;
            Zero = (R == 0) ? 1 : 0;
        }


        /// <summary>
        /// Rotates the right with carry.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        private int RotateRightWithCarry(int a, int b)
        {
            char[] bits = new char[32];
            char[] movedbits = new char[32];

            for (int i = 0; i < bits.Length; i++)
            {
                bits[i] = '0';
                movedbits[i] = '0';
            }

            var temp = Convert.ToString(a, 2).ToCharArray();
            temp.CopyTo(bits, 32 - temp.Length);

            for (int i = 0; i < b; i++)
            {
                movedbits[0] = Carry == 1 ? '1' : '0';
                Carry = Convert.ToInt32(bits[bits.Length - 1].ToString());
                Array.Copy(bits, 0, movedbits, 1, bits.Length - 1);
                bits = movedbits;
            }

            return Convert.ToInt32(new string(bits), 2);
        }

        /// <summary>
        /// Rotates the left with carry.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        private int RotateLeftWithCarry(int a, int b)
        {
            char[] bits = new char[32];
            char[] movedbits = new char[32];

            for (int i = 0; i < bits.Length; i++)
            {
                bits[i] = '0';
                movedbits[i] = '0';
            }

            var temp = Convert.ToString(a, 2).ToCharArray();
            temp.CopyTo(bits, 32 - temp.Length);

            for (int i = 0; i < b; i++)
            {
                movedbits[movedbits.Length - 1] = Carry == 1 ? '1' : '0';
                Carry = Convert.ToInt32(bits[bits.Length - 1].ToString());
                Array.Copy(bits, 1, movedbits, 0, bits.Length - 1);
                bits = movedbits;
            }

            return Convert.ToInt32(new string(bits), 2);
        }
    }
}
