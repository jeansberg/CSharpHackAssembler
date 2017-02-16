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
        ISymbolConverter m_replacer;

        public BasicParser(ISymbolConverter converter)
        {
            m_replacer = converter;
        }

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
                m_replacer.AddSymbol(label, "labels", (i - labelLineNumbers.Count).ToString());
                labelLineNumbers.Add(i);
            }

            // Removes label lines
            for(int i = labelLineNumbers.Count - 1; i >=0; i--)
            {
                lines.RemoveAt(labelLineNumbers[i]);
            }

            // Replaces references to labels with the corresponding instruction number from the symbol table

            for(int i = 0; i < lines.Count; i++)
            {
                string instructionNumber;
                if((instructionNumber = m_replacer.ConvertSymbol(lines[i].TrimStart('@'), "labels")) != "")
                {
                    lines[i] = "@" + instructionNumber;
                }
            }

            // Finds all variable references
            var variableCounter = 0;
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
                    if (m_replacer.ConvertSymbol(variable, "variables") == "")
                    {
                        m_replacer.AddSymbol(variable, "variables", (variableCounter + 16).ToString());
                        variableCounter++;
                    }

                    // Replaces the variable text with the corresponding memory location
                    lines[i] = lines[i].Replace(variable, m_replacer.ConvertSymbol(variable, "variables"));
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