using AlphaCodex.ViewModels;
using System.Data.SQLite;
using System.Linq;
using System;
using AlphaCodex.Data;

namespace AlphaCodex
{
    public class Favorite
    {
        #region Fields
        #endregion

        #region Properties
        public int Id { get; set; }
        public int Group { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        #endregion
        
        #region Constructor
        public Favorite(SQLiteDataReader reader)
        {
            Id = (int)(long)reader["id_favorite"];
            Group = (int)(long)reader["id_group"];
            Type = reader["type"] as string;
            Name = string.Empty;
        }

        public Favorite(FeatViewModel feat, int group=0)
        {
            Id = feat.Id;
            Group = group;
            Type = "feat";
            Name = feat.Name;
        }

        internal bool Save()
        {
            DatabaseAccess da = new DatabaseAccess();

            return da.SaveFavorite(this);
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