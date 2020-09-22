using CodeQuery.Definitions;

namespace CodeQuery.SqlExpressions
{
    public class SqlConstExpression : SqlExpression
    {
        public object Value { get; }
        public SqlColumnType Type { get;  }

        public SqlConstExpression(object value, SqlColumnType type)
        {
            Value = value;
            Type = type;
        }
    }
}