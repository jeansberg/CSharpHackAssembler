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
        /// Converts a field from symbolic to binary text
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        /// <returns>Binary representation of the compute field</returns>
        string ConvertField(string value, string fieldName);
    }
}
