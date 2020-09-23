using System;
using System.Linq.Expressions;
using CodeQuery.SqlExpressions;

namespace CodeQuery.Translators
{
    public interface ISqlMemberTranslator
    {
        bool ShouldRun(MemberExpression member);
        SqlExpression Parse(MemberExpression method, Func<Expression, SqlExpression> resolver);
    }
}