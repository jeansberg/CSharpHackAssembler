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
        /// <param name="fieldValue">The symbolic representation of the field</param>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>Binary representation of the compute field</returns>
        string ConvertField(string fieldValue, string fieldName);
    }
}
