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
            Expressions();
            return;

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
        }

        public static void Expressions()
        {
            ISqlGenerator generator = new GenericSqlGenerator();
            
            // SELECT 31 as "age", 'willem' as "name"
            var constSource = new SqlNoSource();
            var constExp = new SqlSelectQuery(constSource)
            {
                Fields = new List<SqlExpression>
                {
                    new SqlAliasExpression(new SqlConstExpression(31, SqlColumnType.Int32), "age"),
                    new SqlAliasExpression(new SqlConstExpression("willem", SqlColumnType.Varchar), "name"),
                }
            };

            Console.WriteLine(generator.Select(constExp));


            // Select Id, Name from Users
            var simple = new SqlSelectQuery(UserTable.Source)
            {
                Fields = new List<SqlExpression>()
                {
                    new SqlColumnExpression(UserTable.Id),
                    new SqlColumnExpression(UserTable.Email)
                }
            };

            Console.WriteLine(generator.Select(simple));
            
            /*
             * Select u.Id, u.Email, l.Message from Users u
             * inner join Logs l on u.Id = l.UserId
             */
            var condition = new SqlBinaryExpression(new SqlColumnExpression(UserTable.Id), new SqlColumnExpression(LogTable.UserId), SqlBinaryOperator.Equal);

            var join = new SqlSelectQuery(UserTable.Source)
            {
                Joins = new List<SqlJoinSource>
                {
                    new(LogTable.Source, condition, JoinType.Inner)
                },
                Fields = new List<SqlExpression>
                {
                    new SqlColumnExpression(UserTable.Id),
                    new SqlColumnExpression(UserTable.Email),
                    new SqlColumnExpression(LogTable.Message),
                }
            };
            
            Console.WriteLine(generator.Select(join));

            // Select "age", "name"
            // from (SELECT 31 as "age", 'willem' as "name") a

            var subQuery = new SqlSelectQuery(new SqlQuerySource(constExp, "a"))
            {
                Fields = new List<SqlExpression>()
                {
                    new SqlColumnExpression(new SqlColumnDefinition(constSource, "age", SqlColumnType.Int32)),
                    new SqlColumnExpression(new SqlColumnDefinition(constSource, "name", SqlColumnType.Varchar)),
                }
            };
            
            Console.WriteLine(generator.Select(subQuery));
        }
    }

    public static class UserTable
    {
        public static readonly SqlTableSource Source = new("Users");

        public static readonly SqlColumnDefinition Id = new(Source, "Id", SqlColumnType.Varchar);
        public static readonly SqlColumnDefinition Email = new(Source, "Email", SqlColumnType.Varchar);
    }

    public static class LogTable
    {
        public static readonly SqlTableSource Source = new("Logs");

        public static readonly SqlColumnDefinition Id = new(Source, "Id", SqlColumnType.Int32);
        public static readonly SqlColumnDefinition UserId = new(Source, "UserId", SqlColumnType.Int32);
        public static readonly SqlColumnDefinition Message = new(Source, "Message", SqlColumnType.Varchar);
    }
}