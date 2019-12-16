using System.Collections.Generic;

namespace codequery.SqlExpressions
{
    public class ConstSourceSqlExpression : SourceSqlExpression
    {
        public List<ConstSqlExpression> Columns { get; set; }
    }
}