using System.Collections.Generic;

namespace codequery.SqlExpressions
{
    public class QuerySourceSqlExpression : SourceSqlExpression
    {
        public List<SqlExpression> Columns { get; set; } = new List<SqlExpression>();
        public SourceSqlExpression Source { get; set; }
        public List<JoinSqlExpression> Joins { get; set; } = new List<JoinSqlExpression>();
        public BooleanSqlExpression Where { get; set; }
        public List<SqlExpression> GroupBy { get; set; } = new List<SqlExpression>();
        public List<SqlExpression> OrderBy { get; set; }
        
        public int? Offset { get; set; }
        public int? Limit { get; set; }        
    }
}