using System.Collections.Generic;

namespace CodeQuery.SqlExpressions
{
    public class SqlSelectQuery : SqlExpression
    {
        public List<SqlExpression> Fields { get; set; }
        public SqlSource From { get; set; }
        public List<SqlJoinSource> Joins { get; set; }

        public SqlBinaryExpression Where { get; set; }
        public List<SqlExpression> GroupBy { get; set; }
        public List<SqlExpression> OrderBy { get; set; }

        public SqlSelectQuery(SqlSource @from)
        {
            From = @from;
        }
    }
}