using System;
using System.Linq.Expressions;
using System.Reflection;
using CodeQuery.SqlExpressions;

namespace CodeQuery.Translators
{
    public interface ISqlMethodTranslator
    {
        bool ForMethod(MethodInfo info);
        SqlExpression Parse(MethodInfo info, SqlExpression body, MethodCallExpression method, Func<Expression, SqlExpression> resolver);
    }
}