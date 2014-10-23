using System;
using System.IO;
using System.Linq;
using CpuSimulator.Instructions;

namespace CpuSimulator.Components
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

        /// <summary>
        /// The current cpu instruction
        /// </summary>
        private Instruction currentInstruction;

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

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            cpuRun = true;
            Alu = new ALU();
            Ram = new RAM();
            Stack = new Stack();
            Decoder = new Decoder();
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
        /// Gets the decoder.
        /// </summary>
        /// <value>
        /// The decoder.
        /// </value>
        public Decoder Decoder
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
                throw new InvalidOperationException("ProgrammROM has to be initialized before of the CPU Start.");
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
                //if(!ReadRAM()){
                //}

                //Execute
                if (!Execute())
                {

                }

                //RAM Write
                //if(!ReadWrite()){
                //}
            }
        }

        /// <summary>
        /// Step 1 - Fetch new instruction from ProgramRom
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Step 2 - Decodes current Instruction
        /// </summary>
        /// <returns></returns>
        private bool Decode()
        {
            try
            {
                currentInstruction = Decoder.DecodeInstruction(currentUnDecodedInstruction);
                if (currentInstruction == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //TODO errorlogging
                return false;
            }

            //Reset MAR
            Ram.MAR = -1;

            //TODO Init ALU Registers and MAR
            //if (currentInstruction.GroupType == InstructionGroupTyp.Arithmetic)
            //{
            //    //Prepare input for alu
            //    switch (currentInstruction.Type)
            //    {

            //    }
                
            //}
            //else if (currentInstruction.GroupType == InstructionGroupTyp.Processor)
            //{
            //    //prepare MAR for RAM Access

            //}

            return true;
        }

        /// <summary>
        /// Step 4 - Execute Instruction
        /// </summary>
        /// <returns></returns>
        private bool Execute()
        {
            throw new NotImplementedException();
        }

    }
}
