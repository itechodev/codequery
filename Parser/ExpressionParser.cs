using System;
using System.Linq;
using System.Linq.Expressions;
using codequery.Expressions;

namespace codequery.Parser
{
    public class ExpressionParser
    {
        public FieldExpression ParseField(Expression exp, string alias = null)
        {
            // .Select(s => s)
            // QuoteExpression
            // if (exp is UnaryExpression un)
            // {
            //     // un.Method?
            //     return ParseField(un.Operand);
            // }
            // // u => u.Name == "Willy"
            // if (exp is LambdaExpression lambda)
            // {
            //     return ParseLamda(lambda);
            // }
            // // new { ... }
            // if (exp is NewExpression newx)
            // {
            //     var list = newx.Arguments.Select((a,i) => ParseField(a, newx.Members[i].Name)).ToArray();
            //     return new FieldList(list);
            // }
            // if (exp is ConstantExpression constant)
            // {
            //     return ParseConstant(constant);
            // }
            // // u.Name
            // if (exp is MethodCallExpression call)
            // {
            //     return ParseMethodCall(call);
            // }
            // if (exp is MemberExpression member)
            // {
            //     // member.Member.Name == "name"
            //     if (member.Expression is ParameterExpression p)
            //     {
            //         // If Group by Field then .Key = grouped field
            //         if (_query.From is FromGroup group)
            //         {
            //             if (_query.GroupBy.Count() == 1)
            //             {
            //                 return _query.GroupBy.First();
            //             }
            //             return new FieldList(_query.GroupBy);
            //         }
            //         // member.Member
            //         return new FieldName(member.Member.Name, GetSourceFromType(p.Type));
            //     }
            // }
            // if (exp is BinaryExpression bin)
            // {
            //     return ParseBinaryExpression(bin);
            // }
            throw new NotImplementedException();
        }
    }
}