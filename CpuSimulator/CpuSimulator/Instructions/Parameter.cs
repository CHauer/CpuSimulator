using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpuSimulator.Instructions
{
    /// <summary>
    /// 
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public int Content
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
        public ParameterTyp Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the register.
        /// </summary>
        /// <value>
        /// The register.
        /// </value>
        public char Register
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            switch (Type)
            {
                case ParameterTyp.Address:
                case ParameterTyp.Data:
                case ParameterTyp.StackOffset:
                    return String.Format("Typ: {0} Value: {1}", Type.ToString("g").PadRight(10), Content.ToString().PadRight(15));
                case ParameterTyp.Register:
                case ParameterTyp.RegisterAddress:
                    return String.Format("Typ: {0} Value: {1}", Type.ToString("g").PadRight(10), Register.ToString().PadRight(15));
            }

            return string.Empty;
        }
    }
}
