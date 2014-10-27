using System;
using System.Text;

namespace CpuSimulator.Components
{
    public class Stack
    {
        private int[] stackContent;

        public Stack()
        {
            SP = 0;
            stackContent = new int[256];
        }

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

        [Obsolete]
        public int BP
        {
            get;
            private set;
        }

        public int SP
        {
            get;
            private set;
        }

        public void Push(int item)
        {
            stackContent[SP] = item;

            if (SP >= 255)
            {
                throw new ArgumentOutOfRangeException("SP", "StackPointer Overflow");
            }

            SP++;
        }

        public int Pop()
        {
            if (SP <= 0)
            {
                throw new ArgumentOutOfRangeException("SP", "StackPointer out of range.");
            }

            return stackContent[--SP];
        }

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
