namespace HackAssembler
{
    /// <summary>
    /// Uses an IParser and an ICodeGenerator to convert a hack assembly language text string into machine code and saves it to a file
    /// </summary>
    public class Assembler
    {
        private IParser _parser;
        private ICodeGenerator _generator;

        /// <summary>
        /// Assigns the parser and generator implementation through constructor injection
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="generator"></param>
        public Assembler(IParser parser, ICodeGenerator generator)
        {
            _parser = parser;
            _generator = generator;
        }

        /// <summary>
        /// Assembles machine code and saves the output to a file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileContents"></param>
        public void Assemble(string name, string fileContents)
        {
            // Parse lines
            var lines = _parser.Parse(fileContents);

            // Generate code
            var binaryString = _generator.Generate(lines);

            // Saves to a file with the same name as the input file but with the .hack extension
            System.IO.File.WriteAllText(name +".hack", binaryString);
        }
    }
}