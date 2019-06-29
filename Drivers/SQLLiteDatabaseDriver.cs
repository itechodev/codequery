using System;
using System.Linq;
using codequery.Expressions;

namespace codequery.Drivers
{
    public class SQLLiteDatabaseDriver : IDatabaseDriver
    {
        private void GenerateField(SqlGenerator sql, FieldExpression field)
        {
            switch (field)
            {
                case ConstantExpression constant:
                    GenerateConstant(sql, constant);
                    return;
                case SourceFieldExpression named:
                    GenerateNamed(sql, named);
                    return;
                case AggregateExpression aggregate:
                    GenerateAggregate(sql, aggregate);
                    return;
                case FunctionExpression func:
                    GenerateFunc(sql, func);
                    return;
                case MathExpression math:
                    GenerateMath(sql, math);
                    return;
                case RowFunctionExpression rowFunc:
                    GenerateRowFunc(sql, rowFunc);
                    return;
            }
            throw new Exception($"Could not generate SQL for field {field.GetType()}");
        }

        private string GenerateRowFunc(SqlGenerator sql, RowFunctionExpression rowFunc)
        {
            switch (rowFunc.Function)
            {
                case FieldRowFunctionType.Distinct:
                    return "distinct";
            }
            throw new Exception($"Could not map row function '{rowFunc.Function.ToString()}'");        
        }

        private void GenerateMath(SqlGenerator sql, MathExpression math)
        {
            GenerateField(sql, math.Left);
            sql.Add(OperatorStr(math.Op));
            GenerateField(sql, math.Right);
        }

        private string OperatorStr(FieldMathOperator op)
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

        private void GenerateFunc(SqlGenerator sql, FunctionExpression func)
        {
            GenerateFields(sql, func.Arguments);
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

        private string GenerateAggregate(SqlGenerator sql, AggregateExpression aggregate)
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

        private object GenerateFields(SqlGenerator sql, FieldExpression[] arguments)
        {
            return String.Join(" ,", arguments.Select(f => GenerateField(f, tabIndex)));
        }

        private string GenerateNamed(SqlGenerator sql, SourceFieldExpression named)
        {
            return $"{named.Source.Alias}.{named.Name}";
        }

        private string GenerateConstant(SqlGenerator sql, ConstantExpression constant)
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

        private string GenereateSelectFields(SqlGenerator sql, SelectField[] fields)
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
            SqlGenerator sql = new SqlGenerator();
            sql.AddLine("SELECT");
            sql.Indent();
            GenereateSelectFields(sql, query.Fields);
            sql.UnIndent();
            sql.AddLine("FROM");

            GenerateSource(sql, query.From);
            if (query.Where != null)
            {
                sql.AddLine("WHERE");
                sql.Indent();
                GenerateField(sql, query.Where);
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

        private void GenerateSource(SqlGenerator sql, QuerySource from)
        {
            if (from is TableSource table) 
            {
                sql.AddLine($"{table.Table} {table.Alias}");
                return;
            }
            if (from is ConstantSource constant)
            {
                // sql.AddLine($"SELECT {GenereateSelectFields(constant.Fields)} {constant.Alias}");
                return;
            }
            if (from is SubQuerySource sub) 
            {
                // return $"{GenerateSource(sub.Source)}) {sub.Alias}";
                GenerateSource(sql, sub.Source);
                // sql.add
                return;
            }
            throw new NotImplementedException($"Could not generate source from {from.GetType()}");
        }
    }
}