namespace codequery.Expressions
{
    public enum FieldRowFunctionType
    {
        Distinct
        // Don't know of any any row functions yet
    }

    // Not the same a function
    public class SqlRowFunctionExpression : SqlCallbackExpression
    {
        public SqlRowFunctionExpression(FieldType type, SqlExpression body, FieldRowFunctionType function, params SqlExpression[] arguments) : base(type, body, arguments)
        {
            this.Function = function;
        }

        public FieldRowFunctionType Function { get; set; }
    }

}
