using System.Collections.Generic;

namespace HackAssembler
{
    /// <summary>
    /// Translates parsed hack assembly code into machine code
    /// </summary>
    interface ICodeGenerator
    {
        /// <summary>
        /// Transkates a list of hack commands into binary format
        /// </summary>
        /// <param name="commands"></param>
        /// <returns>List of instructions in binary text format</returns>
        string Generate(List<string> commands);
    }
}