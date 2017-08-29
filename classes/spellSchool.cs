using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;

namespace AlphaCodex
{
    public class spellSchool : INotifyPropertyChanged
    {
        // Properties
        public int id { get; set; }

        public string name { get; set; }
        public string description { get; set; }
        public List<spellSubschool> subschool { get; set; } = new List<spellSubschool>();
        private double _disabled = 0;
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
        public spellSchool(SQLiteDataReader reader, SQLiteConnection connDB)
        {
            this.id = (int)(long)reader["id"];

            this.name = reader["name"].ToString();
            this.description = reader["description"].ToString();
            GetSubschool(connDB);
        }

        // Methods //

        private void GetSubschool(SQLiteConnection connDB)
        {
            string queryString = "SELECT * FROM 'spell_subschool' WHERE school = " + id.ToString();
            SQLiteCommand command = new SQLiteCommand(queryString, connDB);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
               subschool.Add(new spellSubschool(reader));
            }
            reader.Close();
        }

        // Recording of the Event
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}