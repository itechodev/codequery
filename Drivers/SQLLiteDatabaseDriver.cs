using System;
using System.Collections.Generic;
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
                    sql.Add(GenerateConstant(constant));
                    return;
                case SourceFieldExpression named:
                    sql.Add(GenerateNamed(named));
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
            sql.Add(GenerateFunctionName(func.Function));
            sql.Add("(");
            GenerateFields(sql, func.Arguments);
            sql.Add(")");
        }

        private string GenerateFunctionName(FieldFunctionType function)
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

        private void GenerateAggregate(SqlGenerator sql, AggregateExpression aggregate)
        {
            sql.Add(GenerateAggregateName(aggregate.Function));
            sql.Add("(");
            GenerateFields(sql, aggregate.Arguments);
            sql.Add(")");
        }

        private string GenerateAggregateName(AggregateFunction function)
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

        private void GenerateFields(SqlGenerator sql, FieldExpression[] arguments)
        {
            GenerateCommaSep(sql, arguments, field => GenerateField(sql, field));
        }

        private string GenerateNamed(SourceFieldExpression named)
        {
            return $"{named.Source.Alias}.{named.Name}";
        }

        private string GenerateConstant(ConstantExpression constant)
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

        private void GenerateCommaSep<T>(SqlGenerator sql, IEnumerable<T> fields, Action<T> callback) where T: class
        {
            foreach (var field in fields)
            {
                callback?.Invoke(field);
                if (field != fields.Last())
                {
                    sql.Add(", ");
                }
            }
        }
        
        private void GenereateSelectFields(SqlGenerator sql, SelectField[] fields)
        {
            GenerateCommaSep(sql, fields, field => {
                GenerateField(sql, field.Expression);
                if (!String.IsNullOrEmpty(field.Alias)) 
                {
                    sql.Add(" AS {field.Alias}");
                }
            });
        }

        public string GenerateSelect(SelectQuery query)
        {
            SqlGenerator sql = new SqlGenerator();
            sql.Add("SELECT");
            sql.Indent();
            sql.NewLine();

            GenereateSelectFields(sql, query.Fields);
            sql.UnIndent();
            sql.NewLine();
            sql.Add("FROM", true);
            
            GenerateSource(sql, query.From);
            if (query.Where != null)
            {
                sql.Add("WHERE", true);
                sql.Indent();
                GenerateField(sql, query.Where);
                sql.UnIndent();
            }
            if (query.GroupBy?.Length > 0)
            {
                sql.Add("GROUP BY");
                GenerateCommaSep(sql, query.GroupBy, f => GenerateField(sql, f));
            }
            if (query.OrderBy?.Length > 0)
            {
                sql.Add("ORDER BY");
                GenerateCommaSep(sql, query.OrderBy, f => {
                    GenerateField(sql, f.By);
                    sql.Add(f.Ascending ? "ASC" : "DESC");
                });
            }
            return sql.Generate();
        }

        private void GenerateSource(SqlGenerator sql, QuerySource from)
        {
            if (from is TableSource table) 
            {
                sql.Add($"{table.Table} {table.Alias}");
                return;
            }
            // Select 10 as value
            if (from is ConstantSource constant)
            {
                sql.Add("(", true);
                sql.Indent();
                sql.Add("SELECT ");
                GenereateSelectFields(sql, constant.Fields);
                sql.UnIndent();
                sql.Add(") {constant.Alias}");
                return;
            }
            if (from is SubQuerySource sub) 
            {
                sql.Add("(");
                GenerateSource(sql, sub.Source);
                sql.Add(") {constant.Alias}");
                return;
            }
            throw new NotImplementedException($"Could not generate source from {from.GetType()}");
        }
    }
}