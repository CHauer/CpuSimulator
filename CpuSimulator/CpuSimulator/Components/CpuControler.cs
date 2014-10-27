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

            traceDecode = false;
            traceFetch = false;
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

        /// <summary>
        /// Runs the cpu pipeline.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">ProgrammROM has to be initialized before of the CPU Start.</exception>
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
            int traceProgrammCounter = 0;

            try
            {
                //current instruction for programmROM
                currentUnDecodedInstruction = ProgramRom.IR;

                //save current programm counter
                if (traceFetch)
                {
                    traceProgrammCounter = ProgramRom.PC;
                }

                ProgramRom.PC++;
            }
            catch (Exception ex)
            {
                LogCpuError(ex);
                return false;
            }

            if (traceFetch)
            {
                LogTraceFetch(traceProgrammCounter);
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
                //TODO Prepare ALU register - if needed 
            }
            else if (currentInstruction.GroupType == InstructionGroupTyp.Processor)
            {
                //prepare MAR for RAM Access
                if (currentInstruction.Type == InstructionTyp.MOV)
                {
                    if (currentInstruction.SourceParameter.Type == ParameterTyp.Address)
                    {
                        memoryRead = true;
                        Ram.MAR = currentInstruction.SourceParameter.Content;
                    }
                }
                else if (currentInstruction.Type == InstructionTyp.MOVI)
                {
                    if (currentInstruction.SourceParameter.Type == ParameterTyp.RegisterAddress)
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

            if (traceDecode)
            {
                LogDecodeInstruction();
            }

            return true;
        }

        /// <summary>
        /// Step 3 - RAM read.
        /// </summary>
        /// <returns></returns>
        private bool RamRead()
        {
            if (Ram.MAR != -1 && memoryRead)
            {
                try
                {
                    Ram.Read();
                }
                catch (Exception ex)
                {
                    LogCpuError(ex);
                    return false;
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

            switch (currentInstruction.GroupType)
            {
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
                    HandleJumpInstruction();
                    break;
                case InstructionGroupTyp.Processor:
                    HandleProcessorInstruction();
                    break;
            }

            return returnValue;
        }

        #region Execute Methods

        #region Processor Instructions 

        /// <summary>
        /// Handles the processor instruction.
        /// </summary>
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
                    LogCpuEnd();
                    break;
                case InstructionTyp.PUSH:
                    switch (currentInstruction.ParameterOne.Type)
                    {
                        case ParameterTyp.Register:
                            Stack.Push(registers[currentInstruction.ParameterOne.Register]);
                            break;
                        case ParameterTyp.Data:
                            Stack.Push(currentInstruction.ParameterOne.Content);
                            break;
                    }
                    break;
                case InstructionTyp.POP:
                    registers[currentInstruction.ParameterOne.Register] = Stack.Pop();
                    break;
                case InstructionTyp.CALL:
                    Stack.Push(ProgramRom.PC);
                    ProgramRom.PC = currentInstruction.ParameterOne.Content;
                    break;
                case InstructionTyp.RET:
                    //TODO Check if right?
                    ProgramRom.PC = Stack.Pop();
                    break;
            }
        }

        /// <summary>
        /// Executes the move indirect command.
        /// </summary>
        private void ExecuteMoveIndirectCommand()
        {   
             var sourceType = currentInstruction.SourceParameter.Type;

             if (currentInstruction.TargetParameter.Type == ParameterTyp.RegisterAddress)
             {
                 //MOVI  [Ziel], Quelle 
                 //prepare MAR for RAM Write
                if(sourceType == ParameterTyp.Register)
                {
                    memoryRead = false;
                    Ram.MAR = registers[currentInstruction.TargetParameter.Register];
                    Ram.MDR = registers[currentInstruction.SourceParameter.Register];
                }
                else if (sourceType == ParameterTyp.Data)
                {
                    memoryRead = false;
                    Ram.MAR = registers[currentInstruction.TargetParameter.Register];
                    Ram.MDR = currentInstruction.SourceParameter.Content;
                }
             }
             else
             {
                 //MOVI Ziel, [Quelle]  
                 registers[currentInstruction.TargetParameter.Register] = Ram.MDR;
             }
        }

        /// <summary>
        /// Executes the move command.
        /// </summary>
        private void ExecuteMoveCommand()
        {
            var targetType = currentInstruction.TargetParameter.Type;

            switch (currentInstruction.SourceParameter.Type)
            {
                case ParameterTyp.Register:       
                    if (targetType == ParameterTyp.Register)
                    {
                        registers[currentInstruction.TargetParameter.Register] = registers[currentInstruction.SourceParameter.Register];
                    }
                    else if (targetType == ParameterTyp.Address)
                    {
                        //prepare MAR for RAM Write
                        memoryRead = false;
                        Ram.MAR = currentInstruction.TargetParameter.Content;
                        Ram.MDR = registers[currentInstruction.SourceParameter.Register];
                    }
                    break;

                case ParameterTyp.Data:
                    if (targetType == ParameterTyp.Register)
                    {
                        registers[currentInstruction.TargetParameter.Register] = currentInstruction.SourceParameter.Content;
                    }
                    else if (targetType == ParameterTyp.Address)
                    {
                        //prepare MAR for RAM Write
                        memoryRead = false;
                        Ram.MAR = currentInstruction.TargetParameter.Content;
                        Ram.MDR = currentInstruction.SourceParameter.Content;
                    }
                    break; 

                case ParameterTyp.Address:
                    if (targetType == ParameterTyp.Register)
                    {
                        registers[currentInstruction.TargetParameter.Register] = Ram.MDR;
                    }
                    break;

                case ParameterTyp.StackOffset:
                    //TODO validate MOV from Stack

                    if (targetType == ParameterTyp.Register)
                    {
                        registers[currentInstruction.TargetParameter.Register] = Stack[currentInstruction.SourceParameter.Content];
                    }
                    else if (targetType == ParameterTyp.Address)
                    {
                        memoryRead = false;
                        Ram.MAR = currentInstruction.TargetParameter.Content;
                        Ram.MDR = Stack[currentInstruction.SourceParameter.Content];
                    }
                    break;
            }
        }

        #endregion

        #region Jump Instructions

        /// <summary>
        /// Handles the jump instruction.
        /// </summary>
        private void HandleJumpInstruction()
        {
            switch (currentInstruction.Type)
            {
                case InstructionTyp.JMP:
                    ProgramRom.PC = currentInstruction.ParameterOne.Content;
                    break;
                case InstructionTyp.JR:
                    ProgramRom.PC += currentInstruction.ParameterOne.Content;
                    break;
                case InstructionTyp.JRC:
                    if (Alu.Carry == 1)
                    {
                        ProgramRom.PC += currentInstruction.ParameterOne.Content;
                    }

                    break;
                case InstructionTyp.JRN:
                    if (Alu.Negative == 1)
                    {
                        ProgramRom.PC += currentInstruction.ParameterOne.Content;
                    }

                    break;
                case InstructionTyp.JRZ:
                    if (Alu.Zero == 1)
                    {
                        ProgramRom.PC += currentInstruction.ParameterOne.Content;
                    }
                    break;
            }
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

        /// <summary>
        /// Handles the debug instruction.
        /// </summary>
        private void HandleDebugInstruction()
        {
            switch (currentInstruction.Type)
            {
                case InstructionTyp.TRACE_DECODE:
                    traceDecode = true;
                    break;
                case InstructionTyp.TRACE_FETCH:
                    traceFetch = true;
                    break;
                case InstructionTyp.RDUMP:
                    RegisterDump();
                    break;
                case InstructionTyp.SDUMP:
                    StackDump();
                    break;
                case InstructionTyp.MDUMP:
                    MemoryDump();
                    break;
            }
        }

        #region Dump Methods

        /// <summary>
        /// Dumps the stack content.
        /// </summary>
        private void StackDump()
        {
            if (Output != null)
            {
                //TODO Stack Dump
                Output.Write(Stack.ToString());
            }
        }

        /// <summary>
        /// Dumps the register content.
        /// </summary>
        private void RegisterDump()
        {
            if (Output != null)
            {
                try
                {
                    Output.WriteLine("    {0}  {1}  {2}", "Binary".PadRight(32), "Hex".PadRight(16), "Int");
                    foreach (char key in registers.Keys)
                    {
                        Output.WriteLine("{0}:  {1}  {2}  ({3})", key, Convert.ToString(registers[key], 2).PadLeft(32, '0'), Convert.ToString(registers[key], 16).PadLeft(16, '0'), registers[key]);
                    }

                    Output.WriteLine(); 

                    Output.WriteLine("{0}: {1}", "PC", ProgramRom.PC);
                    Output.WriteLine("{0}: {1}", "IR", ProgramRom.IR);

                    Output.WriteLine("Flags: {0} {1} {2}", Alu.Zero == 1 ? "Z" : string.Empty,
                                                           Alu.Carry == 1 ? "C" : string.Empty,
                                                           Alu.Negative == 1 ? "N" : string.Empty);

                    Output.Flush();
                }
                catch { ;}
            }
        }

        /// <summary>
        /// Dumps the memory content.
        /// </summary>
        private void MemoryDump()
        {
            if (Output != null)
            {
                //TODO Memory Dump
                Output.Write(Ram.ToString());
            }
        }

        #endregion

        #endregion

        #endregion

        /// <summary>
        /// Step 5 - RAM Write
        /// </summary>
        /// <returns></returns>
        private bool RamWrite()
        {
            if (Ram.MAR != -1 && !memoryRead)
            {
                try
                {
                    Ram.Write();
                }
                catch(Exception ex)
                {
                    LogCpuError(ex);
                    return false;
                }
            }

            //Reset MAR
            Ram.MAR = -1;

            return true;
        }

        #region Logging Methods

        /// <summary>
        /// Logs the given exception as cpu error.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private void LogCpuError(Exception ex)
        {
            if (Output != null)
            {
                try
                {
                    Output.WriteLine("### Error ###\n{0}\n{1}",
                        ex.GetType().Name, ex.Message);
                }
                catch { ;}
            }
        }

        /// <summary>
        /// Logs the given exception as cpu error.
        /// </summary>
        private void LogDecodeInstruction()
        {
            if (Output != null)
            {
                try
                {
                    Output.WriteLine(currentInstruction.ToString());
                }
                catch { ;}
            }
        }

        /// <summary>
        /// Logs the trace fetch.
        /// </summary>
        /// <param name="traceProgrammCounter">The trace programm counter.</param>
        private void LogTraceFetch(int traceProgrammCounter)
        {
            if (Output != null)
            {
                try
                {
                    Output.WriteLine("# {0:0000}: {1}", traceProgrammCounter, currentUnDecodedInstruction);
                }
                catch { ;}
            }
        }

        /// <summary>
        /// Logs the cpu end.
        /// </summary>
        private void LogCpuEnd()
        {

            if (Output != null)
            {
                try
                {
                    Output.WriteLine("CPU HALT - RDUMP ".PadRight(32+16+15, '-'));
                    Output.WriteLine();
                }
                catch { ;}
            }

            RegisterDump();
        }

        #endregion
    }
}
