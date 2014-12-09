using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpuSimulator.Instructions
{
    public class Instruction
    {
        /// <summary>
        /// The parameter one
        /// </summary>
        private Parameter parameterOne;
        /// <summary>
        /// The parameter two
        /// </summary>
        private Parameter parameterTwo;

        /// <summary>
        /// Initializes a new instance of the <see cref="Instruction"/> class.
        /// </summary>
        public Instruction()
        {
        }

        /// <summary>
        /// Gets or sets the plain instruction.
        /// </summary>
        /// <value>
        /// The plain instruction.
        /// </value>
        public string PlainInstruction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public InstructionTyp Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the type of the group.
        /// </summary>
        /// <value>
        /// The type of the group.
        /// </value>
        public InstructionGroupTyp GroupType
        {
            get
            {
                try
                {
                    if ((int)Type >= 10 && (int)Type <= 14)
                    {
                        return InstructionGroupTyp.Debugging;
                    }
                    else if ((int)Type >= 20 && (int)Type <= 26)
                    {
                        return InstructionGroupTyp.Processor;
                    }
                    else if ((int)Type >= 30 && (int)Type <= 34)
                    {
                        return InstructionGroupTyp.Jump;
                    }
                    else if ((int)Type >= 40)
                    {
                        return InstructionGroupTyp.Arithmetic;
                    }
                }
                catch { ;}

                return InstructionGroupTyp.None;
            }
        }

        /// <summary>
        /// Gets or sets the source parameter.
        /// </summary>
        /// <value>
        /// The source parameter.
        /// </value>
        public Parameter SourceParameter
        {
            get
            {
                return parameterTwo;
            }
            set
            {
                parameterTwo = value;
            }
        }

        /// <summary>
        /// Gets or sets the target parameter.
        /// </summary>
        /// <value>
        /// The target parameter.
        /// </value>
        public Parameter TargetParameter
        {
            get
            {
                return parameterOne;
            }
            set
            {
                parameterOne = value;
            }
        }

        /// <summary>
        /// Gets or sets the parameter one.
        /// </summary>
        /// <value>
        /// The parameter one.
        /// </value>
        public Parameter ParameterOne
        {
            get
            {
                return parameterOne;
            }
            set
            {
                parameterOne = value;
            }
        }

        /// <summary>
        /// Gets or sets the parameter two.
        /// </summary>
        /// <value>
        /// The parameter two.
        /// </value>
        public Parameter ParameterTwo
        {
            get
            {
                return parameterTwo;
            }
            set
            {
                parameterTwo = value;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            //Instruction
            builder.AppendLine(String.Format(@"    - Instructiontyp: {0} ({1})", Type.ToString("g"), (int)Type));

            if (parameterOne != null)
            {
                builder.Append(String.Format("       Parameter 1:   {0}", parameterOne.ToString()));
            }
            if (parameterTwo != null)
            {
                builder.AppendLine();
                builder.Append(String.Format("       Parameter 2:   {0}", parameterTwo.ToString()));
            }

            return builder.ToString();
        }
    }
}
