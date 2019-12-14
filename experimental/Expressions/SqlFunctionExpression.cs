namespace codequery.Expressions
{
        // Inmature list. To be added
    public enum FieldFunctionType
    {
        Lower,
        Upper,
        Substring,
        LeftTrim,
        RightTrim,
        Trim,
        Length,
        Replace
    }
    
    public class SqlFunctionExpression : SqlCallbackExpression
    {
        public SqlFunctionExpression(FieldType type, SqlExpression body, FieldFunctionType function, params SqlExpression[] arguments) : base(type, body, arguments)
        {
            this.Function = function;

        }
        public FieldFunctionType Function { get; set; }
    }

}
