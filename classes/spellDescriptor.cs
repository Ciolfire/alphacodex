using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;

namespace AlphaCodex
{
    public class spellDescriptor
    {
        // Properties
        public int id { get; set; }

        public string name { get; set; }
        public string description { get; set; }

        //constructor
        public spellDescriptor(SQLiteDataReader reader)
        {
            this.id = (int)(long)reader["id"];
            this.name = reader["name"].ToString();
        }

        // Methods //
    }
}