using System.Data.SQLite;

namespace AlphaCodex
{
    public class Book
    {
        // Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public string Cover { get; set; }

        //constructor
        public Book(SQLiteDataReader reader)
        {
            Id = (int)(long)reader["id"];
            Name = reader["name"].ToString();
            Reference = reader["reference"].ToString();
            Cover = reader["cover"].ToString();
        }

        // Methods //
    }
}