namespace CodeQuery.SqlGenerators
{
    public interface ISqlGenerator
    {
        string Select(SqlQuerySelect query);
        // Update, Delete, Insert
    }
}