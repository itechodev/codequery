using CodeQuery.SqlExpressions;

namespace CodeQuery.SqlGenerators
{
    public interface ISqlGenerator
    {
        string Select(SqlSelectQuery query);
        // Update, Delete, Insert
    }
}