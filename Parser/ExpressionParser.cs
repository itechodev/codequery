using System;
using System.Linq;
using System.Linq.Expressions;
using codequery.Expressions;
using codequery.QuerySources;

namespace codequery.Parser
{
    // QuerySourceTypeType
    public enum SourceType
    {
        // a normal class instance. Cols as per class instance
        Instance,
        Aggregate
    }

    // Associate given type with members and functions that can be called
    // For instance: .Where(x => x.Caption.Containts("xx"))
    
    public class QuerySourceType
    {
        // s => 
        public QuerySourceType(SelectQuery source, Type type, SourceType sourceType)
        {
            this.Source = source;
            this.Type = type;
            this.SourceType = sourceType;
        }
        public SelectQuery Source { get; set; }
        public Type Type { get; set; }
        public SourceType SourceType { get; set; }
    }

    public class ExpressionParser
    {
        private QuerySourceType[] _sources;

        public ExpressionParser(QuerySourceType[] sources)
        {
            _sources = sources;
        }

        public SqlExpression ToSqlExpression(Expression exp)
        {
            if (exp is System.Linq.Expressions.ConstantExpression constant)
            {
                return ParseConstant(constant);
            }
            // params... => body
            if (exp is LambdaExpression lambda)
            {
                // save original parameters
                // var newScope = new ParserScope(lambda.Parameters.ToArray());
                return ToSqlExpression(lambda.Body);
            }
            // .Select(s => s)
            // QuoteExpression
            if (exp is UnaryExpression un)
            {
                // un.Method?
                return ToSqlExpression(un.Operand);
            }
            if (exp is MemberExpression member)
            {
                if (member.Expression is ParameterExpression param)
                {
                    // Check if accessing from a aggregate source

                    // s => [s].Active
                    var memberName = member.Member.Name;
                    var source = _sources.First(s => s.Type == param.Type);
                    var querySource = source?.Source.From;

                    if (source.SourceType == SourceType.Instance)
                    {
                        // Check for aggregate value's and functions
                        // x => x.Value
                        if (memberName == "Value") 
                        {
                            // return new SqlColumnExpression(querySource.GetColumnType(memberName))
                            return null;
                        }
                    }

                    
                    // Get Query Sourve by memberExpression
                    return new SqlColumnExpression(querySource.GetColumnType(memberName), memberName, querySource);
                    // param.Name ==
                    // param.Type
                }

                throw new NotImplementedException();
            }

            // u.Name
            if (exp is MethodCallExpression call)
            {
                return ParseMethodCall(call);
            }
            if (exp is BinaryExpression bin)
            {
                return ParseBinaryExpression(bin);
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

        private SqlExpression ParseMethodCall(MethodCallExpression call)
        {
            var body = ToSqlExpression(call.Object);
            // Parse all arguments
            var arguments = call.Arguments.Select(a => ToSqlExpression(a)).ToArray();

            // check for special cases for casting
            if (call.Method.Name == "ToString" && call.Method.ReturnType == typeof(string))
            {
                return new SqlCastExpression(FieldType.String, body);
            }
            if (call.Method.Name == "StartsWith")
            {
                if (arguments[0] is SqlConstantExpression pattern)
                {
                    return new SqlLikeExpression(body, pattern.Value.ToString(), SqlLikePattern.Start);
                }
                throw new NotImplementedException();
            }
            if (call.Method.Name == "EndsWith")
            {
                if (arguments[0] is SqlConstantExpression pattern)
                {
                    return new SqlLikeExpression(body, pattern.Value.ToString(), SqlLikePattern.End);
                }
                throw new NotImplementedException();
            }
            if (call.Method.Name == "Contains")
            {
                if (arguments[0] is SqlConstantExpression pattern)
                {
                    return new SqlLikeExpression(body, pattern.Value.ToString(), SqlLikePattern.Both);
                }
                throw new NotImplementedException();
            }
            
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

        public SqlMathExpression ParseBinaryExpression(BinaryExpression bin)
        {
            var left = ToSqlExpression(bin.Left);
            var right = ToSqlExpression(bin.Right);

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