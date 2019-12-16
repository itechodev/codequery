using codequery.Definitions;

namespace codequery.SqlExpressions
{
    public class ConstSqlExpression : SqlExpression
    {
        public ColumnType Type { get; }
        public object Value { get; }

        public ConstSqlExpression(object value, ColumnType? type = null)
        {            
            Value = value;
            Type = type.HasValue ? type.Value : ColumnDefinition.FromType(value.GetType());
        }
    }
}