using codequery.Definitions;

namespace codequery.SqlExpressions
{
    public abstract class SqlExpression
    {

    }

    public class ConstSqlExpression
    {
        public ColumnType Type { get; set; }
        public object Value { get; set; }

        public ConstSqlExpression(object value, ColumnType? type = null)
        {            
            Value = value;
            Type = type.HasValue ? type.Value : ColumnDefinition.FromType(value.GetType());
        }
    }
}