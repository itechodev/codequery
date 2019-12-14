namespace codequery.Expressions
{
     public enum FieldMathOperator
    {
        Plus,
        Minus,
        Multiply,
        Divide,
        Equal,
        GreaterThan,
        LessThan,
        GreaterEqualThan,
        LessEqualThan,
        Or,
        And,
        StringConcat
    }


    public class SqlMathExpression : SqlExpression
    {
        public SqlExpression Left { get; private set; }
        public SqlExpression Right { get; private set; }
        public FieldMathOperator Op { get; private set; }

        public SqlMathExpression(FieldType type, SqlExpression left, FieldMathOperator op, SqlExpression right) : base(type)
        {
            Left = left;
            Right = right;
            Op = op;
        }
    }

}
