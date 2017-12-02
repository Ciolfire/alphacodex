using System.ComponentModel;
using System.Data.SQLite;

namespace AlphaCodex.Models
{
    public class FeatSubtype
    {
        #region Properties
        public int Id { get; set; }

        public string Name { get; set; }
        public string Picture { get; set; }
        #endregion
        
        public FeatSubtype(SQLiteDataReader reader)
        {
            this.Id = (int)(long)reader["id"];

            this.Name = reader["name"].ToString();
            this.Picture = "/AlphaCodex;component/data/pictures/UI/feats/subtype/" + reader["picture"].ToString();
        }
    }
}