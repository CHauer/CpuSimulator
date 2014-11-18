using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CpuSimulator.Instructions;

namespace CpuSimulator.Components
{
    public class Decoder
    {
        /// <summary>
        /// The instruction parameter
        /// </summary>
        private Dictionary<InstructionTyp, int> instructionParameter;
        private Dictionary<InstructionTyp, string> instructionValidationPattern;

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
                {InstructionTyp.RL,             2},
                {InstructionTyp.RRC,            2},
                {InstructionTyp.RLC,            2},
                {InstructionTyp.DIV,            2},
                {InstructionTyp.MUL,            2}
            };

            instructionValidationPattern = new Dictionary<InstructionTyp, string>()
            {
               
                {InstructionTyp.PUSH,           @"PUSH\s+(\#\d+|[A-H])"},
                {InstructionTyp.POP,            @"POP\s+[A-H]"},
                {InstructionTyp.CALL,           @"CALL\s+\@\d+"},
                {InstructionTyp.JMP,            @"JMP\s+\@\d+"},
                {InstructionTyp.JR,             @"JR\s+\#\d+"},
                {InstructionTyp.JRC,            @"JRC\s+\#\d+"},
                {InstructionTyp.JRZ,            @"JRZ\s+\#\d+"},
                {InstructionTyp.JRN,            @"JRN\s+\#\d+"},

                {InstructionTyp.MOV,           @"MOV\s+([A-H]|\@\d+)\s*,\s*(\#\d+|[A-H]|\@\d+|\$\-\d+)"},
                {InstructionTyp.MOVI,          @"MOVI\s+(\[[A-H]\]\s*,\s*(\#\d+|[A-H])|[A-H]\s*,\s*\[[A-H]\])"},
                {InstructionTyp.AND,           @"AND\s+[A-H]\s*,\s*[A-H]"},
                {InstructionTyp.OR,            @"OR\s+[A-H]\s*,\s*[A-H]"},
                {InstructionTyp.XOR,           @"XOR\s+[A-H]\s*,\s*[A-H]"},
                {InstructionTyp.ADD,           @"ADD\s+[A-H]\s*,\s*[A-H]"},
                {InstructionTyp.SUB,           @"SUB\s+[A-H]\s*,\s*[A-H]"},
                {InstructionTyp.SHR,           @"SHR\s+[A-H]\s*,\s*[A-H]"},
                {InstructionTyp.SHL,           @"SHL\s+[A-H]\s*,\s*[A-H]"},
                {InstructionTyp.RR,            @"RR\s+[A-H]\s*,\s*[A-H]"},
                {InstructionTyp.RL,            @"RL\s+[A-H]\s*,\s*[A-H]"},
                {InstructionTyp.RRC,           @"RRC\s+[A-H]\s*,\s*[A-H]"},
                {InstructionTyp.RLC,           @"RLC\s+[A-H]\s*,\s*[A-H]"},
                {InstructionTyp.DIV,           @"DIV\s+[A-H]\s*,\s*[A-H]"},
                {InstructionTyp.MUL,           @"MUL\s+[A-H]\s*,\s*[A-H]"},
            };
        }

        /// <summary>
        /// Decodes the given instruction line.
        /// </summary>
        /// <param name="undecoded">The undecoded.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">undecoded</exception>
        /// <exception cref="System.InvalidOperationException">
        /// </exception>
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
            else
            {
                instructionPart = undecoded;
            }

            try
            {
                irCode = (InstructionTyp)Enum.Parse(typeof(InstructionTyp), instructionPart);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(String.Format("Unknown Command {0}.", instructionPart));
            }

            //check if instruction is valid 
            if (!ValidateInstruction(irCode, undecoded))
            {
                throw new InvalidOperationException(String.Format("The command {0} is invalid.", undecoded));
            }

            //Create Instruction obejct and set parameters
            var instruction = new Instruction()
            {
                PlainInstruction = undecoded,
                Type = irCode
            };

            if (instructionParameter[irCode] != parameters.Length)
            {
                throw new InvalidOperationException(String.Format("Wrong count of parameters for command {0} - expected {1}.", 
                    instructionPart, instructionParameter[irCode]));
            }

            if (instructionParameter[irCode] > 0)
            {
                if (instructionParameter[irCode] == 1)
                {
                    instruction.ParameterOne = CreateParameter(parameters[0].Trim());
                }
                else
                {
                    instruction.TargetParameter = CreateParameter(parameters[0].Trim());
                    instruction.SourceParameter = CreateParameter(parameters[1].Trim());
                }
            }

            return instruction;
        }

        /// <summary>
        /// Validates the instruction.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="undecoded">The undecoded.</param>
        /// <returns></returns>
        private bool ValidateInstruction(InstructionTyp type, string undecoded)
        {
            if (instructionParameter[type] == 0)
            {
                return true;
            }

            try
            {
                if (!Regex.IsMatch(undecoded.Trim(), instructionValidationPattern[type]))
                {
                    return false;
                }
            }
            catch { return false; }

            return true;
        }

        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private Parameter CreateParameter(string parameter)
        {
            int content = 0;
            Parameter para = new Parameter();


            if (parameter.StartsWith("#"))
            {
                para.Type = ParameterTyp.Data;
                para.Content = Convert.ToInt32(parameter.Substring(1));
            }
            else if (parameter.StartsWith("$-"))
            {
                para.Type = ParameterTyp.StackOffset;
                para.Content = Convert.ToInt32(parameter.Substring(2));
            }
            else if (parameter.StartsWith("@"))
            {
                para.Type = ParameterTyp.Address;
                para.Content = Convert.ToInt32(parameter.Substring(1));
            }
            else if (parameter.StartsWith("["))
            {
                para.Type = ParameterTyp.RegisterAddress;
                para.Register = Char.ToUpper(parameter[1]);
            }
            else
            {
                para.Type = ParameterTyp.Register;
                para.Register = Char.ToUpper(parameter[0]);
            }

            return para;
        }

    }
}
