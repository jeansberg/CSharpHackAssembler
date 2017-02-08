using System.Collections.Generic;

namespace HackAssembler
{
    /// <summary>
    /// Parses hack assembly code
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Parses a text string containing hack assembly code and returns a list of assembly language commands that can be directly translated into machine code
        /// </summary>
        /// <param name="assemblyCode">A string of assembly code</param>
        /// <returns>List of commands</returns>
        List<string> Parse(string assemblyCode);
    }
}