using System;
using System.Data.SQLite;
using System.Linq;

namespace AlphaCodex.Models
{
   public class FeatType
    {
        #region Properties
        public int Id { get; set; }

        public string Name { get; set; }
        public string Info { get; set; }
        public string Picture { get; set; }
        #endregion

        #region  Constructor
        public FeatType(SQLiteDataReader reader)
        {
            Id = (int)(long)reader["id"];

            Name = reader["name"].ToString();
            Info = reader["info"].ToString();
            Picture = "/AlphaCodex;component/data/pictures/UI/feats/type/" + reader["picture"].ToString();
        }
        #endregion
    }
}