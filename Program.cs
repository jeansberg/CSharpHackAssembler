using System;
using System.IO;

namespace HackAssembler
{
    /// <summary>
    /// Console application that calls the assembler
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point for the application
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Checks number of parameters
            if (args.Length != 1)
            {
                // Tells the user how to correctly call the application
                Console.WriteLine("Usage: HackAssembler <filePath>");
                return;
            }
            else
            {
                // Reads the contents of the input file into a string
                var fileContent = File.ReadAllText(args[0]);

                // Gets the name of the input file 
                var name = Path.GetFileNameWithoutExtension(args[0]);

                // Configures the assembler with a parser and a code generator
                Assembler assembler = new Assembler(new BasicParser(), new BasicCodeGenerator());

                // Calls the Assemble method
                assembler.Assemble(name, fileContent);
            }
        }
    }
}