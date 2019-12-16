using codequery.Definitions;

namespace codequery.SqlExpressions
{
    public abstract class SqlExpression
    {
        public static ConstSqlExpression Const(object value)
        {
            return new ConstSqlExpression(value);
        }

        public static ColumnSqlExpression Column(string name)
        {
            return new ColumnSqlExpression(name);
        }

        public static AliasSqlExpression Alias(SqlExpression body, string name)
        {
            return new AliasSqlExpression(body, name);
        }

        public static BooleanSqlExpression Boolean(SqlExpression left, SqlExpression right, SqlBooleanOperator op)
        {
            return new BooleanSqlExpression(left, right, op);
        }

        public static VariableSqlExpression Variable(string name)
        {
            return new VariableSqlExpression(name);
        }

        public static MathSqlExpression Math(SqlExpression left, SqlExpression right, SqlMathOperator op)
        {
            return new MathSqlExpression(left, right, op);
        }

        public static AggregateSqlExpression Aggregate(SqlAggregateType type, params SqlExpression[] parameters)
        {
            return new AggregateSqlExpression(type, parameters);
        }

        public static FunctionSqlExpression Function(SqlFunctionType type, params SqlExpression[] parameters)
        {
            return new FunctionSqlExpression(type, parameters);
        }

        public static AllSqlExpression All()
        {
            return new AllSqlExpression();
        }

        public static ConstSourceSqlExpression ConstSource()
        {
            return new ConstSourceSqlExpression();
        }

        public static TableSourceSqlExpression TableSource(string name)
        {
            return new TableSourceSqlExpression(name);
        }

        public static QuerySourceSqlExpression QuerySource()
        {
            return new QuerySourceSqlExpression();
        }

    }
}