using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using CodeQuery.Definitions;

namespace CodeQuery.SqlExpressions
{
    
    public static class SqlExpressionParser
    {
        public static SqlExpression Parse(Expression expression, List<SqlSource> sources)
        {
            if (expression == null)
            {
                return null;
            }
            
            switch (expression)
            {
                // 55
                case ConstantExpression constant:
                    return ParseConstant(constant);
                // [Exp] + [Expr]
                case BinaryExpression binary:
                    return ParseBinaryExpression(binary, sources);
                // t.Column
                case MemberExpression member:
                    return ParseMemberExpression(member, sources);
                // t.Column.Substr(50).Trim()
                case MethodCallExpression call:
                    return ParseMethodExpression(call, sources);
                    
                default:
                    throw new NotImplementedException();
            }
        }

        private static SqlExpression ParseMethodExpression(MethodCallExpression call, List<SqlSource> sources)
        {
            // String functions
            if (call.Method.ReflectedType == typeof(string))
            {
                // = t.Added.ToShortDateString().Substring(5) .Trim()
                // Trim( Substr(ToShortDateString(t."Added"), 5))
                var arg = Parse(call.Object, sources);

                switch (call.Method.Name)
                {
                    case nameof(string.Trim):
                        // trim does not have any arguments
                        return new SqlFunctionExpression(SqlFunctionType.Trim, new[] {arg});
                    case nameof(string.Substring):
                        // substring(string [from <str_pos>] [for <ext_char>])
                        var args = new[]
                        {
                            arg,
                            Parse(call.Arguments.IndexOrDefault(0), sources),
                            Parse(call.Arguments.IndexOrDefault(1), sources)
                        };
                        return new SqlFunctionExpression(SqlFunctionType.Substr, args);
                }
            }

            if (call.Method.ReflectedType == typeof(int))
            {
                // cast etc.
            }
            
            throw new SqlExpressionException($"Could not translate method {call.Method.Name} to a SQL expression");
        }


        private static SqlExpression ParseMemberExpression(MemberExpression member, List<SqlSource> sources)
        {
            // if (member.NodeType == ExpressionType.MemberAccess)
            // Maybe there are anything other than memberAccess
            if (member.NodeType != ExpressionType.MemberAccess)
            {
                throw new SqlExpressionException(
                    $"Could not translate {member.Expression} to a SQL expression. Expression type is of {member.NodeType}.");
            }
            
            // Date functions
            if (member.Member.ReflectedType == typeof(System.DateTime))
            {
                switch (member.Member.Name)
                {
                    case "Now":
                        return new SqlFunctionExpression(SqlFunctionType.CurrentTimeStamp, null);
                }
            }

            // Static Math functions
            if (member.Member.ReflectedType == typeof(System.Math))
            {
                
            }
            
            // Static string functions like String.IsNullOrEmpty
            if (member.Member.ReflectedType == typeof(string))
            {
                
            }
            
            // else it is a field (column)
            return new SqlColumnExpression(GetReferenceFieldDef(member.Member, sources));
        }
        
        private static SqlColumnDefinition GetReferenceFieldDef(MemberInfo info, List<SqlSource> sources)
        {
            var source = sources.Find(s => s.ReflectedType == info.ReflectedType);
            if (source == null)
            {
                throw new SqlExpressionException($"Could not translate property {info.Name} into a SQL source.");
            }

            var col = source.Columns.Find(c => c.Name == info.Name);
            if (col == null)
            {
                throw new SqlExpressionException($"Could not translate property {info.Name} into a SQL source.");
            }
            return col;
        }

        private static SqlExpression ParseBinaryExpression(BinaryExpression binary, List<SqlSource> sources)
        {
            return new SqlBinaryExpression(
                Parse(binary.Left, sources), 
                Parse(binary.Right, sources), 
                ToSqlBinaryOperator(binary.NodeType)
            );
        }

        private static SqlBinaryOperator ToSqlBinaryOperator(ExpressionType binaryNodeType)
        {
            switch (binaryNodeType)
            {
                case ExpressionType.Add:
                    return SqlBinaryOperator.Plus;
                case ExpressionType.And:
                    return SqlBinaryOperator.And;
                case ExpressionType.Coalesce:
                    return SqlBinaryOperator.Coalesce;
                // clause ? true : false 
                // case ExpressionType.Conditional:
                //     break;
                // case ExpressionType.Convert:
                //     break;
                // case ExpressionType.Decrement:
                //     // Assume x--?
                //     break;
                // case ExpressionType.Default:
                //     break;
                case ExpressionType.Divide:
                    return SqlBinaryOperator.Divide;
                // case ExpressionType.Equal:
                //     break;
                case ExpressionType.ExclusiveOr:
                    return SqlBinaryOperator.Xor;
                case ExpressionType.GreaterThan:
                    return SqlBinaryOperator.GreaterThan;
                case ExpressionType.GreaterThanOrEqual:
                    return SqlBinaryOperator.GreaterThanOrEqual;
                // case ExpressionType.Increment:
                //     break;
                case ExpressionType.LeftShift:
                    return SqlBinaryOperator.BitwiseShiftLeft;
                case ExpressionType.LessThan:
                    return SqlBinaryOperator.LessThan;
                case ExpressionType.LessThanOrEqual:
                    return SqlBinaryOperator.LessThanOrEqual;
                case ExpressionType.Modulo:
                    return SqlBinaryOperator.Modulo;
                case ExpressionType.Multiply:
                    return SqlBinaryOperator.Multiply;
                case ExpressionType.Negate:
                case ExpressionType.Not:
                case ExpressionType.NotEqual:
                    return SqlBinaryOperator.Not;
                case ExpressionType.OnesComplement:
                    // C# ~ operator
                    return SqlBinaryOperator.OnesComplement;
                case ExpressionType.Or:
                    return SqlBinaryOperator.Or;
                case ExpressionType.Power:
                    return SqlBinaryOperator.Power;
                case ExpressionType.RightShift:
                    return SqlBinaryOperator.BitwiseShiftRight;
                case ExpressionType.Subtract:
                    return SqlBinaryOperator.Subtract;
                // Casting and converting?
                // case ExpressionType.Unbox:
                //     break;
                default:
                    throw new SqlExpressionException($"Could not convert binary operator {binaryNodeType} into a SqlExpression.");
            }
        }

        private static SqlExpression ParseConstant(ConstantExpression constant)
        {
            return new SqlConstExpression(constant.Value, DefinitionHelper.InferType(constant.Type));
        }
    }
}