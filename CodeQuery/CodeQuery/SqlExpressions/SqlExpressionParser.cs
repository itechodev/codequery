using System;
using System.Linq.Expressions;
using CodeQuery.Definitions;

namespace CodeQuery.SqlExpressions
{
    public static class SqlExpressionParser
    {
        public static SqlExpression Parse(Expression expression)
        {
            switch (expression)
            {
                case ConstantExpression constant:
                    return ParseConstant(constant);
                case BinaryExpression binary:
                    return ParseBinaryExpression(binary);
                default:
                    throw new NotImplementedException();
            }
        }

        private static SqlExpression ParseBinaryExpression(BinaryExpression binary)
        {
            return new SqlBinaryExpression(
                Parse(binary.Left), 
                Parse(binary.Right), 
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