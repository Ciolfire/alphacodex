using System.Data.SQLite;

namespace AlphaCodex
{
   public class FeatPrerequisite : ObservableObject
    {
        // Properties
        public int Id { get; set; }

        public int ParentId { get; set; }
        public string Type { get; set; }
        public int LinkId { get; set; }
        public int ChildId { get; set; }

        //constructor
        public FeatPrerequisite(SQLiteDataReader reader)
        {
            this.Id = (int)(long)reader["id"];

            this.ParentId = (int)(long)reader["parent_id"];
            this.Type = reader["type"].ToString();
            this.LinkId = (int)(long)reader["link_id"];
            this.ChildId = (int)(long)reader["child_id"];
        }
    }
}