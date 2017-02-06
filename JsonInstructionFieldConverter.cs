using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HackAssembler
{
    /// <summary>
    /// Implementation of IInstructionFieldConverter that uses json files as lookup tables
    /// </summary>
    public class JsonInstructionFieldConverter : IInstructionFieldConverter
    {
        private string m_path;
        private List<LookupTable> m_lookupTables;

        public JsonInstructionFieldConverter(string path)
        {
            m_path = path;
            m_lookupTables = new List<LookupTable>();
        }

        public string ConvertField(string value, string fieldName)
        {
            if(!m_lookupTables.Exists(x=>x.Name == fieldName))
            {
                LoadTable(fieldName);
            }

            return m_lookupTables.Find(x => x.Name == fieldName).Table[value];
        }

        private void LoadTable(string tableName)
        {
            using (var reader = new StreamReader(m_path + tableName + ".json"))
            {
                var serializer = new JsonSerializer();
                var jsonReader = new JsonTextReader(reader);

                var tableData = (Dictionary<string, string>)serializer.Deserialize(jsonReader, typeof(Dictionary<string, string>));

                m_lookupTables.Add(new LookupTable(tableName, tableData));
            }
        }
    }
}
