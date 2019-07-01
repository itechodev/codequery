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

    public class MyDatabase : Database
    {
        public DatabaseTable<Station> Stations { get; set; }        
    }

    public class Application
    {
        public void Run()
        {
            var db = new MyDatabase();
            db.Stations
                .Where(s => s.Active)
                .Select(s => s.FarmId)
                .FetchArray();
        }

        private void SqlBuilder()
        {
            var select = new SelectQuery();
            var person = new TableSource("Person", "p");
            select.Fields = new SelectField[]
            {
                new SelectField(new SourceFieldExpression(FieldType.String, "name", person), null),
                new SelectField(new SourceFieldExpression(FieldType.Int, "age", person), null),
            };
            select.From = person;
            select.Where = new MathExpression(FieldType.Bool, new SourceFieldExpression(FieldType.Int, "age", person), FieldMathOperator.GreaterEqualThan, new ConstantExpression(FieldType.Int, 18)); 

            IDatabaseDriver driver = new SQLLiteDatabaseDriver();
            Console.WriteLine(driver.GenerateSelect(select));
        }

        private void Semantics()
        {
        //     var db = new MyDatabase();

        //     // From table source
        //     db.Stations
        //         .Where(s => s.FarmId == 10)
        //         .Select(s => s.Id)
        //         .FetchSingle();
            
        //     db.Stations
        //         .Where(s => s.Active)
        //         .Select(s => new {
        //             Farm = s.FarmId,
        //             Age = s.FarmId.Value
        //         })
        //         .FetchArray();
                
        //     // From Constant source
        //     db.Select(10)
        //         .FetchSingle();

        //     db.Select(new {
        //         a = 10,
        //         b = 20
        //     })
        //     .FetchSingle();

        //     // From SubQuery
        //     db.From(
        //         db.Stations
        //             .Select(s => new {
        //                 random = "field"
        //             })
        //     ).Select(a => a.random);
        }
    }
}