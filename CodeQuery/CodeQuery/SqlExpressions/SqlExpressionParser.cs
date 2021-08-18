using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CodeQuery.Definitions;
using CodeQuery.Translators;

namespace CodeQuery.SqlExpressions
{
    
    public static class SqlExpressionParser
    {

        private static readonly List<ISqlMethodTranslator> MethodTranslators;
        private static readonly List<ISqlMemberTranslator> MemberTranslators;

        static SqlExpressionParser()
        {
            MethodTranslators = new List<ISqlMethodTranslator>
            {
                new SqlStringMethodTranslator(),
                new SqlDateTimeMethodTranslator()
            };

            MemberTranslators = new List<ISqlMemberTranslator>
            {
                new SqlDateTimeMemberTranslator()
            };
        }

        public static SqlExpression Parse(Expression expression, params SqlSource[] sources)
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
                case NewExpression @new:
                    return ParseNewExpression(@new, sources);
                // (a, b) => ...
                case LambdaExpression lambda:
                    // var parameters = lambda.Parameters.Select(p => Parse(p))
                    return Parse(lambda.Body, sources);
                default:
                    throw new NotImplementedException();
            }
        }

        private static SqlExpression ParseNewExpression(NewExpression @new, SqlSource[] sources)
        {
            // Only default constructors are allowed. 
            // Blocked by the generic constraint
            var ret = new SqlSelectListExpression(@new.Constructor.ReflectedType);
            // new { Member = Argument, ..};
            // for (var i = 0; i < @new.Arguments.Count; i++)
            // {
            //     var exp = Parse(@new.Arguments[i], sources);
            //     ret.Add(new SqlSelectExpression(
            //         exp,
            //         i,
            //         @new.Members[i].Name,
            //         GetMemberType(@new.Members[i])
            //     ));
            // }

            return ret;
        }

        private static Type GetMemberType(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new SqlExpressionException($"Cannot determine type for {member.Name}");
            }
        }

        private static SqlExpression ParseMethodExpression(MethodCallExpression call, SqlSource[] sources)
        {
            // Run through translators
            foreach (var translator in MethodTranslators)
            {
                if (!translator.ShouldRun(call)) continue;
                
                var body = Parse(call.Object, sources);
                var ret = translator.Parse(call, body, expression => Parse(expression, sources));
                if (ret != null)
                {
                    return ret;
                }
                // else continue
            }
            
            throw new SqlExpressionException($"Could not translate method {call.Method.Name} to a SQL expression.");
        }


        private static SqlExpression ParseMemberExpression(MemberExpression member, SqlSource[] sources)
        {
            // if (member.NodeType == ExpressionType.MemberAccess)
            // Maybe there are anything other than memberAccess
            if (member.NodeType != ExpressionType.MemberAccess)
            {
                throw new SqlExpressionException(
                    $"Could not translate {member.Expression} to a SQL expression. Expression type is of {member.NodeType}.");
            }
            
            // Run through translators
            foreach (var translator in MemberTranslators)
            {
                if (!translator.ShouldRun(member)) continue;

                var ret = translator.Parse(member, expression => Parse(expression, sources));
                if (ret != null)
                {
                    return ret;
                }
            }
            
            // else it is a field (column)
            return new SqlColumnExpression(GetReferenceFieldDef(member.Member, sources));
        }
        
        private static SqlColumnDefinition GetReferenceFieldDef(MemberInfo info, SqlSource[] sources)
        {
            var source = sources.ToList().Find(s => s.ReflectedType == info.ReflectedType);
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

        private static SqlExpression ParseBinaryExpression(BinaryExpression binary, SqlSource[] sources)
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