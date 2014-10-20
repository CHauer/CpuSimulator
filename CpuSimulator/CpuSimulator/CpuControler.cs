using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CpuSimulator
{
    public class CpuControler
    {
        private bool cpuRun;

        #region Constructor

        public CpuControler()
        {
            Initialize();
        }

        #endregion

        #region Initalize

        private void Initialize()
        {
            cpuRun = true;
            Alu = new ALU();
            Ram = new RAM();
            Stack = new Stack();

        }

        #endregion

        #region Properties

        public ProgramROM ProgramRom
        {
            get;
            set;
        }

        public ALU Alu
        {
            get;
            private set;
        }

        public RAM Ram
        {
            get;
            private set;
        }

        public Stack Stack
        {
            get;
            private set;
        }

        public TextWriter Output { get; set; }

        #endregion

        public void RunCpu()
        {
            while (cpuRun)
            {
                //Fetch
                if (!Fetch())
                {
                    cpuRun = false;
                }

                //Decode

                //RAM Read

                //Execute

                //RAM Write
            }
        }

        private bool Fetch()
        {
            bool status = true;

            ProgramRom.IR;
            ProgramRom.PC++;
            return status;
        }
    }
}
