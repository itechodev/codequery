namespace codequery.SqlExpressions
{
    // @name
    public class VariableSqlExpression: SqlExpression
    {
        public VariableSqlExpression(string name)
        {
            Name = name;
        }

        public string Name { get; }
        // public ColumnType Type { get; }
    }

}