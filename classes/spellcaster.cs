using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace AlphaCodex
{
    public class Spellcaster
    {
        // Properties
        public int id { get; set; }
        public int ability { get; set; }
        public DataTable spellsDaily { get; set; }
        public DataTable spellsKnown { get; set; }
        public List<string> casterType { get; set; }
        public int minLevel { get; set; }
        public int maxLevel { get; set; }
        //constructor
        public Spellcaster(SQLiteDataReader reader)
        {
            id = (int)(long)reader["id"];
            ability = (int)(long)reader["ability"];
            casterType = reader["caster_type"].ToString().Split('\n').Select(sValue => sValue.Trim()).ToList();
            minLevel = (int)(long)reader["min_level"];
            maxLevel = (int)(long)reader["max_level"];

            string daily = reader["spells_daily"].ToString();
            if (daily != "")
                initSpellsDaily(daily.Split('\n').Select(sValue => sValue.Trim()).ToList());
            else
                spellsDaily = null;
            string known = reader["spells_known"].ToString();
            if (known != "")
                initSpellsKnown(known.Split('\n').Select(sValue => sValue.Trim()).ToList());
            else
                spellsKnown = null;     
        }

        // Methods //
        private void initSpellsDaily(List<string> spellsDaily)
        {
            var tableDaily = new DataTable();

            tableDaily.Columns.Add("Level");
            for (int level = minLevel; level <= maxLevel; level++)
                tableDaily.Columns.Add(level.ToString());
            string tmp;
            foreach (string spells in spellsDaily)
            {
                tmp = levelToString(tableDaily.Rows.Count + 1) + spells;
                tableDaily.Rows.Add(tmp.Split(','));
            }
            this.spellsDaily = tableDaily;
        }

        private void initSpellsKnown(List<string> spellsKnown)
        {
            var tableKnown = new DataTable();
            for (int level = minLevel; level <= maxLevel; level++)
                tableKnown.Columns.Add(level.ToString());
            foreach (string spells in spellsKnown)
                tableKnown.Rows.Add(spells.Split(','));
            this.spellsKnown = tableKnown;
        }

        private string levelToString(int current)
        {
            string level = current.ToString();
            if ((current % 10) == 1 && current != 11)
                level += "st";
            else if ((current % 10) == 2 && current != 12)
                level += "nd";
            else if ((current % 10) == 3 && current != 13)
                level += "rd";
            else
                level += "th";
            return level + ",";
        }
    }
}