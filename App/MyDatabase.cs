using System;
using codequery.Drivers;
using codequery.Expressions;
using codequery.QuerySources;

namespace codequery.App
{
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
        public void Run()
        {
            var db = new MyDatabase();
            var exp = db.Stations
                .Where(s => s.Active.ToString().Substring(1).ToLower() == "aa")
                .Where(s => s.UID.Contains("11"))
                .Select(s => new {
                    aa = s.FarmId,
                    bb = s.Active
                });

            IDatabaseDriver driver = new SQLLiteDatabaseDriver();
            Console.WriteLine(driver.GenerateSelect(exp.Query));

            db.From(
                db.Select(new { Num = 1, Name = "Pizza"})
                .Union(new { Num = 2, Name = "Burgers"})
                .Union(new { Num = 3, Name = "Pancakes"})
            ).Where(x => x.Num > 0);

                
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