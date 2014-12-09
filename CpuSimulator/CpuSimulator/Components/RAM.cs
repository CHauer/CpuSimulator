using System;
using System.Text;

namespace CpuSimulator.Components
{
    public class RAM
    {
        /// <summary>
        /// The memory
        /// </summary>
        private int[] memory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RAM"/> class.
        /// </summary>
        public RAM()
        {
            memory = new int[256];
        }

        /// <summary>
        /// Gets or sets the mar.
        /// </summary>
        /// <value>
        /// The mar.
        /// </value>
        public int MAR
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the MDR.
        /// </summary>
        /// <value>
        /// The MDR.
        /// </value>
        public int MDR
        {
            get;
            set;
        }

        /// <summary>
        /// Reads this instance.
        /// </summary>
        public void Read()
        {
            MDR = memory[MAR];
        }

        /// <summary>
        /// Writes this instance.
        /// </summary>
        public void Write()
        {
            memory[MAR] = MDR;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
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
