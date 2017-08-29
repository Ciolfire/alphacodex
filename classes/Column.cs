using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace AlphaCodex
{
    public class Column
    {
        public string header { get; private set; }
        public List<string> rows { get; private set; }

        public Column(SQLiteDataReader reader, char separator='|')
        {
            header = reader["header"] as string;
            var tmp = reader["rows"] as string;
            rows = tmp.Split(separator).ToList();
        }

        public Column(string header, List<string> rows)
        {
            this.header = header;
            this.rows = rows;
        }
    }
}
