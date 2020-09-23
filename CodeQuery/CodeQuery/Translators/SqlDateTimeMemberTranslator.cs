using System;
using System.Linq.Expressions;
using CodeQuery.SqlExpressions;

namespace CodeQuery.Translators
{
    public class SqlDateTimeMemberTranslator : ISqlMemberTranslator
    {
        public bool ShouldRun(MemberExpression member)
        {
            return member.Member.ReflectedType == typeof(DateTime);
        }

        public SqlExpression Parse(MemberExpression method, Func<Expression, SqlExpression> resolver)
        {
            switch (method.Member.Name)
            {
                case nameof(DateTime.Now):
                    return new SqlFunctionExpression(SqlFunctionType.CurrentTimeStamp);
            }

            return null;
        }
    }
}