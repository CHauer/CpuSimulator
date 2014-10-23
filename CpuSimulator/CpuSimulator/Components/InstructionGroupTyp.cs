using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpuSimulator.Components
{
    public enum InstructionGroupTyp : int
    {
        None = 0,
        Debugging = 1,
        Processor = 2,
        Jump = 3,
        Arithmetic = 4,

    }
}
