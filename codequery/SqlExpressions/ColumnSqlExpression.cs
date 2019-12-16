using codequery.Definitions;

namespace codequery.SqlExpressions
{
    public class ColumnSqlExpression : SqlExpression
    {
        //public ColumnDefinition Definition { get; set; }
        public string Name { get; }
        
        // public ColumnSqlExpression(ColumnDefinition definition)
        // {
        //     Definition = definition;
        // }

        public ColumnSqlExpression(string name)
        {
             Name = name;
        }
    }
}