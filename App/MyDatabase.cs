using System;
using System.Collections.Generic;
using System.Linq;
using codequery.Drivers;
using codequery.Expressions;
using codequery.QuerySources;

namespace codequery.App
{
    public class Author 
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Book 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
    }


    public class ScissorsEntry
    {
        public double Long { get; set; }
        public double Lat { get; set; }
        public double Alt { get; set; }
        public double Battery { get; set; }

        public DateTime TimeStamp { get; set; }
        public int StationId { get; set; }
        public int ScissorsId { get; set; }
        public int? BinTagEntryId { get; set; }
    }
    
    public class Station
    {
        public int Id { get; set; }
        public string UID { get; set; }
        public bool Active { get; set; }
        public int? FarmId { get; set; }
    }

    public class Scissor
    {
        public int Id { get; set; }
        public string UID { get; set; }
        public string RFIDUID { get; set; }
    }

    public class Farm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
    }

    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MyDatabase : Database
    {
        public DatabaseTable<Client> Clients { get; set; }
        public DatabaseTable<Farm> Farms { get; set; }
        public DatabaseTable<Station> Stations { get; set; }
    }

    public class Application
    {
        private void LogSql<T>(ResultQuerySource<T> query)
        {
            IDatabaseDriver driver = new SQLLiteDatabaseDriver();
            Console.WriteLine(driver.GenerateSelect(query.Query));
        }

        public void SignatureOnly()
        {
            IQuerySource<Book> books = null;
            IQuerySource<Author> authors = null;
            
            List<Book> ebook = null;
            List<Author> eauthor = null;

            ebook
                .Join(eauthor, b => b.AuthorId, h => h.Id, (b,a) => new {b,a})
                .GroupBy(se => se.b.AuthorId)
                .Select(g => g.Max(k => k.a.Id));
                
                // .GroupBy(g => g.AuthorId);
                // .Select(f => f.)

            var q = books
                .Join(authors)
                .GroupBy((b, a) => a.Id)
                .Select(f => new {
                    Count = f.Count(),
                    Maxi = f.Max((b, a) => b.Name)
                })
                .Join(authors)
                .Join(authors)
                .Select((prev, a, a2) => new {
                    CC = prev.Count,
                    Name = a.Name
                })
                .Join(books)
                .Join(authors)
                .Select((a, b, c) => c.Name);
                
                

                // .Select( (book, author) => new {
                //     AuthorName = author.Name,
                //     BookName = book.Name
                // });
            
        }

        public void Run()
        {
            var db = new MyDatabase();
            
            var unionList = db.From(db.Union(
                new { Id = 1, Category = 1, Name = "A"},
                new { Id = 2, Category = 1, Name = "B"},
                new { Id = 3, Category = 2, Name = "A"}
            ))
            .GroupBy(x => new {Category = x.Category * 2, x.Name})
            .Select(x => new {
                category = x.Value.Category,
                name = x.Value.Name,
                count = x.Count(),
                aa = x.Sum(k => k.Category),
                max = x.Max(k => k.Name),
                min = x.Min(k => k.Name)
            });

            LogSql(unionList);
        } 

        private void Other()
        {
            var db = new MyDatabase();
            var exp = db.Stations
                .Where(s => s.Active.ToString().Substring(1).ToLower() == "aa")
                .Where(s => s.UID.Contains("11"))
                .Select(s => new {
                    aa = s.FarmId,
                    bb = s.Active
                });
            LogSql(exp);

            LogSql(db.Select(new { Num = 1, Name = "Pizza"}));

            LogSql(db.Select((1, "Hannes hond")));

            var unionList = db.From(db.Union(
                new { Calories = 212, Name = "Pizza"},
                new { Calories = 11, Name = "Burgers"},
                new { Calories = 0, Name = "Water"}
            )).Where(x => x.Calories > 10).SelectAll();

            LogSql(unionList);

            var constExp = db.From(
                db.Select(new { Num = 1, Name = "Pizza"})
                .Union(new { Num = 2, Name = "Burgers"})
                .Union(new { Num = 3, Name = "Pancakes"})
            )
            .Where(x => x.Num > 0)
            .Select(x => new {
                Food = x.Name,
                Price = x.Num * 2
            });
            
            LogSql(constExp);    
        }

        private void SqlBuilder()
        {
            var select = new SelectQuery();
            var person = new SqlTableSource(Database.GetTableDefinition<Person>(), "p");
            select.Fields = new SelectField[]
            {
                new SelectField(new SqlColumnExpression(FieldType.String, "name", person), null),
                new SelectField(new SqlColumnExpression(FieldType.Int, "age", person), null),
            };
            select.From = person;
            select.Where = new SqlMathExpression(FieldType.Bool, new SqlColumnExpression(FieldType.Int, "age", person), FieldMathOperator.GreaterEqualThan, new SqlConstantExpression(FieldType.Int, 18)); 

            IDatabaseDriver driver = new SQLLiteDatabaseDriver();
            Console.WriteLine(driver.GenerateSelect(select));
        }
    }

    internal class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}