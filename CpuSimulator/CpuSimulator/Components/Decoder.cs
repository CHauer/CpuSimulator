using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            String instructionPart;
            String[] parameters = null;
            InstructionTyp instruction;
            int irCode;

            if (String.IsNullOrEmpty(undecoded))
            {
                //TODO exception Handling
                return null;
            }

            var parts = undecoded.Split(new char[] { ' ' });

            instructionPart = parts[0];

            if (parts.Length > 1)
            {
                parameters = new String[parts.Length - 1];
                Array.Copy(parts, 1, parameters, 0, parameters.Length);
            }

            try
            {
                instruction = (InstructionTyp)Enum.Parse(typeof(InstructionTyp), instructionPart);
            }
            catch (Exception ex)
            {
                //TODO Error handling
                return null;
            }

            irCode = (int)instruction;

            return null;
        }

    }
}
