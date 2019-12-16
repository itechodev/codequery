namespace codequery.SqlExpressions
{

    public enum SqlMathOperator
    {
        Plus,
        Minus,
        Divide,
        Multiple,
        Power,
        Mod,
    }
    
    public class MathSqlExpression : SqlExpression
    {
        public MathSqlExpression(SqlExpression left, SqlExpression right, SqlMathOperator @operator)
        {
            Left = left;
            Right = right;
            Operator = @operator;
        }

        public SqlExpression Left { get; }
        public SqlExpression Right { get; }
        public SqlMathOperator Operator { get; }
    }

}