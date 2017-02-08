using System;
using System.Collections.Generic;
using System.Linq;

namespace HackAssembler
{
    /// <summary>
    /// Basic implementation of IParser
    /// </summary>
    public class BasicParser : IParser
    {
        public List<string> Parse(string assemblyCode)
        {
            // Split contents into a list of lines
            var lines = assemblyCode.Split('\n').ToList();

            RemoveEmptySpace(ref lines);

            ProcessSymbols(ref lines);
                 
            return lines;
        }

        // Removes comments, blank lines and spaces
        private void RemoveEmptySpace(ref List<string> lines)
        {
            // Iterates through the list from the end since since the collection is modified inside the loop
            for (var i = lines.Count - 1; i >= 0; i--)
            {
                // Removes spaces and carriage return symbols
                lines[i] = lines[i].Replace(" ", "").TrimEnd('\r');

                // Finds comments
                var commentPosition = lines[i].IndexOf("//");

                // Removes comment text
                if (commentPosition > 0)
                {
                    lines[i] = lines[i].Remove(commentPosition, lines[i].Length - commentPosition);
                }

                // Removes entire line if it is empty or only consists of a comment
                if (string.IsNullOrWhiteSpace(lines[i]) || commentPosition == 0)
                {
                    lines.Remove(lines[i]);
                }
            }
        }

        // Processes predefined symbols as well as user-defined label and variable symbols
        private void ProcessSymbols(ref List<string> lines)
        {
            // Creates a symbol table that maps label strings to instruction numbers
            var symbolTable = new Dictionary<string, int>();

            // Gets predefined symbols for the hack language
            var predefinedSymbols = System.IO.File.ReadAllLines("predefined.txt");

            foreach (var symbol in predefinedSymbols)
            {
                // Expects a file with symbol names and instruction numbers separated by spaces
                symbolTable.Add(symbol.Split(' ')[0], Convert.ToInt32(symbol.Split(' ')[1]));
            }

            // Creates a list of label line numbers so label lines can be removed after building the symbol table
            var labelLineNumbers = new List<int>();

            // Finds all label declarations
            for (var i = 0; i < lines.Count-1; i++)
            {
                // Checks if a line contains a label declaration
                var label = GetLabel(lines[i]);

                if(label.Length == 0)
                {
                    continue;
                }

                // Adds the label to the symbol table with the corresponding instruction number 
                // The number of lines removed needs to be subtracted from the instruction number in order to be correct
                symbolTable.Add(label, i - labelLineNumbers.Count);
                labelLineNumbers.Add(i);
            }

            // Removes label lines
            for(int i = labelLineNumbers.Count - 1; i >=0; i--)
            {
                lines.RemoveAt(labelLineNumbers[i]);
            }

            // Replaces references to labels with the corresponding instruction number from the symbol table
            foreach(var pair in symbolTable)
            {
                for(int i = 0; i < lines.Count; i++)
                {
                    if(lines[i].TrimStart('@') == pair.Key)
                    {
                        lines[i] = "@" + pair.Value.ToString();
                    }
                }
            }

            // Creates a variable table that maps variable names to memory locations starting at 16
            var variableTable = new Dictionary<string, int>();

            // Finds all variable references
            for (var i = 0; i <lines.Count; i++)
            {
                if (lines[i].Contains("@"))
                {
                    var variable = lines[i].TrimStart('@');

                    // Checks whether the instruction refers directly to a memory location
                    if (IsNumber(variable))
                    {
                        continue;
                    }

                    // Adds the variable to the table and assigns it a memory location when it is referenced for the first time
                    if (!variableTable.ContainsKey(variable))
                    {
                        var memoryAddress = variableTable.Keys.Count + 16;
                        variableTable.Add(variable, memoryAddress);
                    }

                    // Replaces the variable text with the corresponding memory location
                    lines[i] = lines[i].Replace(variable, variableTable[variable].ToString());
                }
            }
        }

        // Returns a boolean value indicating whether a string is a number
        private bool IsNumber(string variable)
        {
            int res;
            return int.TryParse(variable, out res);
        }

        // Returns the name of a label if the line contains a label declaration
        private string GetLabel(string line)
        {
            if(line.Contains("("))
            {
                return line.TrimStart('(').TrimEnd(')');
            }
            else
            {
                return "";
            }
        }
    }
}