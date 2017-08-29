using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Windows;

namespace AlphaCodex
{
    public class Skill
    {
        // Properties
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Ability keyAbility { get; set; }
        public bool untrained { get; set; }
        public short armorPenalty { get; set; }


        //constructor
        public Skill(SQLiteDataReader reader)
        {
            id = (int)(long)reader["id"];
            name = reader["name"] as string;
            description = reader["description"] as string;
            keyAbility = MainWindow.Abilities.Find(x => x.id == (int)(long)reader["key_ability"]);
            if (keyAbility == null)
                keyAbility = new Ability();
            untrained = (long)reader["untrained"] != 0;
            armorPenalty = (short)(long)reader["armor_penalty"];
        }

        // Methods //
    }
}