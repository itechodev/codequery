namespace codequery.Expressions
{
    public class SqlOrderByClause
    {
        public SqlColumnExpression By { get; set; }
        public bool Ascending { get; set; } = true;
    }

}
