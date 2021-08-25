using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CodeQuery.SqlExpressions;

namespace CodeQuery.SqlGenerators
{
    
    public class GenericSqlGenerator : ISqlGenerator
    {
        public string Select(SqlSelectQuery query)
        {
            var builder = new StringBuilder("SELECT ");
            builder.Append(string.Join(", ", query.Fields.Select(BuildExpression)));
            
            builder.Append(BuildSource(query.From));

            if (query.Joins?.Count > 0)
            {
                builder.Append(string.Join("\n", query.Joins.Select(BuildJoin)));
            }
            
            return builder.ToString();
        }

        private string BuildJoin(SqlJoinSource @join)
        {
            return $"{@join.JoinType.ToString()} JOIN {BuildSource(@join.Source)} ON {BuildExpression(@join.Condition)}";
        }

        private class InspectTypeItem
        {
            public string Name { get; set; }
            public Func<object, object> GetValue { get; set;  }
        }

        public static object GetValue(MemberInfo memberInfo, object @object)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(@object);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(@object);
                default:
                    throw new NotImplementedException();
            }
        } 
        
        private string BuildSource(SqlSource source)
        {
            if (source == null)
                return null;
            
            switch (source)
            {
                case SqlNoSource noSource:
                    var type = noSource.ReflectedType;
                    var props = type.GetProperties().Concat(type.GetMembers());
                    var columns = string.Join("", "", props.Select(p => p.Name));
                    var values = noSource.Rows.Select(r =>
                    {
                        return "(" + string.Join(", ", props.Select(p => GetValue(p, r))) + ")";
                    });
                    
                    return $" FROM (VALUES {string.Join(", ", values)} {noSource.Alias} ({columns}))";
                case SqlQuerySource query:
                    return $" FROM ({Select(query.SelectQuery)}) {query.Alias}";
                case SqlTableSource table:
                    return $" FROM \"{table.TableName}\"";
                default:
                    throw new ArgumentOutOfRangeException(nameof(source));
            }
        }
        
        private string ToCommaList(IEnumerable<SqlExpression> list)
        {
            return list == null ? null : string.Join(", ", list.Select(BuildExpression));
        }

        private string BuildExpression(SqlExpression exp)
        {
            switch (exp)
            {
                case SqlAliasExpression alias:
                    return $"{BuildExpression(alias.Exp)} AS \"{alias.Alias}\""; 
                case SqlConstExpression @const:
                    // switch @const.Type...
                    return @const.Value.ToString();
                case SqlBinaryExpression binary:
                    return $"{BuildExpression(binary.Left)} {binary.Operator} {BuildExpression(binary.Right)}"; 
                case SqlColumnExpression column:
                    return $"\"{column.Definition.Name}\"";
                case SqlFunctionExpression func:
                    return $"{func.Type}({ToCommaList(func.Arguments)})";
            }

            return null;
        }
    }
}