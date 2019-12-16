namespace codequery.SqlExpressions
{
    public enum SqlFunctionType
    {
        Coalesce
    }
    
    public class FunctionSqlExpression : SqlExpression
    {
        public SqlFunctionType Type { get; }
        public SqlExpression[] Parameters { get; }

        public FunctionSqlExpression(SqlFunctionType type, params SqlExpression[] parameters)
        {
            Type = type;
            Parameters = parameters;
        }
    }
}