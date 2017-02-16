using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;

namespace HackAssembler
{
    /// <summary>
    /// Implementation of IInstructionFieldConverter that uses json files as lookup tables
    /// </summary>
    public class JsonLookupHandler : ISymbolConverter
    {
        private string m_path;
        private List<LookupTable> m_lookupTables;

        // Takes an optional path parameter if files are not in the executable folder
        public JsonLookupHandler(string path = "")
        {
            m_path = path;
            m_lookupTables = new List<LookupTable>();
        }

        public string ConvertSymbol(string key, string type)
        {
            // Loads the table into memory if it hasn't been done
            if(!m_lookupTables.Exists(x=>x.Name == type))
            {
                LoadTable(type);
            }

            var table = m_lookupTables.Find(x => x.Name == type).Table;

            // Returns the value if found
            if(table.ContainsKey(key))
            {
                return table[key];
            }
            else
            {
                return "";
            }
        }

        // Loads a symbol table with a specified name into memory
        private void LoadTable(string type)
        {
            var fileName = m_path + type + ".json";
            if (!File.Exists(fileName))
            {
                m_lookupTables.Add(new LookupTable(type, new Dictionary<string, string>()));
                return;
            }

            try
            {
                using (var reader = new StreamReader(m_path + type + ".json"))
                {
                    // Deserialize the file contents into a dictionary and add it to the list
                    var serializer = new JsonSerializer();
                    var jsonReader = new JsonTextReader(reader);

                    var tableData = (Dictionary<string, string>)serializer.Deserialize(jsonReader, typeof(Dictionary<string, string>));

                    m_lookupTables.Add(new LookupTable(type, tableData));
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Could not load lookup table", e);
            }
        }

        public void AddSymbol(string key, string type, string value)
        {
            // Loads the table into memory if it hasn't been done
            if (!m_lookupTables.Exists(x => x.Name == type))
            {
                LoadTable(type);
            }

            // Adds the symbol
            m_lookupTables.Find(x => x.Name == type).Table.Add(key, value);
        }
    }
}
