namespace CodeQuery.SqlExpressions
{
    // all math, string, int and date functions
    // ie. abs, ceil, exp, floor, ln, log, mod, substring, trim etc.
    public class SqlFunctionExpression : SqlExpression
    {
        public SqlFunctionType Type { get; }
        public SqlExpression[] Arguments { get; }
        
        public SqlFunctionExpression(SqlFunctionType type, params SqlExpression[] arguments)
        {
            Type = type;
            Arguments = arguments;
        }
    }
}