using System;
using System.Linq.Expressions;
using CodeQuery.SqlExpressions;

namespace CodeQuery.Translators
{
    public interface ISqlMethodTranslator
    {
        bool ShouldRun(MethodCallExpression info);
        SqlExpression Parse(MethodCallExpression method, SqlExpression body, Func<Expression, SqlExpression> resolver);
    }
}