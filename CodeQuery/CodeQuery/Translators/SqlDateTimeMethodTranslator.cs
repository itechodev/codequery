using System;
using System.Globalization;
using System.Linq.Expressions;
using CodeQuery.SqlExpressions;

namespace CodeQuery.Translators
{
    internal class SqlDateTimeMethodTranslator : ISqlMethodTranslator
    {

        public bool ShouldRun(MethodCallExpression info)
        {
            return info.Method.ReflectedType == typeof(DateTime);
        }

        public SqlExpression Parse(MethodCallExpression method, SqlExpression body, Func<Expression, SqlExpression> resolver)
        {
            switch (method.Method.Name)
            {
                case nameof(DateTime.ToShortDateString):
                    return new SqlFunctionExpression(SqlFunctionType.TimestampToString,
                        (SqlConstExpression) CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern);
            }

            return null;
        }
    }
}