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
    }
}
