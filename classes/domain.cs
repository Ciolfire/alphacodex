using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;

namespace AlphaCodex
{
    public class domain
    {
        // Properties
        public int id { get; set; }

        public string name { get; set; }
        public string power { get; set; }

        public List<string> spells { get; set; }
        //constructor
        public domain(SQLiteDataReader reader)
        {
            this.id = (int)(long)reader["id"];
            this.name = reader["name"].ToString();
            this.power = reader["power"].ToString();
            this.spells = reader["spells"].ToString().Split('\n').Select(sValue => sValue.Trim()).ToList();
        }

        // Methods //
    }
}