namespace codequery.Expressions
{
    public class SqlSelectQuery
    {
        public SqlSelectField[] Fields { get; set; }
        public SqlQuerySource From { get; set; }
        public SqlJoinClause[] Joins { get; set; }
        public SqlExpression Where { get; set; }
        public SqlExpression[] GroupBy { get; set; }
        public SqlOrderByClause[] OrderBy { get; set; }
    }

}
