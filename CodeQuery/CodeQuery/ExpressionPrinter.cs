using System;
using System.Linq.Expressions;

namespace CodeQuery
{
    public static class ExpressionPrinter
    {
        private static void Log(string message, int indent)
        {
            Console.WriteLine(new string(' ', indent * 4) + message);
        }

        public static void Print(Expression exp, int indent = 0)
        {
            switch (exp)
            {
                case BinaryExpression binary:
                    Log("Binary:", indent);
                    Log("Left", indent + 1);
                    Print(binary.Left, indent + 1);
                    Log("Right", indent + 1);
                    Print(binary.Right, indent + 1);
                    break;
                case BlockExpression block:
                    Log("Block:", indent);
                    break;
                case ConditionalExpression conditional:
                    Log("Conditional:", indent);
                    break;

                case ConstantExpression constant:
                    Log($"Constant: {constant.Value}", indent);
                    break;

                case DebugInfoExpression debugInfo:
                    Log("DebugInfo:", indent);
                    break;

                case DefaultExpression @default:
                    Log("Default:", indent);
                    break;

                case DynamicExpression dynamic:
                    Log("Dynamic:", indent);
                    break;

                case GotoExpression @goto:
                    Log("Goto:", indent);
                    break;

                case IndexExpression index:
                    Log("Index:", indent);
                    break;

                case InvocationExpression invocation:
                    Log("Invocation:", indent);
                    break;

                case LabelExpression label:
                    Log("Label:", indent);
                    break;

                case LambdaExpression lambda:
                    Log($"Lambda Expression {lambda}", indent);
                    Log("Parameters:", indent);
                    foreach (var p in lambda.Parameters)
                    {
                        Print(p, indent + 1);
                    }

                    Print(lambda.Body, indent + 1);
                    break;
                case ListInitExpression listInit:
                    Log("ListInit:", indent);
                    break;
                case LoopExpression loop:
                    Log("Loop:", indent);
                    break;
                case MemberExpression member:
                    Log($"Member: {member.Member.Name}, type: {member.Member.ReflectedType}", indent);
                    if (member.Expression != null)
                    {
                        Print(member.Expression, indent + 1);
                    }
                    break;
                case MemberInitExpression memberInit:
                    Log("MemberInit:", indent);
                    break;
                case MethodCallExpression methodCall:
                    Log("MethodCall:", indent);
                    break;
                case NewArrayExpression newArray:
                    Log("New array expression:", indent);
                    break;
                case NewExpression @new:
                    Log("New:", indent);
                    // return a new type
                    // new { .. }
                    // new Constructor()
                    foreach (var arg in @new.Arguments)
                    {
                        Print(arg, indent + 1);
                    }
                    break;
                case ParameterExpression parameter:
                    Log($"With parameter {parameter.Name}: {parameter.Type.Name}", indent);
                    break;
                case RuntimeVariablesExpression runtimeVariables:
                    Log("RuntimeVariables:", indent);
                    break;
                case SwitchExpression @switch:
                    Log("Switch:", indent);
                    break;
                case TryExpression @try:
                    Log("Try:", indent);
                    break;
                case TypeBinaryExpression typeBinary:
                    Log("TypeBinary:", indent);
                    break;
                case UnaryExpression unary:
                    Log("Unary:", indent);
                    break;
                default:
                    Log($"Unknown expression: {exp.Type}", indent);
                    break;
            }
        }
    }
}