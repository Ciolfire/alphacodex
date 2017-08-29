using System;
using System.Data.SQLite;
using System.Linq;

namespace AlphaCodex
{
   public class FeatType : ObservableObject
    {
        #region Properties
        public int Id { get; set; }

        public string Name { get; set; }
        public string Info { get; set; }
        public string Picture { get; set; }
        private bool _disabled;
        public bool Disabled
        {
            get { return this._disabled; }
            set
            {
                if (value != this._disabled)
                {
                    _disabled = value;
                    NotifyPropertyChanged("Disabled");
                }
            }
        }
        #endregion

        #region  constructor
        public FeatType(SQLiteDataReader reader)
        {
            Id = (int)(long)reader["id"];

            Name = reader["name"].ToString();
            Info = reader["info"].ToString();
            Picture = "/AlphaCodex;component/data/pictures/UI/feats/type/" + reader["picture"].ToString();

            //if (!resourceNames.Contains(Picture))
            //{
            //    Picture = "/AlphaCodex;component/data/pictures/UI/feats/type/none.png";
            //}
            Disabled = false;
        }
        #endregion
    }
}