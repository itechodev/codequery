using System;
using CodeQuery;
using CodeQuery.Interfaces;
using Sample.Tables;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new TestDb();
            db.Initialize();
            
            // Select 1 as "num", current_date as "now"
            

            var results = db.TopUp
                .JoinUser()
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