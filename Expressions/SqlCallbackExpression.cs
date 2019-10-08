namespace codequery.Expressions
{
    public abstract class SqlCallbackExpression : SqlExpression
    {
        public SqlCallbackExpression(FieldType type, SqlExpression body, params SqlExpression[] arguments) : base(type)
        {
            this.Arguments = arguments;
            this.Body = body;
        }

        public SqlExpression[] Arguments { get; set; }
        public SqlExpression Body { get; set; }
    }

}
