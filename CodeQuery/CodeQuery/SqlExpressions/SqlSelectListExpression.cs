using System;
using System.Collections.Generic;

namespace CodeQuery.SqlExpressions
{
    // A list of expression 
    // like: select exp1, exp2, exp3 from...
    public class SqlSelectListExpression : SqlExpression
    {
        public Type Type { get; set; }
        public List<SqlSelectQuery> Expressions { get; private set; } = new List<SqlSelectQuery>();

        public SqlSelectListExpression(Type type)
        {
            Type = type;
        }

        public void Add(SqlSelectQuery exp)
        {
            Expressions.Add(exp);
        }
    }
}