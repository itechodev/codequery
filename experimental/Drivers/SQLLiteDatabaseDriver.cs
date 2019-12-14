using System;
using System.Collections.Generic;
using System.Linq;
using codequery.Expressions;

namespace codequery.Drivers
{
    public class SQLLiteDatabaseDriver : IDatabaseDriver
    {
        private void GenerateField(SqlGenerator sql, SqlExpression field)
        {
            switch (field)
            {
                case SqlConstantExpression constant:
                    sql.Add(GenerateConstant(constant));
                    return;
                case SqlColumnExpression named:
                    sql.Add(GenerateNamed(named));
                    return;
                case SqlAggregateExpression aggregate:
                    GenerateAggregate(sql, aggregate);
                    return;
                case SqlFunctionExpression func:
                    GenerateFunc(sql, func);
                    return;
                case SqlMathExpression math:
                    GenerateMath(sql, math);
                    return;
                case SqlRowFunctionExpression rowFunc:
                    GenerateRowFunc(sql, rowFunc);
                    return;
                case SqlCastExpression cast:
                    GenerateCast(sql, cast);
                    return;
                case SqlLikeExpression like:
                    GenerateLike(sql, like);
                    return;
            }
            throw new Exception($"Could not generate SQL for field {field.GetType()}");
        }

        private void GenerateLike(SqlGenerator sql, SqlLikeExpression like)
        {
            GenerateField(sql, like.Body);
            sql.Add(" LIKE '");
            if (like.PatternType != SqlLikePattern.End)
            {
                sql.Add("%");
            }
            sql.Add(like.Pattern);
            if (like.PatternType != SqlLikePattern.Start)
            {
                sql.Add("%");
            }
            sql.Add("'");
        }

        private void GenerateCast(SqlGenerator sql, SqlCastExpression cast)
        {
            sql.Add("CAST(");
            GenerateField(sql, cast.Field);
            sql.Add($" AS {FieldToStr(cast.FieldType)})");
        }

        private object FieldToStr(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.Int:
                    return "INT";
                case FieldType.String:
                    return "STRING";
            }
            throw new NotImplementedException();
        }

        private string GenerateRowFunc(SqlGenerator sql, SqlRowFunctionExpression rowFunc)
        {
            switch (rowFunc.Function)
            {
                case FieldRowFunctionType.Distinct:
                    return "distinct";
            }
            throw new Exception($"Could not map row function '{rowFunc.Function.ToString()}'");        
        }

        private void GenerateMath(SqlGenerator sql, SqlMathExpression math)
        {
            GenerateField(sql, math.Left);
            sql.Add(" " + OperatorStr(math.Op) + " ");
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

        private void GenerateFunc(SqlGenerator sql, SqlFunctionExpression func)
        {
            // lower(....) 
            sql.Add(GenerateFunctionName(func.Function));
            sql.Add("(");
            GenerateField(sql, func.Body);
            if (func.Arguments?.Length > 0)
            {
                sql.Add(", ");
                GenerateFields(sql, func.Arguments);
            }
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

        private void GenerateAggregate(SqlGenerator sql, SqlAggregateExpression aggregate)
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

        private void GenerateFields(SqlGenerator sql, SqlExpression[] arguments)
        {
            GenerateCommaSep(sql, arguments, false, (field, _) => GenerateField(sql, field));
        }

        private string GenerateNamed(SqlColumnExpression named)
        {
            return $"{named.Source.Alias}.{named.Name}";
        }

        private string GenerateConstant(SqlConstantExpression constant)
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

        private void GenerateCommaSep<T>(SqlGenerator sql, IEnumerable<T> fields, bool newline, Action<T, int> callback) where T: class
        {
            int i = 0;
            foreach (var field in fields)
            {
                callback?.Invoke(field, i++);
                if (field != fields.Last())
                {
                    sql.Add(", ");
                    if (newline)
                    {
                        sql.NewLine();
                    }
                }
            }
        }
        
        private void GenereateSelectFields(SqlGenerator sql, SqlSelectQuery query)
        {
            if (query.Fields == null)
            {
                // Select all fields from
                GenerateCommaSep(sql, query.From.Columns, true, (f,_) => {
                    GenerateField(sql, new SqlColumnExpression(f.Type, f.Name, query.From));
                });
                return;
            }
            
            GenerateCommaSep(sql, query.Fields, true, (field, _) => {
                GenerateField(sql, field.Expression);
                if (!String.IsNullOrEmpty(field.Alias)) 
                {
                    sql.Add($" AS {field.Alias}");
                }
            });
        }

        public string GenerateSelect(SqlSelectQuery query)
        {
            SqlGenerator sql = new SqlGenerator();
            // If there is only 1 const selection
            if (query.From is SqlConstantSource constSource)
            {
                GenerateSource(sql, constSource);
                return sql.Generate();
            }
            sql.Add("SELECT");
            sql.NewLineIndent();
            GenereateSelectFields(sql, query);
            sql.NewLineUnIndent();
            sql.Add("FROM");
            sql.NewLineIndent();
            GenerateSource(sql, query.From);
            sql.NewLineUnIndent();

            if (query.Where != null)
            {
                sql.Add("WHERE");
                sql.NewLineIndent();
                GenerateField(sql, query.Where);
                sql.NewLineUnIndent();
            }
            if (query.GroupBy?.Length > 0)
            {
                sql.Add("GROUP BY");
                GenerateCommaSep(sql, query.GroupBy, false, (f,_) => GenerateField(sql, f));
            }
            if (query.OrderBy?.Length > 0)
            {
                sql.Add("ORDER BY");
                GenerateCommaSep(sql, query.OrderBy, false, (f,_) => {
                    GenerateField(sql, f.By);
                    sql.Add(f.Ascending ? "ASC" : "DESC");
                });
            }
            return sql.Generate();
        }

        private void GenerateSource(SqlGenerator sql, SqlQuerySource from)
        {
            if (from is SqlTableSource table) 
            {
                sql.Add($"{table.Name} {table.Alias}");
                return;
            }
            // Select 10 as value
            if (from is SqlConstantSource constant)
            {
                sql.NewLine();
                sql.Add("SELECT ");
                
                GenerateCommaSep(sql, constant.ValueExpressions, false, (exp, i) => {
                    sql.Add($"{GenerateConstant(exp)} AS {constant.Columns[i].Name}");
                });

                return;
            }
            if (from is SqlSubQuerySource sub) 
            {
                sql.Add("(");
                GenerateSource(sql, sub.Source);
                sql.Add($") {sub.Alias}");
                return;
            }
            if (from is SqlUnionSource union)
            {
                sql.Add("(");
                sql.Indent();
                foreach (var u in union.Sources)
                {
                    GenerateSource(sql, u);
                    if (u != union.Sources.Last())
                    {
                        sql.NewLine();
                        sql.Add(union.UnionAll ? "UNION ALL" : "UNION");
                    }
                }
                sql.UnIndent();
                sql.NewLine();
                sql.Add($") {union.Alias}");
                return;
            }
            throw new NotImplementedException($"Could not generate source from {from.GetType()}");
        }
    }
}