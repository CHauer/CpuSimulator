using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CpuSimulator.Components;

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

            //create instance of the cpu controler
            CpuControler controller = new CpuControler();

            //For redirecting output to file
            controller.Output = Console.Out;
            
            //Load Programm rom object
            var program = new ProgramROM();
            try
            {
                //read programm instructions in programmrom
                program.ReadProgramm(programFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return;
            }

            //load programm into controller
            controller.ProgramRom = program;

            try
            {
                //Start Programm, Cpu
                controller.RunCpu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            //wait for close
            Console.ReadLine();
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
