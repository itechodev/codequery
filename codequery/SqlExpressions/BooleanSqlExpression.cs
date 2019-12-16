namespace codequery.SqlExpressions
{

    public enum SqlBooleanOperator
    {
        And,
        Or,
        Equal,
        NotEqual,
        GreaterThen,
        NotGreaterThen,
        LessThen,
        NotLessThen,
        GreateEqualThan,
        NotGreateEqualThan,
        LessEqualThan,
        NotLessEqualThan,
        Like,
        NotLike
    }
    
    public class BooleanSqlExpression : SqlExpression
    {
        public BooleanSqlExpression(SqlExpression left, SqlExpression right, SqlBooleanOperator @operator)
        {
            Left = left;
            Right = right;
            Operator = @operator;
        }

        public SqlExpression Left { get; }
        public SqlExpression Right { get; }
        public SqlBooleanOperator Operator { get; }

    }

}