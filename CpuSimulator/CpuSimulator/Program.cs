using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpuSimulator
{
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            String programFile = String.Empty;

            if (args.Length > 0)
            {
                programFile = args[0];

                if (!File.Exists(programFile))
                {
                    programFile = string.Empty;
                }
            }

            if (String.IsNullOrEmpty(programFile))
            {
                programFile = ReadProgramFilePath();
            }

            CpuControler controller = new CpuControler();
            //For redirecting output to file
            controller.Output = Console.Out;
            
            //Load Programm 
            var program = new ProgramROM();
            program.ReadProgramm(programFile);
            //TODO Error Handling

            //load programm into controller
            controller.ProgramRom = program;

            //Start Programm, Cpu
            controller.RunCpu();
        }

        /// <summary>
        /// Reads the program file path from Console.In
        /// </summary>
        /// <returns></returns>
        private static string ReadProgramFilePath()
        {
            string path;

            do
            {
                Console.Write("Bitte geben Sie ein gültiges Program File an:");
                path = Console.ReadLine();
            }
            while (!File.Exists(path));

            return path;
        }
    }
}
