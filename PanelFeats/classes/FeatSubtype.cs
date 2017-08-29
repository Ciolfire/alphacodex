using System.ComponentModel;
using System.Data.SQLite;

namespace AlphaCodex
{
    public class FeatSubtype : ObservableObject
    {  
        // Properties
        public int Id { get; set; }

        public string Name { get; set; }
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

        //constructor
        public FeatSubtype(SQLiteDataReader reader)
        {
            this.Id = (int)(long)reader["id"];

            this.Name = reader["name"].ToString();
            this.Picture = "/AlphaCodex;component/data/pictures/UI/feats/subtype/" + reader["picture"].ToString();
            this.Disabled = false;
        }

        // Methods //
    }
}