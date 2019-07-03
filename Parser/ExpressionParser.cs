using System;
using System.Linq;
using System.Linq.Expressions;
using codequery.Expressions;

namespace codequery.Parser
{
    public class QuerySourceType
    {
        public SqlQuerySource Source { get; set; }
        public Type Type { get; set; }
    }
    public class ParserScope
    {

        public ParserScope(ParameterExpression[] parameters)
        {
            Parameters = parameters;
        }

        public ParameterExpression[] Parameters { get; set; }
        public QuerySourceType[] QuerySourceTypes  { get; set; }

        public ParameterExpression FindParamByType(Type type)
        {
            return Parameters.FirstOrDefault(p => p.Type == type);
        }

        public SqlQuerySource GetSourceByType(Type type)
        {
            var qs = QuerySourceTypes.FirstOrDefault(q => q.Type == type);
            return qs?.Source;
        }
    }

    public class ExpressionParser
    {
        private SelectQuery _query;

        public ExpressionParser(SelectQuery query)
        {
            _query = query;
        }

        public SqlExpression ToSqlExpression(Expression exp, ParserScope scope = null)
        {
            if (exp is System.Linq.Expressions.ConstantExpression constant)
            {
                return ParseConstant(constant);
            }
            // params... => body
            if (exp is LambdaExpression lambda)
            {
                // save original parameters
                var newScope = new ParserScope(lambda.Parameters.ToArray());
                return ToSqlExpression(lambda.Body, newScope);
            }
            // .Select(s => s)
            // QuoteExpression
            if (exp is UnaryExpression un)
            {
                // un.Method?
                return ToSqlExpression(un.Operand);
            }
            // new { ... }
            if (exp is NewExpression newx)
            {
            //     newx.Arguments
            //     var list = newx.Arguments.Select((a,i) => ParseField(a, newx.Members[i].Name)).ToArray();
            //     return new FieldList(list);
            }
            
            // x => [x.Active]
            if (exp is MemberExpression member)
            {
                if (member.Expression is ParameterExpression param)
                {
                    // s => [s].Active
                    var memberName = member.Member.Name;
                    var source = scope.GetSourceByType(param.Type);
                    // Get Query Sourve by memberExpression
                    return new SqlColumnExpression(source.GetColumnType(memberName), memberName, source);
                    // param.Name ==
                    // param.Type
                }

                throw new NotImplementedException();   
            }

            // u.Name
            if (exp is MethodCallExpression call)
            {
                return ParseMethodCall(call, scope);
            }
            if (exp is BinaryExpression bin)
            {
                return ParseBinaryExpression(bin, scope);
            }
            throw new NotImplementedException();
        }
       
        
        private SqlQuerySource GetSourceFromType(Type type)
        {
            // if (type == _query.From.Type)
            // {
            //     return _query.From;
            // }
            // if (_query.From is FromJoin join)
            // {
            //     if (type == join.Left.Type)
            //     {
            //         return join.Left;
            //     }
            //     if (type == join.Right.Type)
            //     {
            //         return join.Right;
            //     }
            // }
            throw new Exception("Could not determine type");
        }

        private SqlExpression ParseConstant(System.Linq.Expressions.ConstantExpression constant)
        {
            switch (constant.Value)
            {
                case char ch:
                    return new SqlConstantExpression(FieldType.Char, ch);
                case string str:
                    return new SqlConstantExpression(FieldType.String, str);
                case int i:
                    return new SqlConstantExpression(FieldType.Int, i);
                case double d:
                    return new SqlConstantExpression(FieldType.Double, d);
                case bool b:
                    return new SqlConstantExpression(FieldType.Bool, b);
            }
            throw new NotImplementedException();
        }

        private SqlExpression ParseMethodCall(MethodCallExpression call, ParserScope scope)
        {
            var body = ToSqlExpression(call.Object, scope);
            // check for special cases for casting
            if (call.Method.Name == "ToString" && call.Method.ReflectedType == typeof(string))
            {
                return new SqlCastExpression(FieldType.String, body);
            }
            // Parse all arguments
            var arguments = call.Arguments.Select(a => ToSqlExpression(a, scope)).ToArray();
            var (returnType, function) = ToStringFunctionType(call.Type, call.Method.Name);
            return new SqlFunctionExpression(returnType, body, function, arguments);
        }

        private (FieldType, FieldFunctionType) ToStringFunctionType(Type type, string name)
        {
            switch (name)
            {
                case "ToLower":
                    return (FieldType.String, FieldFunctionType.Lower);
                case "ToUpper":
                    return (FieldType.String, FieldFunctionType.Upper);
                case "Substring":
                    return (FieldType.String, FieldFunctionType.Substring);
            }
            throw new NotImplementedException();
        }

        public SqlMathExpression ParseBinaryExpression(BinaryExpression bin, ParserScope scope)
        {
            var left = ToSqlExpression(bin.Left, scope);
            var right = ToSqlExpression(bin.Right, scope);
            
            return new SqlMathExpression(left.FieldType, left, ParseOperator(left.FieldType, right.FieldType, bin.NodeType), right);
        }

        private FieldMathOperator ParseOperator(FieldType left, FieldType right, ExpressionType nodeType)
        {
            if (nodeType == ExpressionType.Equal)
            {
                return FieldMathOperator.Equal;
            }
            if (nodeType == ExpressionType.OrElse || nodeType == ExpressionType.Or)
            {
                return FieldMathOperator.Or;
            }
            if (nodeType == ExpressionType.Add)
            {
                // x + string == string
                if (left == FieldType.String || right == FieldType.String)
                {
                    return FieldMathOperator.StringConcat;
                }
                return FieldMathOperator.Plus;
            }
            if (nodeType == ExpressionType.GreaterThan)
            {
                return FieldMathOperator.GreaterThan;
            }
            if (nodeType == ExpressionType.GreaterThanOrEqual)
            {
                return FieldMathOperator.GreaterEqualThan;
            }
            if (nodeType == ExpressionType.LessThan)
            {
                return FieldMathOperator.LessThan;
            }
            if (nodeType == ExpressionType.LessThanOrEqual)
            {
                return FieldMathOperator.LessEqualThan;
            }

            if (nodeType == ExpressionType.Multiply)
            {
                return FieldMathOperator.Multiply;
            }
            if (nodeType == ExpressionType.Divide)
            {
                return FieldMathOperator.Divide;
            }
            if (nodeType == ExpressionType.Add)
            {
                return FieldMathOperator.Plus;
            }
            if (nodeType == ExpressionType.Subtract)
            {
                return FieldMathOperator.Minus;
            }

            throw new NotImplementedException();
        }
    }
}