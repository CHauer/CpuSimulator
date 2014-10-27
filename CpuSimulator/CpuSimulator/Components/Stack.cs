using System;

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
            return string.Empty;
        }
    }
}
