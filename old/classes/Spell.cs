using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows;

namespace AlphaCodex
{
    public class Spell : INotifyPropertyChanged
    {
        // Properties
        public int id { get; set; }
        public string name { get; set; }
        public string info { get; set; }
        public string description { get; set; }
        public spellSchool school { get; set; }
        public spellSubschool subschool { get; set; }
        public List<string> descriptor { get; set; }
        private Visibility _visibility = Visibility.Visible;
        public Visibility visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                NotifyPropertyChanged("visibility");
            }
        }
        // Events //
        public event PropertyChangedEventHandler PropertyChanged;

        // Constructors
        public Spell() { }
        public Spell(SQLiteDataReader reader, ObservableCollection<spellSchool> schoolList, ObservableCollection<spellDescriptor> descriptorList)
        {


            this.id = (int)(long)reader["id"];
            this.name = reader["name"].ToString();
            this.info = reader["info"].ToString();
            this.description = reader["description"].ToString();
            this.school = schoolList.First(item => item.id == (long)reader["school"]);
            if ((long)reader["subschool"] != 0)
                this.subschool = school.subschool.First(item => item.id == (long)reader["subschool"]);
            if ((string)reader["descriptor"] != "")
                getDescriptor(reader, descriptorList);
        }

        // Methods //

        private void getDescriptor(SQLiteDataReader reader, ObservableCollection<spellDescriptor> descriptorList)
        {
            List<string> tmp;
            string str;

            str = (string)reader["descriptor"];
            tmp = str.Split(',').Select(sValue => sValue.Trim()).ToList();
            this.descriptor = new List<string>();
            foreach (string descriptor in tmp)
                this.descriptor.Add(descriptorList.First(item => item.id.ToString() == descriptor).name);
        }

        // Recording of the Event
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
