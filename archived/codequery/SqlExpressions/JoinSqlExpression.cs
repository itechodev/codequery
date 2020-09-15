namespace codequery.SqlExpressions
{
    public enum JoinType
    {
        Inner,
        Left,
        Right,
        Outer,
        Cross
    }

    public class JoinSqlExpression : SqlExpression
    {
        public BooleanSqlExpression On { get; }
        public SourceSqlExpression Source { get; }
        public JoinType Type { get; set; }
    }
}