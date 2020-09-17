using System;
using CodeQuery.Interfaces;

namespace CodeQuery
{
    // ---------------------------

    public class Enquiries
    {
        public int Id { get; set; }
        public int UserId { get; set;  }
        
    }

    public class User
    {
        public int Id { get; set;  }
        public string Name { get; set; }
    }

    public class TopUp
    {
        public int Id { get; set; }
        public int UserId { get; set;  }
        public DateTime Added { get; set; }
        public int Count { get; set; }
    }

    public class TestDb : DatabaseContext
    {
        public DbTable<Enquiries> Enquiries { get; set;  }
        public DbTable<User> Users { get; set; }
        public DbTable<TopUp> Topups { get; set;  }

        public void Query()
        {
            Enquiries
                .GroupBy(e => e.UserId)
                .Select(e => new
                {
                    UserId = e.Key,
                    Used = e.Count(null)
                })
                .Join(JoinType.Inner, Topups, (e, t) => e.Used == t.Count)
                .Join(Users, (e, _, u) => e.UserId == u.Id)
                .Select((e, t, u) => new
                {
                    Used = e.Used + t.Count,
                    NAme = u.Name
                });
        }
        
    }

    public abstract class SqlSource
    {
        
    }
    
    public class SqlNoSource : SqlSource
    {
        
    }
    
    public class SqlTable : SqlSource
    {
        
    }

    public class SqlQuerySelect: SqlSource
    {
        public string[] Fields { get; set; }
        public SqlSource Source { get; set; }
        public string GroupBy { get; set; }
        public string Where { get; set; }
        public string Join { get; set; }
    }
    
    
    
    
}