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