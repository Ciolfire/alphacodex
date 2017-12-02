using System.Data.SQLite;
using System.Linq;

namespace AlphaCodex
{
    public class Favorite
    {
        #region Fields
        #endregion

        #region Properties
        public int Id { get; set; }
        public string Group { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        #endregion
        
        #region Constructor
        public Favorite(SQLiteDataReader reader)
        {
            Id = (int)(long)reader["id_favorite"];
            Group = reader["name"] as string;
            Type = reader["type"] as string;
            Name = string.Empty;
        }

        //public Favorite(int id, string type)
        //{
        //    Type = type;
        //    Id = id;
        //    Name = "lol";
        //    switch (type)
        //    {
        //        case "feat":
        //            Name = "??";//MainWindow.Feats.SingleOrDefault(feat => feat.Id == id).Name;
        //            break;
        //        default:
        //            Name = "???";
        //            break;
        //    }
        //}
        #endregion
        #region Methods
        #endregion
    }
}