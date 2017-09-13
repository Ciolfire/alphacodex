using System.ComponentModel;
using System.Data.SQLite;

namespace AlphaCodex
{
   public class featType : INotifyPropertyChanged
    {
        // Properties
        public int id { get; set; }

        public string name { get; set; }
        public string picture { get; set; }
        private double _disabled;
        public double disabled
        {
            get { return this._disabled; }
            set
            {
                if (value != this._disabled)
                {
                    _disabled = value;
                    NotifyPropertyChanged("disabled");
                }
            }
        }
        // Events //
        public event PropertyChangedEventHandler PropertyChanged;

        //constructor
        public featType(SQLiteDataReader reader)
        {
            this.id = (int)(long)reader["id"];

            this.name = reader["name"].ToString();
            this.picture = "data/pictures/UI/" + reader["picture"].ToString();
            this.disabled = 0;
        }
        // Recording of the Event
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}