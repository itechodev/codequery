using codequery.Definitions;

namespace codequery.SqlExpressions
{
    public class TableSourceSqlExpression: SourceSqlExpression
    {
        public TableSourceSqlExpression(string name)
        {
            Name = name;
        }
        
        public string Name { get; }
    }
}