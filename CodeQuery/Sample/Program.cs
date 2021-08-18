using System;
using System.Collections.Generic;
using System.Linq;
using CodeQuery;
using CodeQuery.Definitions;
using CodeQuery.Interfaces;
using CodeQuery.SqlExpressions;
using CodeQuery.SqlGenerators;
using Sample.Tables;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new TestDb();
            // db.Initialize();
            
            // // Update users set col1 = val1 [where clause]
            // db.User
            //     .Set(u => u.Name, u => u.Name + "-changed")
            //     .Where(u => u.Id == 10)
            //     .Update();
            //
            // // Insert into Users (..) values (...)
            // db.User.Insert(new User()
            // {
            //     Name = "NewUser"
            // });
            //
            // // Insert into Users (..) values ((...), (...), ...)
            // db.User.Insert(new[]
            // {
            //     new User(),
            //     new User(),
            //     new User(),
            // });
            // // Insert into Users (..) select ....
            // db.User.Insert(db.User.Where(u => u.Id > 10));
            //
            // // delete from Users [where clause]
            // db.User.Delete(u => u.Id == 10);
            //
            // // Select 1 as "num", current_date as "now" union
            // var aa = db.Select(new
            //     {
            //         Num = 1,
            //         Now = DateTime.Now
            //     })
            //     .Union(db.Select(new
            //     {
            //         Num = 2,
            //         Now = DateTime.Now.AddDays(-1)
            //     }));
            //
            // // db.Function(DbFunctions.GenerateSeries(50, 100))
            // // db.GenerateSeries()
            //
            // // SELECT generate_series(50, 100)
            // var numbers = DbFunctions.GenerateSeries(50, 100)
            //     .FetchMultiple();

            var results = db.TopUp
                .InnerJoin(db.User, (up, user) => up.UserId == user.Id);
            
                // .Where((up, user) => up.Id < 100 && user.Id > 200)
                // .GroupBy((up, user) => new {Id = user.Id, Name = user.Name })
                // .Select(a => new
                // {
                //     UserId = a.Key.Id,
                //     Name = a.Key.Name,
                //     MinId = a.Min((t, _) => t.Id),
                //     MaxId = a.Max((t, _) => t.Id),
                //     Count = a.Sum((t, _) => t.Count),
                // });

            Console.WriteLine("Hello World!");
        }

        public static void Expressions()
        {
            var userTable = new SqlTableSource("Users");
                
            // Select Id, Name from Users
            var select = new SqlSelectExpression(userTable)
            {
                Fields = new List<SqlExpression>()
                {
                    new SqlColumnExpression(new SqlColumnDefinition(userTable, "Email", SqlColumnType.Varchar)),
                    new SqlColumnExpression(new SqlColumnDefinition(userTable, "Id", SqlColumnType.Int32))
                }
            };
        }
    }
}