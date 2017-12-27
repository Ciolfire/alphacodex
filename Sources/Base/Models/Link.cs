using System;
using System.Data.SQLite;

namespace AlphaCodex.Models
{
   public class Link: EventArgs
    {
        #region Properties
        public int ParentId { get; set; }
        public string Type { get; set; }
        public int LinkId { get; set; } = -1;
        public string Name { get; set; } = string.Empty;
        #endregion Properties

        #region Constructor
        protected Link()
        {
        }

        public Link(SQLiteDataReader reader)
        {
            ParentId = (int)(long)reader["parent_id"];
            Type = reader["type"].ToString();
            LinkId = (int)(long)reader["link_id"];
        }

        public Link(string name, int parent, string type, int id = 0)
        {
            Name = name;
            ParentId = parent;
            Type = type;
            LinkId = id;
        }
        #endregion Constructor
    }
}