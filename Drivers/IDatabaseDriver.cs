using System;
using System.Collections.Generic;
using System.Linq;
using codequery.Expressions;

namespace codequery.Drivers
{
    public interface IDatabaseDriver
    {
        string GenerateSelect(SelectQuery query);
    }

    public class SQLLiteDatabaseDriver : IDatabaseDriver
    {
        private string GenerateField(FieldExpression field, int tabIndex)
        {
            switch (field)
            {
                case ConstantExpression constant:
                    return GenerateConstant(constant, tabIndex);
                case SourceFieldExpression named:
                    return GenerateNamed(named, tabIndex);
                case AggregateExpression aggregate:
                    return GenerateAggregate(aggregate, tabIndex);
                case FunctionExpression func:
                    return GenerateFunc(func, tabIndex);
                case MathExpression math:
                    return GenerateMath(math, tabIndex);
                case RowFunctionExpression rowFunc:
                    return GenerateRowFunc(rowFunc, tabIndex);
            }
            throw new Exception($"Could not generate SQL for field {field.GetType()}");
        }

        private string GenerateRowFunc(RowFunctionExpression rowFunc, int tabIndex)
        {
            switch (rowFunc.Function)
            {
                case FieldRowFunctionType.Distinct:
                    return "distinct";
            }
            throw new Exception($"Could not map row function '{rowFunc.Function.ToString()}'");        
        }

        private string GenerateMath(MathExpression math, int tabIndex)
        {
            return $"{GenerateField(math.Left, tabIndex)} {OperatorStr(math.Op)} {GenerateField(math.Right, tabIndex)}";
        }

        private object OperatorStr(FieldMathOperator op)
        {
            switch (op)
            {
                case FieldMathOperator.Plus:
                    return "+";
                case FieldMathOperator.Minus:
                    return "-";
                case FieldMathOperator.Multiply:
                    return "*";
                case FieldMathOperator.Divide:
                    return "/";
                case FieldMathOperator.Equal:
                    return "=";
                case FieldMathOperator.GreaterThan:
                    return ">";
                case FieldMathOperator.LessThan:
                    return "<";
                case FieldMathOperator.GreaterEqualThan:
                    return ">=";
                case FieldMathOperator.LessEqualThan:
                    return "<=";
                case FieldMathOperator.Or:
                    return "OR";
                case FieldMathOperator.And:
                    return "AND";
            }
            throw new NotImplementedException();
        }

        private string GenerateFunc(FunctionExpression func, int tabIndex)
        {
            var fields = GenerateFields(func.Arguments, tabIndex);
            return $"{GenerateFunctionName(func.Function)}({fields})";
        }

        private object GenerateFunctionName(FieldFunctionType function)
        {
            switch (function)
            {
                case FieldFunctionType.Lower:
                    return "lower";
                case FieldFunctionType.Upper:
                    return "upper";
                case FieldFunctionType.Substring: 
                    return "substr";
            }
            throw new NotImplementedException($"Could not generate function {function.ToString()}");
        }

        private string GenerateAggregate(AggregateExpression aggregate, int tabIndex)
        {
            var fields = GenerateFields(aggregate.Arguments, tabIndex);
            return $"{GenerateAggregateName(aggregate.Function)}({fields})";
        }

        private object GenerateAggregateName(AggregateFunction function)
        {
            switch (function)
            {
                case AggregateFunction.Average:
                    return "AVERAGE";
                case AggregateFunction.Count:
                    return "COUNT";
            }
            throw new Exception($"Could not map aggregate function '{function.ToString()}'");
        }

        private object GenerateFields(FieldExpression[] arguments, int tabIndex)
        {
            return String.Join(" ,", arguments.Select(f => GenerateField(f, tabIndex)));
        }

        private string GenerateNamed(SourceFieldExpression named, int tabIndex)
        {
            return $"{named.Source.Alias}.{named.Name}";
        }

        private string GenerateConstant(ConstantExpression constant, int tabIndex)
        {
            switch (constant.FieldType)
            {
                case FieldType.Int:
                case FieldType.Double:
                    return constant.Value.ToString();
                case FieldType.String:
                case FieldType.Char:
                    return $"'{constant.Value.ToString()}'";
            }
            throw new NotImplementedException($"Cannot generate constant of type {constant.FieldType}");
        }

        private string GenereateSelectFields(SelectField[] fields)
        {
            return String.Join(", ", fields.Select(f => {
                var str = GenerateField(f.Expression, 0);
                if (!String.IsNullOrEmpty(f.Alias)) 
                {
                    return str + $" AS {f.Alias}";
                }
                return str;
             }));
        }

        public string GenerateSelect(SelectQuery query)
        {
            string sql = "SELECT ";
            sql += GenereateSelectFields(query.Fields);
            sql += " FROM ";
            sql += GenerateSource(query.From);
            if (query.Where != null)
            {
                sql += " WHERE " + GenerateField(query.Where, 0);
            }
            if (query.GroupBy?.Length > 0)
            {
                sql += " GROUP BY " + String.Join(", ", query.GroupBy.Select(s => GenerateField(s, 0)));
            }
            if (query.OrderBy?.Length > 0)
            {
                sql += " ORDER BY " + String.Join(", ", query.OrderBy.Select(s => GenerateField(s.By, 0) + (s.Ascending ? "ASC" : "DESC")));
            }
            return sql;
        }

        private string GenerateSource(QuerySource from)
        {
            if (from is TableSource table) 
            {
                return $"{table.Table} {table.Alias}";
            }
            if (from is ConstantSource constant)
            {
                return $"SELECT {GenereateSelectFields(constant.Fields)} {constant.Alias}";
            }
            if (from is SubQuerySource sub) 
            {
                return $"{GenerateSource(sub.Source)}) {sub.Alias}";
            }
            throw new NotImplementedException($"Could not generate source from {from.GetType()}");
        }
    }
}