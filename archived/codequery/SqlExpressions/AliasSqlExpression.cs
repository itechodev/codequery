namespace codequery.SqlExpressions
{
    public class AliasSqlExpression : SqlExpression
    {
        public string Name { get; }
        public SqlExpression Body { get; }
        
        public AliasSqlExpression(SqlExpression body, string name)
        {
            Name = name;
            Body = body;
        }
    }


}