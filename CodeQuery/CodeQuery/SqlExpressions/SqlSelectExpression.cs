namespace CodeQuery.SqlExpressions
{
    public class SqlSelectExpression
    {
        public ISqlExpression Body { get; }
        public int Order { get; }
        public string Alias { get; }

        public SqlSelectExpression(ISqlExpression body, int order, string alias = null)
        {
            Body = body;
            Order = order;
            Alias = alias;
        }
    }
}