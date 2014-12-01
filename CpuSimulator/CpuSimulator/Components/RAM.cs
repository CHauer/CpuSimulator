using System;
using System.Text;

namespace CpuSimulator.Components
{
    public class RAM
    {
        private int[] memory;

        public RAM()
        {
            memory = new int[256];
        } 

        public int MAR
        {
            get;
            set;
        }

        public int MDR
        {
            get;
            set;
        }

        public void Read()
        {
            MDR = memory[MAR];
        }

        public void Write()
        {
            memory[MAR] = MDR;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("     {0}  {1}  {2}", "Binary".PadRight(32), "Hex".PadRight(16), "Int");
            builder.AppendLine();

            for (int i = 0; i < 256; i++)
            {
                if (memory[i] != 0)
                {
                    builder.AppendFormat("{0:000}  {1}  {2}  ({3})",
                                                        i, Convert.ToString(memory[i], 2).PadLeft(32, '0'),
                                                        Convert.ToString(memory[i], 16).PadLeft(16, '0'),
                                                        memory[i]);
                    builder.AppendLine();
                }
            }
            builder.AppendLine();

            return builder.ToString();
        }
    }
}
