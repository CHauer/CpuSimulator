using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CpuSimulator
{
    public class CpuControler
    {
        /// <summary>
        /// Indicates if the cpu is running /
        ///  processing instructions from the programm rom
        /// </summary>
        private bool cpuRun;

        /// <summary>
        /// The current instruction
        /// </summary>
        private string currentUnDecodedInstruction;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CpuControler"/> class.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the program rom.
        /// </summary>
        /// <value>
        /// The program rom.
        /// </value>
        public ProgramROM ProgramRom
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the alu.
        /// </summary>
        /// <value>
        /// The alu.
        /// </value>
        public ALU Alu
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ram.
        /// </summary>
        /// <value>
        /// The ram.
        /// </value>
        public RAM Ram
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the stack.
        /// </summary>
        /// <value>
        /// The stack.
        /// </value>
        public Stack Stack
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public TextWriter Output { get; set; }

        #endregion

        public void RunCpu()
        {
            if (ProgramRom == null)
            {
                return;
            }

            while (cpuRun)
            {
                //Fetch
                if (!Fetch())
                {
                    cpuRun = false;
                }

                //Decode
                if (!Decode())
                {

                }

                //RAM Read


                //Execute

                //RAM Write
            }
        }

        private bool Fetch()
        {
            
            try
            {
                //current instruction for programmROM
                currentUnDecodedInstruction = ProgramRom.IR;
                
                //PC++
                ProgramRom.PC++;
            }
            catch (Exception ex)
            {
                //TODO error Logging
                return false;
            }

            return true;
        }

        private bool Decode()
        {
            String instructionPart;
            String[] parameters;
            InstructionTyp instruction;

            if (String.IsNullOrEmpty(currentUnDecodedInstruction))
            {
                //TODO exception Handling
                return false;
            }
            else
            {
                if (currentUnDecodedInstruction.Contains(' '))
                {
                    var parts = currentUnDecodedInstruction.Split(new char[] { ' ' });
                    instructionPart = parts[0];

                    if (parts.Length > 1)
                    {
                        //parameters = parts.
                    }
                }
                else
                {
                    instructionPart = currentUnDecodedInstruction;
                }
            }

            try
            {
                instruction = (InstructionTyp)Enum.Parse(typeof(InstructionTyp), currentUnDecodedInstruction);
            }
            catch (Exception ex)
            {
                //TODO Error handling
                return false;
            }

            switch (instruction)
            {

            }

            return true;
        }
    }
}
