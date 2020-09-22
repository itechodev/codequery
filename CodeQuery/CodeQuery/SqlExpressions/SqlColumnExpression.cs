using CodeQuery.Definitions;

namespace CodeQuery.SqlExpressions
{
    // Reference a column from a table
    // Select [table.ColumnName] 

    public class SqlColumnExpression : SqlExpression
    {
        public SqlColumnDefinition Definition { get; }

        public SqlColumnExpression(SqlColumnDefinition definition)
        {
            Definition = definition;
        }
    }
}