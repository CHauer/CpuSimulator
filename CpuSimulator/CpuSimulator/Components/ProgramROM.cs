using System;
using System.IO;
using System.Linq;

namespace CpuSimulator.Components
{
    public class ProgramROM
    {
        private string[] instructions;
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
            instructions = new string[256];
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
                //all lines of the programm 
                var linesRaw = content.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                
                //seperate comments "//" out of raw lines
                var linesWithoutComments = linesRaw.Select(line => line.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)[0]).ToList();


                //remove empty lines
                var lines = linesWithoutComments.Where(line => !String.IsNullOrEmpty(line))
                                                .Select(line => line.Trim()).ToList(); 

                if (lines.Count > 256)
                {
                    throw new InvalidDataException("Programm File ist zu lang! Maximal 256 Befehle sind erlaubt!");
                }

                Array.Copy(lines.ToArray(), instructions, lines.Count);
            }
            else
            {
                throw new InvalidDataException("Programm File ist ungültig! Keine Befehle vorhanden!");
            }
        }

        /// <summary>
        /// Gets or sets the programm counter.
        /// Points to the next readable instruction in IR.
        /// </summary>
        /// <value>
        /// The pc.
        /// </value>
        /// <exception cref="System.ArgumentOutOfRangeException">value</exception>
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

        /// <summary>
        /// Gets the current instruction on the position of the ProgrammCounter PC.
        /// </summary>
        /// <value>
        /// The instruction.
        /// </value>
        public String IR
        {
            get
            {
                return instructions[PC];
            }
        }
    }
}
