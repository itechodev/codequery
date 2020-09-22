namespace CodeQuery.SqlExpressions
{
    public class SqlBinaryExpression
    {
        public ISqlExpression Left { get;  }
        public ISqlExpression Right { get;  }
        public SqlBinaryOperator Operator { get; set; }

        public SqlBinaryExpression(ISqlExpression left, ISqlExpression right, SqlBinaryOperator @operator)
        {
            Left = left;
            Right = right;
            Operator = @operator;
        }
    }
}