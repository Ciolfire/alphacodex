using System;
using System.Data.SQLite;

namespace AlphaCodex
{
    public class Ability
    {
        // Properties
        public short id { get; set; }
        public string name { get; set; }
        public string shortName {get;set;}
        public string description { get; set; }

        //constructor
        public Ability(SQLiteDataReader reader)
        {
            id = (short)(long)reader["id"];
            name = reader["name"].ToString();
            shortName = reader["short"].ToString();
            description = reader["description"].ToString();
        }

        public Ability(Ability ability) : this(ability.id, ability.name, ability.shortName, ability.description) { }

        // empty-constructor
        public Ability()
        {
            id = 0;
            name = "n/a";
            shortName = "n/a";
            description = "n/a";
        }

        public Ability(short id, string name, string shortName, string description)
        {
            this.id = id;
            this.name = name;
            this.shortName = shortName;
            this.description = description;
        }

        // Methods //
    }
}