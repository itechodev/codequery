using System;
using System.Collections.Generic;

namespace CodeQuery.SqlExpressions
{
    // A list of expression 
    // like: select exp1, exp2, exp3 from...
    public class SqlSelectListExpression : SqlExpression
    {
        public Type Type { get; set; }
        public List<SqlSelectExpression> Expressions { get; private set; } = new List<SqlSelectExpression>();

        public SqlSelectListExpression(Type type)
        {
            Type = type;
        }

        public void Add(SqlSelectExpression exp)
        {
            Expressions.Add(exp);
        }
    }
}