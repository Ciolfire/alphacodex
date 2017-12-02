using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AlphaCodex
{
    public partial class MainWindow : Window
    {
        #region Get Data
        /// <summary>
        /// Retrieve the list of abilities from the DB
        /// </summary>
        //private void GetAbilities()
        //{
        //    Abilities = new List<Ability>();

        //    string queryString = "SELECT * FROM 'ability'";

        //    using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
        //    {
        //        connDB.Open();
        //        SQLiteCommand command = new SQLiteCommand(queryString, connDB);
        //        SQLiteDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            Abilities.Add(new Ability(reader));
        //        }
        //        reader.Close();
        //        connDB.Close();
        //    }
        //    System.Diagnostics.Debug.WriteLine("[General] Abilities Initialized");
        //}

        //private void GetAlignments()
        //{
        //    Alignments = new List<Alignment>();

        //    string queryString = "SELECT * FROM 'alignment'";

        //    using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
        //    {
        //        connDB.Open();
        //        SQLiteCommand command = new SQLiteCommand(queryString, connDB);
        //        SQLiteDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            Alignments.Add(new Alignment(reader));
        //        }
        //        reader.Close();
        //        connDB.Close();
        //    }
        //    System.Diagnostics.Debug.WriteLine("[General] Alignments Initialized");
        //}


        //private void GetPlayer()
        //{
        //    string queryString = "SELECT * FROM 'player' WHERE id LIKE 1";

        //    using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
        //    {
        //        connDB.Open();
        //        SQLiteCommand command = new SQLiteCommand(queryString, connDB);
        //        SQLiteDataReader reader = command.ExecuteReader();

        //        reader.Read();
        //        Player = new Player(reader);
        //        reader.Close();
        //        connDB.Close();
        //    }
        //    System.Diagnostics.Debug.WriteLine("[General] Player Initialized");
        //}

        //private void GetSkills()
        //{
        //    Skills = new List<Skill>();

        //    string queryString = "SELECT * FROM 'skill'";

        //    using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
        //    {
        //        connDB.Open();
        //        SQLiteCommand command = new SQLiteCommand(queryString, connDB);
        //        SQLiteDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            Skills.Add(new Skill(reader));
        //        }
        //        reader.Close();
        //        connDB.Close();
        //    }
        //    System.Diagnostics.Debug.WriteLine("[General] Skills Initialized");
        //}

        

        /// <summary>
        /// Retrieve the list of magic schools from the DB
        /// </summary>
        //private void GetSpellSchoolList()
        //{
        //    string queryString = "SELECT * FROM 'spell_school'";

        //    using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
        //    {
        //        connDB.Open();
        //        SQLiteCommand command = new SQLiteCommand(queryString, connDB);
        //        SQLiteDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            SpellSchoolList.Add(new spellSchool(reader, connDB));
        //        }
        //        reader.Close();
        //        connDB.Close();
        //    }
        //}

        /// <summary>
        /// Retrieve the list of spell descriptors from the DB
        /// </summary>
        //private void GetSpellDescriptorList()
        //{
        //    string queryString = "SELECT * FROM 'spell_descriptor'";

        //    using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
        //    {
        //        connDB.Open();
        //        SQLiteCommand command = new SQLiteCommand(queryString, connDB);
        //        SQLiteDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            spellDescriptorList.Add(new spellDescriptor(reader));
        //        }
        //        reader.Close();
        //        connDB.Close();
        //    }
        //}

        /// <summary>
        /// Retrieve the list of spellcaster classes from the DB
        /// </summary>
        //private void GetSpellDomainList()
        //{
        //    string queryString = "SELECT * FROM 'spell_domain'";

        //    using (SQLiteConnection connDB = new SQLiteConnection(MainWindow.conn))
        //    {
        //        connDB.Open();
        //        SQLiteCommand command = new SQLiteCommand(queryString, connDB);
        //        SQLiteDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            DomainList.Add(new domain(reader));
        //        }
        //        reader.Close();
        //        connDB.Close();
        //    }
        //}
        #endregion
    }
}