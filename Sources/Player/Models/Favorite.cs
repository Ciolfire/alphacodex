using System.Data.SQLite;
using System.Linq;
using System;
using AlphaCodex.Data;
using AlphaCodex.Models;
using AlphaCodex.ViewModels;

namespace AlphaCodex
{
    public class Favorite : Link
    {
        #region Fields
        private int _id { get; set; }
        #endregion

        #region Properties
        public int Group { get; set; }
        #endregion
        
        #region Constructor
        public Favorite(SQLiteDataReader reader)
        {
            _id = (int)(long)reader["id"];
            Type = reader["type"] as string;
            ParentId = (int)(long)reader["id_favorite"];
            Group = (int)(long)reader["id_group"];
            Name = string.Empty;
        }

        public Favorite(FeatViewModel feat, int group=0)
        {
            ParentId = feat.Id;
            Group = group;
            Type = "feat";
            Name = feat.Name;
        }

        public Favorite(string name, int parent, string type, int id = 0) : base(name, parent, type, id)
        {
        }
        #endregion
        #region Methods
        internal bool Save()
        {
            DatabaseAccess da = new DatabaseAccess();

            int result = da.SaveFavorite(this);
            if (result == -1) return false;
            _id = result;
            return true;
            
        }

        internal bool Delete()
        {
            DatabaseAccess da = new DatabaseAccess();

            if (!da.DeleteFavorite(_id)) return false;
            return true;

        }
        #endregion
    }
}