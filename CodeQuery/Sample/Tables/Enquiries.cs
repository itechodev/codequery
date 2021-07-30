using CodeQuery;

namespace Sample.Tables
{
    public class Enquiries: DbTable
    {
        public int Id { get; set; }
        public int UserId { get; set;  }
    }
}