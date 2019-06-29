using System;
using codequery.Drivers;
using codequery.Expressions;

namespace codequery
{
    class Program
    {
        static void Main(string[] args)
        {
            var select = new SelectQuery();
            var person = new TableSource("Person", "p");
            select.Fields = new SelectField[]
            {
                new SelectField(new SourceFieldExpression(FieldType.String, "name", person), null),
                new SelectField(new SourceFieldExpression(FieldType.Int, "age", person), null),
            };
            select.From = person;

            IDatabaseDriver driver = new SQLLiteDatabaseDriver();
            Console.WriteLine(driver.GenerateSelect(select));
        }
    }
}
