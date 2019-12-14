namespace codequery.Expressions
{
    // Any expressino with an optional alias
    public class SqlSelectField
    {
        public SqlSelectField(SqlExpression exp, string alias)
        {
            Expression = exp;
            Alias = alias;
        }
        public string Alias { get; set; }
        public SqlExpression Expression { set; get; }
    }

}
