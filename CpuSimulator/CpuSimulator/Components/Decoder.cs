using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CpuSimulator.Instructions;

namespace CpuSimulator.Components
{
    public class Decoder
    {
        /// <summary>
        /// The instruction parameter
        /// </summary>
        private Dictionary<InstructionTyp, int> instructionParameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Decoder"/> class.
        /// </summary>
        public Decoder()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            instructionParameter = new Dictionary<InstructionTyp, int>()
            {
                {InstructionTyp.RDUMP,          0},
                {InstructionTyp.TRACE_FETCH,    0},
                {InstructionTyp.TRACE_DECODE,   0},
                {InstructionTyp.MDUMP,          0},
                {InstructionTyp.SDUMP,          0},
                {InstructionTyp.HALT,           0},
                {InstructionTyp.RET,            0},
                
                {InstructionTyp.PUSH,           1},
                {InstructionTyp.POP,            1},
                {InstructionTyp.CALL,           1},
                {InstructionTyp.JMP,            1},
                {InstructionTyp.JR,             1},
                {InstructionTyp.JRC,            1},
                {InstructionTyp.JRZ,            1},
                {InstructionTyp.JRN,            1},

                {InstructionTyp.MOV,            2},
                {InstructionTyp.MOVI,           2},
                {InstructionTyp.AND,            2},
                {InstructionTyp.OR,             2},
                {InstructionTyp.XOR,            2},
                {InstructionTyp.ADD,            2},
                {InstructionTyp.SUB,            2},
                {InstructionTyp.SHR,            2},
                {InstructionTyp.SHL,            2},
                {InstructionTyp.RR,             2},
                {InstructionTyp.RL,            2},
                {InstructionTyp.RRC,            2},
                {InstructionTyp.RLC,            2},
            };
        }

        public Instruction DecodeInstruction(string undecoded)
        {
            String instructionPart = string.Empty;
            String[] parameters = new String[0];
            InstructionTyp irCode;

            if (String.IsNullOrEmpty(undecoded))
            {
                throw new ArgumentNullException("undecoded");
            }

            if (undecoded.Contains(' '))
            {
                instructionPart = undecoded.Substring(0, undecoded.IndexOf(' '));

                String parameterPart = undecoded.Substring(undecoded.IndexOf(' ') + 1).Trim();

                parameters = parameterPart.Split(new char[] { ',' });
            }

            try
            {
                irCode = (InstructionTyp)Enum.Parse(typeof(InstructionTyp), instructionPart);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(String.Format("Unknown Command {0}.", instructionPart));
            }

            //Create Instruction obejct and set parameters
            var instruction = new Instruction()
            {
                PlainInstruction = undecoded,
                Type = irCode
            };

            if (!instructionParameter[irCode].Equals(parameters.Length))
            {
                throw new InvalidOperationException(String.Format("Wrong count of parameters for command {0} - expected {1}.", 
                    instructionPart, instructionParameter[irCode]));
            }

            if (instructionParameter[irCode] == 1)
            {
                instruction.Parameter = CreateParameter(parameters[0]);
            }
            else
            {
                instruction.TargetParameter = CreateParameter(parameters[0]);
                instruction.SourceParameter = CreateParameter(parameters[1]);
            }

            //TODO check if instruction is valid 
            //ValidateInstruction();


            return null;
        }

        private bool ValidateInstruction(Instruction instruction)
        {
            return false;
        }

        private Parameter CreateParameter(string parameter )
        {
            int content = 0;
            Parameter para = new Parameter();

            if (parameter.StartsWith("#"))
            {
                para.Type = ParameterTyp.Data;
                para.Content = Convert.ToInt32(parameter.Substring(1));
            }
            else if (parameter.StartsWith("$"))
            {
                para.Type = ParameterTyp.StackOffset;
                para.Content = Convert.ToInt32(parameter.Substring(1));
            }
            else if (parameter.StartsWith("@"))
            {
                para.Type = ParameterTyp.Address;
                para.Content = Convert.ToInt32(parameter.Substring(1));
            }
            else
            {
                para.Type = ParameterTyp.Register;
                para.Content = Char.ToUpper(parameter[0]);
            }

            return para;
        }

    }
}
