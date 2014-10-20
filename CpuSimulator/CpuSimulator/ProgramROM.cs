using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CpuSimulator
{
    public class ProgramROM
    {
        private string[] commands;
        private string programFilePath;

        private int pc;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramROM" /> class.
        /// </summary>
        public ProgramROM()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            commands = new string[256];
            PC = 0;
        }

        /// <summary>
        /// Reads the programm.
        /// </summary>
        /// <param name="programFile">The program file.</param>
        /// <exception cref="System.IO.InvalidDataException">Programm File ist zu lang! Maximal 256 Befehle sind erlaubt!</exception>
        public void ReadProgramm(string programFile)
        {
            programFilePath = programFile;
            string content = string.Empty;

            try
            {
                StreamReader reader = new StreamReader(File.OpenRead(programFilePath));
                content = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!String.IsNullOrEmpty(content))
            {
                var lines = content.Split(new String[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                if (lines.Length > 256)
                {
                    throw new InvalidDataException("Programm File ist zu lang! Maximal 256 Befehle sind erlaubt!");
                }

                Array.Copy(lines, commands, lines.Length);
            }
            else
            {
                throw new InvalidDataException("Programm File ist ungültig! Keine Befehle vorhanden!");
            }
        }

        public int PC
        {
            get
            {
                return pc;
            }
            set
            {
                if (value > 255)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                pc = value;
            }
        }

        public String IR
        {
            get
            {
                return commands[PC];
            }
        }
    }
}
