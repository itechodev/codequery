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

        public static implicit operator SqlConstExpression(int value)
        {
            return new SqlConstExpression(value, SqlColumnType.Int32);
        }
        
        public static implicit operator SqlConstExpression(string value)
        {
            return new SqlConstExpression(value, SqlColumnType.Varchar);
        }
    }
}