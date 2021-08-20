using CodeQuery.SqlExpressions;

namespace CodeQuery.SqlGenerators
{
    public interface ISqlGenerator
    {
        string Select(SqlSelectQuery selectQuery);
        // Update, Delete, Insert
    }
}