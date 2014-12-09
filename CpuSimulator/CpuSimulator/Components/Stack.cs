using System;
using System.Text;

namespace CpuSimulator.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class Stack
    {
        /// <summary>
        /// The stack content
        /// </summary>
        private int[] stackContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stack"/> class.
        /// </summary>
        public Stack()
        {
            SP = 0;
            stackContent = new int[256];
        }

        /// <summary>
        /// Gets the <see cref="System.Int32"/> with the specified offset.
        /// </summary>
        /// <value>
        /// The <see cref="System.Int32"/>.
        /// </value>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">offset;StackPointer out of range.</exception>
        public int this[int offset]
        {
            get
            {
                if ((SP - offset) < 0)
                {
                    throw new ArgumentOutOfRangeException("offset", "StackPointer out of range.");
                }

                //TODO Check if SP - 1 - offset? 
                return stackContent[SP - offset];
            }
        }

        /// <summary>
        /// Gets the bp.
        /// </summary>
        /// <value>
        /// The bp.
        /// </value>
        [Obsolete]
        public int BP
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the sp.
        /// </summary>
        /// <value>
        /// The sp.
        /// </value>
        public int SP
        {
            get;
            private set;
        }

        /// <summary>
        /// Pushes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">SP;StackPointer Overflow</exception>
        public void Push(int item)
        {
            stackContent[SP] = item;

            if (SP >= 255)
            {
                throw new ArgumentOutOfRangeException("SP", "StackPointer Overflow");
            }

            SP++;
        }

        /// <summary>
        /// Pops this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">SP;StackPointer out of range.</exception>
        public int Pop()
        {
            if (SP <= 0)
            {
                throw new ArgumentOutOfRangeException("SP", "StackPointer out of range.");
            }

            return stackContent[--SP];
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

            for (int i = 0; i < SP; i++)
            {
                builder.AppendFormat("{0:000}  {1}  {2}  ({3})",
                                                    i, Convert.ToString(stackContent[i], 2).PadLeft(32, '0'),
                                                    Convert.ToString(stackContent[i], 16).PadLeft(16, '0'),
                                                    stackContent[i]);
                builder.AppendLine();
            }
            builder.AppendLine();

            return builder.ToString();
        }
    }
}
