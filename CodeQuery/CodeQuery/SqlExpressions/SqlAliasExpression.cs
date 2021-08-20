namespace CodeQuery.SqlExpressions
{
    public class SqlAliasExpression : SqlExpression
    {
        public SqlExpression Exp { get; }
        public string Alias { get; }

        public SqlAliasExpression(SqlExpression exp, string alias)
        {
            Exp = exp;
            Alias = alias;
        }
    }
}