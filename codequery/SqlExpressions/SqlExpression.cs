using codequery.Definitions;

namespace codequery.SqlExpressions
{
    public abstract class SqlExpression
    {
        public static ConstSqlExpression Const(object value)
        {
            return new ConstSqlExpression(value);
        }

        public static ColumnSqlExpression Column(ColumnDefinition definition)
        {
            return new ColumnSqlExpression(definition);
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

        public static MathSqlExpression Boolean(SqlExpression left, SqlExpression right, SqlMathOperator op)
        {
            return new MathSqlExpression(left, right, op);
        }
    }





    

}