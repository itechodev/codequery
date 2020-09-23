using System;

namespace CodeQuery.SqlExpressions
{
    public class SqlSelectExpression : SqlExpression
    {
        public SqlExpression Body { get; }
        public int Order { get; }
        public string Alias { get; }
        public Type ReflectedType { get; }

        public SqlSelectExpression(SqlExpression body, int order, string @alias, Type reflectedType)
        {
            Body = body;
            Order = order;
            Alias = alias;
            ReflectedType = reflectedType;
        }
    }
}