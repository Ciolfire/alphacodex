using System.Data.SQLite;

namespace AlphaCodex
{
    public class Alignment
    {
        // Properties
        public int id { get; set; }
        public string name { get; set; }
        public string shortName {get;set;}
        public string description { get; set; }

        //constructor
        public Alignment(SQLiteDataReader reader)
        {
            id = (int)(long)reader["id"];
            name = reader["name"] as string;
            shortName = reader["short"] as string;
            description = reader["description"] as string;
        }

        // Methods //
    }
}