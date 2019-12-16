namespace codequery.SqlExpressions
{

    public enum SqlBooleanOperator
    {
        And,
        Or,
        Xor,
        Equal,
        GreaterThen,
        LessThen,
        GreateEqualThan,
        LessEqualThan,
        Like
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