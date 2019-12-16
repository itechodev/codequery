namespace codequery.SqlExpressions
{

    public enum SqlAggregateType
    {
        Count,
        Sum,
        Min,
        Max,
        Average
    }

    public class AggregateSqlExpression : SqlExpression
    {
        public SqlAggregateType Type { get; }
        public SqlExpression[] Parameters { get; }

        public AggregateSqlExpression(SqlAggregateType type, params SqlExpression[] parameters)
        {
            Type = type;
            Parameters = parameters;
        }
    }
}