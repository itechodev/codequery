using System;
using System.Linq.Expressions;
using System.Reflection;
using CodeQuery.SqlExpressions;

namespace CodeQuery.Translators
{
    internal class SqlStringMethodTranslator : ISqlMethodTranslator
    {
        public bool ForMethod(MethodInfo info)
        {
            return info.ReflectedType == typeof(string);
        }

        public SqlExpression Parse(MethodInfo info, SqlExpression body, MethodCallExpression method, Func<Expression, SqlExpression> resolver)
        {
            switch (info.Name)
            {
                case nameof(string.Trim):
                    // trim does not have any arguments
                    return new SqlFunctionExpression(SqlFunctionType.Trim, new[] {body});
                case nameof(string.Substring):
                    // substring(string [from <str_pos>] [for <ext_char>])
                    var args = new[]
                    {
                        body,
                        resolver.Invoke(method.Arguments.IndexOrDefault(0)),
                        resolver.Invoke(method.Arguments.IndexOrDefault(1))
                    };
                    return new SqlFunctionExpression(SqlFunctionType.Substr, args);
            }

            return null;
        }
    }
}