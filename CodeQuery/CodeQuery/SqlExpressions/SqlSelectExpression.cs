namespace CodeQuery.SqlExpressions
{
    public class SqlSelectExpression : SqlExpression
    {
        public SqlExpression Body { get; }
        public int Order { get; }
        public string Alias { get; }

        public SqlSelectExpression(SqlExpression body, int order, string alias = null)
        {
            Body = body;
            Order = order;
            Alias = alias;
        }
    }
}