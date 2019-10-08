using System;
using codequery.Drivers;
using codequery.Expressions;
using codequery.QuerySources;

namespace codequery.App
{
    public class Application
    {


        public void Run()
        {
            // var db = new MyDatabase();

            Expression();            
            
        } 

        private void Expression()
        {
            var users = new TableDefinition
            {
                Name = "Users",
                Columns = new ColumnDefinition[]
                {
                    new ColumnDefinition("Id", FieldType.Int),
                    new ColumnDefinition("Username", FieldType.String),
                    new ColumnDefinition("Password", FieldType.String),
                }
            };
            // Select password from useres where id == 2 
            var select = new SqlSelectQuery();
            select.From = new SqlTableSource(users, "u");
            select.Fields = new SqlSelectField[]
            {
                new SqlSelectField(new SqlColumnExpression(select.From.FieldByIndex(2)), null)
            };
            select.Where = new SqlMathExpression(
                FieldType.Bool, 
                new SqlColumnExpression(select.From.FieldByIndex(0)), 
                FieldMathOperator.Equal, 
                new SqlConstantExpression(FieldType.Int, 2)
            );
            
            var driver = new SQLLiteDatabaseDriver();
            string sql = driver.GenerateSelect(select);
            Console.WriteLine(sql);
        }
    }
}