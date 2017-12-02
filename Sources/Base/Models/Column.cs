using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace AlphaCodex.Models
{
    public class Column
    {
        public string Header { get; private set; }
        public List<string> Rows { get; private set; }

        public Column(SQLiteDataReader reader, char separator='|')
        {
            Header = reader["header"] as string;
            var rows = reader["rows"] as string;
            Rows = rows.Split(separator).ToList();
        }

        public Column(string header, List<string> rows)
        {
            Header = header;
            Rows = rows;
        }
    }
}
