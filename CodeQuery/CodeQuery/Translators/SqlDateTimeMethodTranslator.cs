using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using CodeQuery.SqlExpressions;

namespace CodeQuery.Translators
{
    internal class SqlDateTimeMethodTranslator : ISqlMethodTranslator
    {
        public bool ForMethod(MethodInfo info)
        {
            return info.ReflectedType == typeof(DateTime);
        }

        public SqlExpression Parse(MethodInfo info, SqlExpression body, MethodCallExpression method, Func<Expression, SqlExpression> resolver)
        {
            switch (info.Name)
            {
                case nameof(DateTime.ToShortDateString):
                    return new SqlFunctionExpression(SqlFunctionType.TimestampToString,
                        (SqlConstExpression) CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern);
            }

            return null;
        }
    }
}