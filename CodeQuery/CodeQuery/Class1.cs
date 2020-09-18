using System;
using CodeQuery.Interfaces;

namespace CodeQuery
{
    // ---------------------------

    public class Enquiries: DbTable
    {
        public int Id { get; set; }
        public int UserId { get; set;  }
        
    }

    public class User : DbTable
    {
        public int Id { get; set;  }
        public string Name { get; set; }
    }

    public class TopUp : DbTable
    {
        public int Id { get; set; }
        public int UserId { get; set;  }
        public DateTime Added { get; set; }
        public int Count { get; set; }
    }

   
    public class TestDb : DbSchema
    {
        public DbQuery<Enquiries> Enquiries { get; set;  }
        public DbQuery<User> Users { get; set; }
        public DbQuery<TopUp> Topups { get; set;  }

        public void Query()
        {
            var r = new TopUp();
            
            Topups.Select(t => t.Max(f => f.Added));
            

            // Topups
            //     .Join(JoinType.Inner, Users)
            //     //.Where(t => t.Count > 1)
            //     .GroupBy((t, u) => t.UserId)
            //     .Select(t => new
            //     {
            //         UserId = t.Key,
            //         Total = t.Sum(g => g.Item1.Count)
            //         // Total = t.Sum((t, u) => t.Added),
            //         // Average = t.Average(f => f.Count),
            //         // Min = t.Min(f => f.Added),
            //         // Max = t.Max(f => f.Added)
            //     });
            //
            // Enquiries
            //     .GroupBy(e => e.UserId)
            //     .Select(e => new
            //     {
            //         UserId = e.Key,
            //         Used = e.Count(null),
            //     })
            //     .Join(JoinType.Inner, Topups, (e, t) => e.Used == t.Count)
            //     .Join(Users, (e, _, u) => e.UserId == u.Id)
            //     .Select((e, t, u) => new
            //     {
            //         Used = e.Used + t.Count,
            //         NAme = u.Name
            //     });
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