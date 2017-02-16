namespace HackAssembler
{
    /// <summary>
    /// Looks up and saves symbols names and associated integers
    /// </summary>
    public interface ISymbolConverter
    {
        /// <summary>
        /// Converts a symbol to its associated integer
        /// </summary>
        /// <param name="key">The name of the symbol</param>
        /// <param name="type">The type of the symbol</param>
        /// <returns>The associated integer</returns>
        string ConvertSymbol(string key, string type);

        /// <summary>
        /// Adds a symbol with a specified name, type and value
        /// </summary>
        /// <param name="key">The name of the symbol</param>
        /// <param name="type">The type of the symbol</param>
        /// <param name="value">The integer value</param>
        void AddSymbol(string key, string type, string value);
    }
}
