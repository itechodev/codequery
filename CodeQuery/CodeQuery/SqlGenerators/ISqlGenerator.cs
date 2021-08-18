using CodeQuery.SqlExpressions;

namespace CodeQuery.SqlGenerators
{
    public interface ISqlGenerator
    {
        string Select(SqlSelectExpression query);
        // Update, Delete, Insert
    }
}