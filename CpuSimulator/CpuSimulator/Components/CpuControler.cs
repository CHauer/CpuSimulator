using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Indicates if Ram is red or written.
        /// true if RAM is red
        /// false if ram has to be written 
        /// </summary>
        private bool memoryRead;

        /// <summary>
        /// The registers
        /// </summary>
        private Dictionary<char, int> registers;

        /// <summary>
        /// The trace decode flag
        /// show the  decoded instruction info
        /// </summary>
        private bool traceDecode;

        /// <summary>
        /// The trace fetch flag
        /// show the current command
        /// </summary>
        private bool traceFetch;

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

            registers = new Dictionary<char, int>(){{'A', 0},
                                                    {'B', 0},
                                                    {'C', 0},
                                                    {'D', 0},
                                                    {'E', 0},
                                                    {'F', 0},
                                                    {'G', 0},
                                                    {'H', 0}};
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
                if (cpuRun)
                {
                    if (!Decode())
                    {
                        cpuRun = false;
                    }
                }

                //RAM Read
                if (cpuRun)
                {
                    if (!RamRead())
                    {
                        cpuRun = false;
                    }
                }

                //Execute
                if (cpuRun)
                {
                    if (!Execute())
                    {
                        cpuRun = false;
                    }
                }

                //RAM Write
                if (cpuRun)
                {
                    if (!RamWrite())
                    {
                        cpuRun = false;
                    }
                }
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
                LogCpuError(ex);
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
                LogCpuError(ex);
                return false;
            }

            //Reset MAR
            Ram.MAR = -1;

            // Init ALU Registers and MAR
            if (currentInstruction.GroupType == InstructionGroupTyp.Arithmetic)
            {
                //TODO Prepare ALU register?
            }
            else if (currentInstruction.GroupType == InstructionGroupTyp.Processor)
            {
                //prepare MAR for RAM Access
                if (currentInstruction.Type == InstructionTyp.MOV)
                {
                    if (currentInstruction.TargetParameter.Type == ParameterTyp.Address)
                    {
                        memoryRead = false;
                        Ram.MAR = currentInstruction.TargetParameter.Content;
                    }
                    else if (currentInstruction.SourceParameter.Type == ParameterTyp.Address)
                    {
                        memoryRead = true;
                        Ram.MAR = currentInstruction.SourceParameter.Content;
                    }
                }
                else if (currentInstruction.Type == InstructionTyp.MOVI)
                {
                    if (currentInstruction.TargetParameter.Type == ParameterTyp.RegisterAddress)
                    {
                        memoryRead = false;
                        try
                        {
                            Ram.MAR = registers[currentInstruction.TargetParameter.Register];
                        }
                        catch (Exception ex)
                        {
                            LogCpuError(ex);
                            return false;
                        }
                    }
                    else if (currentInstruction.SourceParameter.Type == ParameterTyp.RegisterAddress)
                    {
                        memoryRead = true;
                        try
                        {
                            Ram.MAR = registers[currentInstruction.SourceParameter.Register];
                        }
                        catch (Exception ex)
                        {
                            LogCpuError(ex);
                            return false;
                        }
                    }
                }

            }

            return true;
        }

        /// <summary>
        /// Step 3 - RAM read.
        /// </summary>
        /// <returns></returns>
        private bool RamRead()
        {
            if (Ram.MAR != -1)
            {
                if (memoryRead)
                {
                    Ram.Read();
                }
                else
                {
                    Ram.Write();
                }
            }

            //Reset MAR
            Ram.MAR = -1;

            return true;
        }

        /// <summary>
        /// Step 4 - Execute Instruction
        /// </summary>
        /// <returns></returns>
        private bool Execute()
        {
            bool returnValue = true;

            switch(currentInstruction.GroupType){
                case InstructionGroupTyp.Debugging:
                    HandleDebugInstruction();
                    break;
                case InstructionGroupTyp.Arithmetic:
                    if (!HandleArithmeticInstruction())
                    {
                        returnValue = false;
                    }
                    break;
                case InstructionGroupTyp.Jump:
                    break; 
                case InstructionGroupTyp.Processor:
                    HandleProcessorInstruction();
                    break; 
        }

            return returnValue;
        }

        #region Execute Methods

        #region Processor Instructions 

        private void HandleProcessorInstruction()
        {
            switch (currentInstruction.Type)
            {
                case InstructionTyp.MOV:
                    ExecuteMoveCommand();
                    break;
                case InstructionTyp.MOVI:
                    ExecuteMoveIndirectCommand();
                    break;
                case InstructionTyp.HALT:
                    cpuRun = false;
                    break;
                case InstructionTyp.PUSH:

                    break;
                case InstructionTyp.POP:

                    break;
                case InstructionTyp.CALL:
                    throw new NotImplementedException();
                    break;
                case InstructionTyp.RET:
                    throw new NotImplementedException();
                    break;
            }
        }
        
        private void ExecuteMoveIndirectCommand()
        {
            

        }

        private void ExecuteMoveCommand()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Jump Instructions

        private void HandleJumpInstruction()
        {
        }

        #endregion

        #region ALU Instructions

        /// <summary>
        /// Handles the arithmetic instruction.
        /// </summary>
        private bool HandleArithmeticInstruction()
        {
            try
            {
                Alu.Execute(registers[currentInstruction.ParameterOne.Register],
                            registers[currentInstruction.ParameterTwo.Register],
                            currentInstruction.Type);

                registers['A'] = Alu.R;
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Debug Instructions

        private void HandleDebugInstruction()
        {
            switch (currentInstruction.Type)
            {
                case InstructionTyp.TRACE_DECODE:
                    break;
                case InstructionTyp.TRACE_FETCH:
                    break;
                case InstructionTyp.RDUMP:
                    break;
                case InstructionTyp.SDUMP:
                    break;
                case InstructionTyp.MDUMP:
                    break;
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Step 5 - RAM Write
        /// </summary>
        /// <returns></returns>
        private bool RamWrite()
        {
            if (Ram.MAR != -1)
            {
                if (memoryRead)
                {
                    Ram.Read();
                }
                else
                {
                    Ram.Write();
                }
            }

            //Reset MAR
            Ram.MAR = -1;

            return true;
        }

        /// <summary>
        /// Logs the given exception as cpu error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private void LogCpuError(Exception ex)
        {
            Output.WriteLine("### Error ###\n{0}\n{1}",
                                ex.GetType().Name, ex.Message);
        }

    }
}
