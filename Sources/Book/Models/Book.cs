using System.Data.SQLite;

namespace AlphaCodex.Models
{
    public class Book
    {
        #region  Fields
        public int Id { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public string Setting { get; set; }
        public string Cover { get; set; }
        #endregion

        #region constructor
        public Book(SQLiteDataReader reader)
        {
            Id = (int)(long)reader["id"];
            Name = reader["name"].ToString();
            Reference = reader["reference"].ToString();
            Setting = reader["setting"].ToString();
            string cover = reader["cover"].ToString();
            if (string.IsNullOrEmpty(cover))
            {
                cover = "none.jpg";
            }
            Cover = "/AlphaCodex;component/data/pictures/UI/thumbnails/books/" + cover;
        }
        #endregion
    }
}