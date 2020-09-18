using System;
using System.Linq;
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
        public void Query()
        {
            var res = From<TopUp>()
                .Select(t => new {t.Added, t.Count})
                .FetchMultiple();

            var results = From<TopUp>()
                .Join<User>(JoinType.Inner)
                .Where((up, user) => up.Id < 100 && user.Id > 200)
                .GroupBy((up, user) => user.Id)
                .Select(a => new
                {
                    UserId = a.Key,
                    MinId = a.Min((t, _) => t.Id),
                    MaxId = a.Max((t, _) => t.Id),
                    Count = a.Sum((t, _) => t.Count),
                });
        }
        
    }

   
}