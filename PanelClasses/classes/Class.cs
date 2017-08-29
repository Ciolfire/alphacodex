using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace AlphaCodex
{
    public class Class : ObservableObject
    {
        // Properties
        public int Id { get; private set; }

        public string Name { get; private set; }
        public List<string> Specials { get; private set; }
        private List<Column> ExtraColumns { get; set; } = new List<Column>();
        public short Bab { get; private set; }
        public bool SaveFort { get; private set; }
        public bool SaveRef { get; private set; }
        public bool SaveWill { get; private set; }
        public string Picture { get; private set; }

        // Fluff
        public string Description { get; private set; }
        public string Adventures { get; private set; }
        public string Characteristics { get; private set; }
        public string Alignment { get; private set; }
        public string Religion { get; private set; }
        public string Background { get; private set; }
        public string Races { get; private set; }
        public string Other_classes { get; private set; }
        public string Role { get; private set; }
        // Game rule information
        public string Abilities { get; private set; }
        public List<short> AlignmentRestriction { get; private set; }
        public string AlignmentInfo { get; private set; }
        public int HitDie { get; private set; }
        public Spellcaster Spells { get; private set; }
        // skills
        public List<CharSkill> Skills { get; private set; } = new List<CharSkill>();
        // features
        public string Features { get; private set; }
        // ex
        public string Ex { get; private set; }

        private Visibility _visibility = Visibility.Visible;
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                NotifyPropertyChanged("visibility");
            }
        }

        // Constructors
        public Class() {
            Picture = "/AlphaCodex;component/data/pictures/classes/rogue.png";
        }
        public Class(SQLiteDataReader reader)
        {
            Id = (int)(long)reader["id"];
            Name = reader["name"] as string;

            initTable(reader);
            initFlavor(reader); 
            // Gameplay
            HitDie = (int)(long)reader["hit_die"];
            Abilities = reader["abilities"] as string;
            InitAlign(reader);
            initSkills();
            InitSpells();
            Features = reader["features"] as string;
            Ex = reader["ex_field"] as string;
            // get the picture
            if (reader["picture"].ToString() != "")
                Picture = "/AlphaCodex;component/data/pictures/classes/" + reader["picture"] as string;
            else
                Picture = "/AlphaCodex;component/data/pictures/classes/rogue.png";
        }

        private void InitSpells()
        {
            string queryString = "SELECT * FROM class_caster WHERE class_id = " + Id;
            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    Spells = new Spellcaster(reader);
                }
                else
                    Spells = null;
                reader.Close();
                connDB.Close();
            }
        }

        private void InitAlign(SQLiteDataReader reader)
        {
            var alignment = reader["alignment_restriction"] as string;

            if (string.IsNullOrEmpty(alignment))
                AlignmentInfo = "Any.";
            else if (Regex.IsMatch(alignment, @"^[1-9,]+$"))
                AlignmentInfo = generateAlignmentText(alignment.Split(',').Select(short.Parse).ToList());
            else
                AlignmentInfo = alignment;
        }

        #region Methods

        // set table data
        private void initTable(SQLiteDataReader reader)
        {
            Bab = (short)(long)reader["bab"];
            SaveFort = (long)reader["save_fort"] != 0;
            SaveRef = (long)reader["save_ref"] != 0;
            SaveWill = (long)reader["save_will"] != 0;
            var toSplit = reader["special"] as string;
            // split the specials by levels
            Specials = toSplit.Split('|').ToList();
            // get the extra fields of the table, if any
            getExtraFields();
        }

        // set the flavor text
        private void initFlavor(SQLiteDataReader reader)
        {
            Description = reader["description"] as string;
            Adventures = reader["adventures"] as string;
            Characteristics = reader["characteristics"] as string;
            Alignment = reader["alignment"] as string;
            Religion = reader["religion"] as string;
            Background = reader["background"] as string;
            Races = reader["races"] as string;
            Other_classes = reader["other_classes"] as string;
            Role = reader["role"] as string;
        }

        private void initSkills()
        {
            string queryString = "SELECT s.id, name, description, skill_field, key_ability, untrained, armor_penalty FROM class_skill LEFT JOIN skill as s ON skill_id = s.id WHERE class_id = " + Id;
            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Skills.Add(new CharSkill(reader));
                }
                reader.Close();
                connDB.Close();
            }
        }

        public void getExtraFields()
        {
            string queryString = "SELECT * FROM 'class_extra_column' WHERE class_id = " + Id;
            using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
            {
                connDB.Open();
                SQLiteCommand command = new SQLiteCommand(queryString, connDB);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    ExtraColumns.Add(new Column(reader));
               reader.Close();
               connDB.Close();
            }
        }

        public List<string> setHeaders()
        {
            var headers = new List<string>();

            headers.Add("Level");
            headers.Add("Base\nAttack Bonus");
            headers.Add("Fort\nSave");
            headers.Add("Ref\nSave");
            headers.Add("Will\nSave");
            headers.Add("Special");
            return headers;
        }

        #region alignmentText
        // check the allowed alignments and create the according text
        private string generateAlignmentText(List<short> forbiddenAlignments)
        {
            // check for specific alignment, then for forbidden, then for allowed, then for specifics
            if (forbiddenAlignments.Count == 8)
            {
                foreach (Alignment alignment in MainWindow.Alignments)
                    if (forbiddenAlignments.Contains((short)alignment.id) == false)
                        return alignment.name + ".";
            }
            else if (forbiddenAlignments.Count == 3)
                return initForbiddenAlignment(forbiddenAlignments);
            else if (forbiddenAlignments.Count == 6)
                return initMandatoryAlignment(forbiddenAlignments);
            else
                return initSpecificAlignments(forbiddenAlignments);
            return null;
        }

        // check for "forbidden specified" alignments
        private string initForbiddenAlignment(List<short> forbiddenAlignments)
        {
            if (forbiddenAlignments.Contains(1)
                && forbiddenAlignments.Contains(2)
                && forbiddenAlignments.Contains(3))
                return ("Any nongood.");
            else if (forbiddenAlignments.Contains(7)
                && forbiddenAlignments.Contains(8)
                && forbiddenAlignments.Contains(9))
                return ("Any nonevil.");
            else if (forbiddenAlignments.Contains(1)
                && forbiddenAlignments.Contains(4)
                && forbiddenAlignments.Contains(7))
                return ("Any nonlawful.");
            else if (forbiddenAlignments.Contains(3)
                && forbiddenAlignments.Contains(6)
                && forbiddenAlignments.Contains(9))
                return ("Any nonchaotic.");
            else
                return initSpecificAlignments(forbiddenAlignments);
        }

        // check for "mandatory specified" alignments
        private string initMandatoryAlignment(List<short> forbiddenAlignments)
        {
            if (forbiddenAlignments.Contains(1) == false
                && forbiddenAlignments.Contains(2) == false
                && forbiddenAlignments.Contains(3) == false)
                return ("Any good.");
            else if (forbiddenAlignments.Contains(7) == false
                && forbiddenAlignments.Contains(8) == false
                && forbiddenAlignments.Contains(9) == false)
                return ("Any evil.");
            else if (forbiddenAlignments.Contains(1) == false
                && forbiddenAlignments.Contains(4) == false
                && forbiddenAlignments.Contains(7) == false)
                return ("Any lawful.");
            else if (forbiddenAlignments.Contains(3) == false
                && forbiddenAlignments.Contains(6) == false
                && forbiddenAlignments.Contains(9) == false)
                return ("Any chaotic.");
            else
                return initSpecificAlignments(forbiddenAlignments);
        }

        // check for neutral or specifics alignments
        private string initSpecificAlignments(List<short> forbiddenAlignments)
        {
            string alignements = "";

            foreach (Alignment alignment in MainWindow.Alignments)
            {
                if (!forbiddenAlignments.Contains((short)alignment.id))
                {
                    if (alignements.Any())
                        alignements += ", ";
                    alignements += alignment.name;
                }
            }
            return alignements + ".";
        }
        #endregion

        #region table creation
        public DataTable buildTable()
        {
            var classTable = new DataTable();

            foreach (string header in setHeaders())
                classTable.Columns.Add(header);
            if (ExtraColumns.Any())
                foreach (Column extraColumn in ExtraColumns)
                    classTable.Columns.Add(extraColumn.header);
            foreach (string special in Specials)
                classTable.Rows.Add((buildLevel(special, classTable.Rows.Count + 1)).ToArray());
            return classTable;
        }

        private List<string> buildLevel(string special, int current)
        {
            var level = new List<string>();

            // Actual level
            level.Add(levelToString(current));
            // Base Attack bonus
            level.Add(attackBonusToString(current));
            // Saves for this level
            level.Add(saveToString(current, SaveFort));
            level.Add(saveToString(current, SaveRef));
            level.Add(saveToString(current, SaveWill));
            // Special for this level
            level.Add(special);
            // If extra columns, add it to the level
            if (ExtraColumns.Any())
                foreach (Column extraColumn in ExtraColumns)
                    level.Add(extraColumn.rows.ElementAt(current - 1));
            return level;
        }

        // Level generation methods
        private string attackBonusToString(int current)
        {
            string attack = null;
            int bonus;

            if (Bab == 2)
                bonus = current;
            else if (Bab == 1)
                bonus = current-(1+(current-1)/4);
            else
                bonus = (current/2);
            // epic
            if (current > 20)
                bonus += (current-19)/2;
            // calculate the number of attacks
            while (bonus > 0)
            {
                if (attack != null)
                    attack += "/+" + bonus;
                else
                    attack = "+" + bonus;
                bonus -= 5;
            }
            if (bonus == 0 && attack == null)
                attack = "+0";
                return attack;
        }

        private string saveToString(int current, bool isGood)
        {
            string save = "+";
            int bonus;
            if (isGood == true)
                bonus = 2 + (current/2);
            else
                bonus = 0 + (current/3);
            // epic
            if (current > 20)
                bonus += (current-20)/2;
            return save + bonus;
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
            return level;
        }
        #endregion

        #endregion
    }
}
