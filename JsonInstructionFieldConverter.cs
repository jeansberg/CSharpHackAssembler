using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace HackAssembler
{
    /// <summary>
    /// Implementation of IInstructionFieldConverter that uses json files as lookup tables
    /// </summary>
    public class JsonInstructionFieldConverter : IInstructionFieldConverter
    {
        private string m_path;
        private List<LookupTable> m_lookupTables;

        // Takes an optional path parameter if files are not in the executable folder
        public JsonInstructionFieldConverter(string path = "")
        {
            m_path = path;
            m_lookupTables = new List<LookupTable>();
        }

        public string ConvertField(string fieldValue, string fieldName)
        {
            // Loads the table into memory if it hasn't been done
            if(!m_lookupTables.Exists(x=>x.Name == fieldName))
            {
                LoadTable(fieldName);
            }

            // Returns the binary value
            return m_lookupTables.Find(x => x.Name == fieldName).Table[fieldValue];
        }

        // Loads a symbol table with a specified name into memory
        private void LoadTable(string tableName)
        {
            try
            {
            using (var reader = new StreamReader(m_path + tableName + ".json"))
            {
                // Deserialize the file contents into a dictionary and add it to the list
                var serializer = new JsonSerializer();
                var jsonReader = new JsonTextReader(reader);

                var tableData = (Dictionary<string, string>)serializer.Deserialize(jsonReader, typeof(Dictionary<string, string>));

                m_lookupTables.Add(new LookupTable(tableName, tableData));
            }
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Could not load symbol table", e);
            }
        }
    }
}
