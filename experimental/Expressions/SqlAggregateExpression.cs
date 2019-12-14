namespace codequery.Expressions
{
     // List of aggregate functions codeQuery supports
    // Item to be added
    public enum AggregateFunction
    {
        Count,
        Sum,
        Min,
        Max,
        Average
    }

    // count(a.id), sum(b.value)
    public class SqlAggregateExpression : SqlCallbackExpression
    {
        public SqlAggregateExpression(FieldType type, SqlExpression body, AggregateFunction function, params SqlExpression[] arguments) : base(type, body, arguments)
        {
            this.Function = function;
        }

        public AggregateFunction Function { get; set; }
    }

}
