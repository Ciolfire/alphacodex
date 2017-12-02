using AlphaCodex.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AlphaCodex.Data
{
    public partial class DatabaseAccess
    {
        /// <summary>
        /// Retrieve the list of feats from the database
        /// </summary>
        public List<Feat> GetFeats()
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'feat' ORDER BY name");

            return ExecuteQuery(command, FeatsHandler) as List<Feat>;
        }

        /// <summary>
        /// Handle the feats data fetched from the database
        /// </summary>
        /// <returns></returns>
        private Object FeatsHandler()
        {
            List<Feat> feats = new List<Feat>();
            while (reader.Read())
            {
                feats.Add(new Feat(reader));
            }
            System.Diagnostics.Debug.WriteLine("[Feats] Feats Fetched.");
            return feats;
        }

        /// <summary>
        /// Retrieve the list of feat types from the database
        /// </summary>
        public List<FeatType> GetFeatType()
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'feat_type' ORDER BY name");

            return ExecuteQuery(command, FeatTypesHandler) as List<FeatType>;
        }

        /// <summary>
        /// Handle the feat types data fetched from the database
        /// </summary>
        /// <returns></returns>
        private Object FeatTypesHandler()
        {
            List<FeatType> featTypes = new List<FeatType>();
            while (reader.Read())
            {
                featTypes.Add(new FeatType(reader));
            }
            System.Diagnostics.Debug.WriteLine("[Feats] Types Fetched.");
            return featTypes;
        }

        /// <summary>
        /// Retrieve the list of feat subtypes from the database
        /// </summary>
        public List<FeatSubtype> GetFeatSubtype()
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'feat_subtype' ORDER BY name");

            return ExecuteQuery(command, FeatSubtypesHandler) as List<FeatSubtype>;
        }

        /// <summary>
        /// Handle the feat subtypes data fetched from the database
        /// </summary>
        /// <returns></returns>
        private Object FeatSubtypesHandler()
        {
            List<FeatSubtype> featSubtypes = new List<FeatSubtype>();
            while (reader.Read())
            {
                featSubtypes.Add(new FeatSubtype(reader));
            }
            System.Diagnostics.Debug.WriteLine("[Feats] Subtypes Fetched.");
            return featSubtypes;
        }

        /// <summary>
        /// Retrieve the feat prerequisites for each feat from the database
        /// </summary>
        /// <returns></returns>
        public void GetFeatsPrerequisites(List<Feat> feats)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT link_id, type , parent_id FROM 'feat_prerequisite' WHERE child_id LIKE @id");

            foreach (Feat feat in feats)
            {
                command.Parameters.AddWithValue("@id", feat.Id);
                feat.Prerequisites = (ExecuteQuery(command, FeatPrerequisitesHandler)) as List<Link>;
            }
            System.Diagnostics.Debug.WriteLine("[Feats] Prerequisites Fetched.");
        }

        /// <summary>
        /// Handle the feat prerequisite data fetched from the database
        /// </summary>
        /// <returns></returns>
        private Object FeatPrerequisitesHandler()
        {
            List<Link> featPrerequisites = new List<Link>();
            while (reader.Read())
            {
                featPrerequisites.Add(new Link(reader));
            }
            return featPrerequisites;
        }
    }
}