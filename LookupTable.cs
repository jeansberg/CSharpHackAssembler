using System.Collections.Generic;

namespace HackAssembler
{
    /// <summary>
    /// Wraps a dictionary object containing symbolic field values and their binary form for a specific field
    /// </summary>
    public class LookupTable
    {
        private string m_name;

        private Dictionary<string, string> m_table;

        public LookupTable(string name, Dictionary<string, string> table)
        {
            m_name = name;
            m_table = table;
        }

        public string Name
        {
            get
            {
                return m_name;
            }

            set
            {
                m_name = value;
            }
        }

        public Dictionary<string, string> Table
        {
            get
            {
                return m_table;
            }

            set
            {
                m_table = value;
            }
        }
    }
}
