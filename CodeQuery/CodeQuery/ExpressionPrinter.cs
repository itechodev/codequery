using System.Linq.Expressions;

namespace CodeQuery
{
    public static class ExpressionPrinter
    {
        public static void PrintExpression(Expression exp, int indent = 0)
        {
            switch (exp)
            {
                case null:
                    break;
                case BinaryExpression binary:
                    break;
                case BlockExpression block:
                    break;
                case ConditionalExpression conditional:
                    break;
                case ConstantExpression constant:
                    break;
                case DebugInfoExpression debugInfo:
                    break;
                case DefaultExpression @default:
                    break;
                case DynamicExpression dynamic:
                    break;
                case GotoExpression @goto:
                    break;
                case IndexExpression index:
                    break;
                case InvocationExpression invocation:
                    break;
                case LabelExpression label:
                    break;
                case LambdaExpression lambda:
                    break;
                case ListInitExpression listInit:
                    break;
                case LoopExpression loop:
                    break;
                case MemberExpression member:
                    break;
                case MemberInitExpression memberInit:
                    break;
                case MethodCallExpression methodCall:
                    break;
                case NewArrayExpression newArray:
                    break;
                case NewExpression @new:
                    break;
                case ParameterExpression parameter:
                    break;
                case RuntimeVariablesExpression runtimeVariables:
                    break;
                case SwitchExpression @switch:
                    break;
                case TryExpression @try:
                    break;
                case TypeBinaryExpression typeBinary:
                    break;
                case UnaryExpression unary:
                    break;
            }
        }
    }
}