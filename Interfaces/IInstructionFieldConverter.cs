using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAssembler
{
    /// <summary>
    /// Converts instruction fields from symbolic representation to binary
    /// </summary>
    public interface IInstructionFieldConverter
    {
        /// <summary>
        /// Converts a compute field value from symbolic to binary text
        /// </summary>
        /// <param name="cmpText"></param>
        /// <returns>Binary representation of the compute field</returns>
        string ConvertCmpField(string cmpText);

        /// <summary>
        /// Converts a destination field value from symbolic to binary text
        /// </summary>
        /// <param name="destText"></param>
        /// <returns>Binary representation of the destination field</returns>
        string ConvertDestField(string destText);

        /// <summary>
        /// Converts a jump field value from symbolic to binary text
        /// </summary>
        /// <param name="jmpText"></param>
        /// <returns>Binary representation of the jump field</returns>
        string ConvertJmpField(string jmpText);
    }
}
