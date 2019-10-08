namespace codequery.Expressions
{
    public enum JoinType
    {
        Inner,
        Left,
        Rigth,
        Full,
        Cross
    }
    public class SqlJoinClause
    {
        public string JoinType { get; set; }
        public SqlExpression OnClause { get; set; }
        public SqlQuerySource Source { get; set; }
    }

}
