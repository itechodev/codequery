using System;
using System.Linq;
using CodeQuery;
using CodeQuery.Definitions;
using CodeQuery.Interfaces;
using CodeQuery.SqlExpressions;
using Sample.Tables;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new TestDb();
            db.Initialize();
            
            // Update users set col1 = val1 [where clause]
            db.User
                .Update(u => u.Name, u => u.Name + "-changed")
                .Where(u => u.Id == 10);

            // Insert into Users (..) values (...)
            db.User.Insert(new User()
            {
                Name = "NewUser"
            });
            
            // Insert into Users (..) values ((...), (...), ...)
            db.User.Insert(new[]
            {
                new User(),
                new User(),
                new User(),
            });
            // Insert into Users (..) select ....
            db.User.Insert(db.User.Where(u => u.Id > 10));
            
            // delete from Users [where clause]
            db.User.Delete(u => u.Id == 10);
            
            // Select 1 as "num", current_date as "now" union
            db.Select(new
                {
                    Num = 1,
                    Now = DateTime.Now
                })
                .Union(db.Select(new
                {
                    Num = 2,
                    Now = DateTime.Now.AddDays(-1)
                }));
            
            // db.Function(DbFunctions.GenerateSeries(50, 100))
            // db.GenerateSeries()

            // SELECT generate_series(50, 100)
            var numbers = DbFunctions.GenerateSeries(50, 100)
                .FetchMultiple();
            
            var results = db.TopUp
                .Join<User>(JoinType.Inner)
                // .JoinUser()
                .Where((up, user) => up.Id < 100 && user.Id > 200)
                .GroupBy((up, user) => user.Id)
                .Select(a => new
                {
                    UserId = a.Key,
                    MinId = a.Min((t, _) => t.Id),
                    MaxId = a.Max((t, _) => t.Id),
                    Count = a.Sum((t, _) => t.Count),
                });

            Console.WriteLine("Hello World!");
        }
    }
}