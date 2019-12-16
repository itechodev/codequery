using codequery.Definitions;

namespace codequery.SqlExpressions
{
    public class ColumnSqlExpression : SqlExpression
    {
        public ColumnDefinition Definition { get; set; }

        public ColumnSqlExpression(ColumnDefinition definition)
        {
            Definition = definition;
        }
    }
}