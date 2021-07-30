using CodeQuery;

namespace Sample.Tables
{
    // [DatabaseSchema("")]
    public class User : DbTable
    {
        public int Id { get; set;  }
        public string Name { get; set; }
    }
}