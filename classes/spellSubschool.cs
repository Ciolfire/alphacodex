using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;

namespace AlphaCodex
{
    public class spellSubschool
    {
        // Properties
        public int id { get; set; }

        public string name { get; set; }
        public string description { get; set; }

        //constructor
        public spellSubschool(SQLiteDataReader reader)
        {
            this.id = (int)(long)reader["id"];

            this.name = reader["name"].ToString();
            this.description = reader["description"].ToString();
        }

        // Methods //
    }
}