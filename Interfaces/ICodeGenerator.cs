using System.Collections.Generic;

namespace HackAssembler
{
    /// <summary>
    /// Translates parsed hack assembly code into machine code
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// Generates binary code from a list of hack commands
        /// </summary>
        /// <param name="commands">A list of hack assembly commands</param>
        /// <returns>List of instructions in binary text format</returns>
        string Generate(List<string> commands);
    }
}