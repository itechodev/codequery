namespace CodeQuery.SqlExpressions
{
    public class SqlBinaryExpression : SqlExpression
    {
        public SqlExpression Left { get;  }
        public SqlExpression Right { get;  }
        public SqlBinaryOperator Operator { get; set; }

        public SqlBinaryExpression(SqlExpression left, SqlExpression right, SqlBinaryOperator @operator)
        {
            Left = left;
            Right = right;
            Operator = @operator;
        }
    }
}