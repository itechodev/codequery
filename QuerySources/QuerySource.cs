using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using codequery.Expressions;
using codequery.Parser;

namespace codequery.QuerySources
{
    public enum WhereType
    {
        Or,
        And
    }
    // Each query source needs to a definition of all the columns
    public class BaseQuerySource
    {
        public BaseQuerySource(SelectQuery select)
        {
            Query = select ?? new SelectQuery();
        }

        public SelectQuery Query { get; set; }
    }

    public class Join2<S1, S2>
    {
        public S1 First { get; set; }
        public S2 Second { get; set; }
    }

    public class Join3<S1, S2, S3> : Join2<S1, S2>
    {
        public S3 Thrird { get; set; }
    }

    public class Join4<S1, S2, S3, S4>: Join3<S1, S2, S3>
    {
        public S4 Fourth { get; set; }
    }


    public class Join<L,R>
    {
        public L Left { get; set; }
        public R Right { get; set; }
    }

    public class AggregateQuerySource<T, N> : BaseQuerySource
    {
        
        public AggregateQuerySource(SelectQuery select) : base(select)
        {
        }

        public void Having()
        {
            

        }

        // Can only select the group by fields / and or aggregates for the rest
        public ResultQuerySource<F> Select<F>(Expression<Func<Aggregate<N, T>, F>> fields)
        {
            var parser = new ExpressionParser(new QuerySourceType[] 
            {
                new QuerySourceType(Query.From, typeof(Aggregate<N, T>), SourceType.Aggregate),
            });

            if (fields.Body is NewExpression newx)
            {
                // x => new { ... }
                Query.Fields = newx.Arguments.Select((a,i) => 
                    new SelectField(parser.ToSqlExpression(a), newx.Members[i].Name)
                ).ToArray();
            }
            else if (fields.Body is ParameterExpression p)
            {
                // Same as selectAll
                // x => x
                if (p.Type != typeof(T))
                {
                    throw new Exception($"Type {p.Type.ToString()} is not part of query");
                }
                Query.Fields = Query.From.Columns
                    .Select(c => new SelectField(new SqlColumnExpression(c.Type, c.Name, Query.From), null))
                    .ToArray();
            } else 
            {
                // x => x.name 
                // x => x.func() 
                Query.Fields = new SelectField[1] { new SelectField(parser.ToSqlExpression(fields), null)};
            }
            return new ResultQuerySource<F>(Query);
         }
    }
    public interface Aggregate<T, N>
    {
        T Value { get; set; }
        int Count();
        int CountDistinct();
        int? Average(Expression<Func<N, int>> field);
        double? Average(Expression<Func<N, double>> field);
        int? AverageDistinct(Expression<Func<N, int>> field);
        double? AverageDistinct(Expression<Func<N, double>> field);
        int? Sum(Expression<Func<N, int>> field);
        double? Sum(Expression<Func<N, double>> field);
        P Max<P>(Expression<Func<N, P>> clause);
        P Min<P>(Expression<Func<N, P>> clause);
    }

    public class QuerySource<T> : BaseQuerySource
    {
        public QuerySource(SelectQuery select = null): base(select)
        {
            
        }

        public ResultQuerySource<T> SelectAll()
        {
             Query.Fields = Query.From.Columns
                    .Select(c => new SelectField(new SqlColumnExpression(c.Type, c.Name, Query.From), null))
                    .ToArray();
            return new ResultQuerySource<T>(Query);
        }

        public ResultQuerySource<N> Select<N>(Expression<Func<T, N>> fields)
        {
            var parser = new ExpressionParser(new QuerySourceType[] 
            {
                new QuerySourceType(Query.From, typeof(T), SourceType.Instance)
            });

            if (fields.Body is NewExpression newx)
            {
                // x => new { ... }
                Query.Fields = newx.Arguments.Select((a,i) => 
                    new SelectField(parser.ToSqlExpression(a), newx.Members[i].Name)
                ).ToArray();
            }
            else if (fields.Body is ParameterExpression p)
            {
                // Same as selectAll
                // x => x
                if (p.Type != typeof(T))
                {
                    throw new Exception($"Type {p.Type.ToString()} is not part of query");
                }
                Query.Fields = Query.From.Columns
                    .Select(c => new SelectField(new SqlColumnExpression(c.Type, c.Name, Query.From), null))
                    .ToArray();
            } else {
                // x => x.name 
                // x => x.func() 
                Query.Fields = new SelectField[1] { new SelectField(parser.ToSqlExpression(fields), null)};
            }
            return new ResultQuerySource<N>(Query);
        }

        public AggregateQuerySource<T, N> GroupBy<N>(Expression<Func<T, N>> groupBy)
        {
            var parser = new ExpressionParser(new QuerySourceType[] 
            {
                new QuerySourceType(Query.From, typeof(T), SourceType.Instance)
            });
            var groupExp = parser.ToSqlExpression(groupBy);
            Query.GroupBy = new  SqlExpression[1] { groupExp };
            return new AggregateQuerySource<T, N>(Query);
        }

        public QuerySource<T> Where(Expression<Func<T, bool>> predicate, WhereType type = WhereType.And)
        {
            // x => x.Field. > 10...
            // .Where(s => s.Active)
            // .Where(s => s.UID.Contains("11"))
            
            var parser = new ExpressionParser(new QuerySourceType[] 
            {
                new QuerySourceType(Query.From, typeof(T), SourceType.Instance)
            });
            var clause = parser.ToSqlExpression(predicate);
            if (Query.Where == null)
            {
                Query.Where = clause;
            }
            else
            {
                // Combine previous with AND or OR
                Query.Where = new SqlMathExpression(
                    clause.FieldType, 
                    Query.Where, 
                    type == WhereType.And ? FieldMathOperator.And : FieldMathOperator.Or, 
                    clause);
            }
            return new QuerySource<T>(Query);
        }

        public QuerySource<T, A> InnerJoin<A>(Func<A> select)
        {
            return null;
        }


        public QuerySource<T, A> LeftJoin<A>(Func<A> select) where A: class
        {
            return null;
        }

        public QuerySource<T, Nullable<A>> LeftJoin<A>(Func<Nullable<A>> select) where A: struct
        {
            return null;
        }

        public QuerySource<T, A> RightJoin<A>(Func<A> select) where A: class
        {
            return null;
        }
        
        public QuerySource<T, Nullable<A>> RightJoin<A>(Func<Nullable<A>> select) where A: struct
        {
            return null;
        }
    }

    public class QuerySource<A, B>
    {
        public QuerySource<A, B, C> InnerJoin<C>(Func<C> select)
        {
            return null;
        }
    }

    public class QuerySource<A, B, C>
    {
        public QuerySource<A, B, C, D> InnerJoin<D>(Func<D> select)
        {
            return null;
        }
    }

     public class QuerySource<A, B, C, D>
    {
        public G Select<G>(Func<A, B, C, D, G> exp)
        {
            return default(G);
        }
    }

    public class ResultQuerySource<T>: BaseQuerySource
    {
        public ResultQuerySource(SelectQuery select) : base(select)
        {
        }

        public ResultQuerySource<T> Union(T fields)
        {
            var def = Database.GetTableDefinition(typeof(T));
            var values = def.FieldsAndProps.Select(f => f.GetValue(fields)).ToArray();
            
            // Keetp the same source and column definitions
            var bottom = new SqlConstantSource(values, Query.From.Columns, "b");
            // Append to union if there is any
            if (Query.From is SqlUnionSource union && !union.UnionAll)
            {
                union.Sources.Add(bottom);
                return new ResultQuerySource<T>(Query);
            }
            // else create new UnionSource
            var select = new SelectQuery
            {
                From = new SqlUnionSource(Query.From, bottom, false, Query.From.Columns, "c")
            };
            return new ResultQuerySource<T>(select);
        }

        public ResultQuerySource<T> UnionAll(T fields)
        {
            var def = Database.GetTableDefinition(typeof(T));
            var values = def.FieldsAndProps.Select(f => f.GetValue(fields)).ToArray();
            
             // Keetp the same source and column definitions
            var bottom = new SqlConstantSource(values, Query.From.Columns, "b");
            // Append to union if there is any
            if (Query.From is SqlUnionSource union && union.UnionAll)
            {
                union.Sources.Add(bottom);
                return new ResultQuerySource<T>(Query);
            }
            // else create new UnionSource
            var select = new SelectQuery
            {
                From = new SqlUnionSource(Query.From, bottom, true, Query.From.Columns, "c")
            };
            return new ResultQuerySource<T>(select);
        }


        public T FetchSingle()
        {
            return default(T);
        }

        public T[] FetchArray()
        {
            return null;
        }
    }

    public class TableDefinition
    {
        public string Name { get; set; }
        public ColumnDefinition[] Columns { get; set; }
        public FieldProp[] FieldsAndProps { get; internal set; }
    }
    
    public class ColumnDefinition
    {
        public ColumnDefinition(string name, FieldType type)
        {
            this.Name = name;
            this.Type = type;

        }
        public ColumnDefinition(string name, FieldType type, string alias) 
        {
            this.Name = name;
            this.Type = type;
        }
        // More to come like precision length etc.
        public string Name { get; set; }
        public FieldType Type { get; set; }
    }

    public class Database
    {
        // Constant source queries
        public ResultQuerySource<T> Select<T>(T fields)
        {
            var def = GetTableDefinition(typeof(T));
            var values = def.FieldsAndProps.Select(f => f.GetValue(fields)).ToArray();
                 
            var select = new SelectQuery
            {
                From = new SqlConstantSource(values, def.Columns, "b") 
            };
            return new ResultQuerySource<T>(select);
        }

        public ResultQuerySource<T> Union<T>(params T[] values)
        {
            return Union<T>(values, false);
        }

        public ResultQuerySource<T> UnionAll<T>(params T[] values)
        {
            return Union<T>(values, true);
        }

        public ResultQuerySource<T> Union<T>(IEnumerable<T> values, bool unionAll = false)
        {
            var def = GetTableDefinition(typeof(T));
            var rows = values
                .Select(v => def.FieldsAndProps.Select(f => f.GetValue(v)))
                .ToArray();

            // Only single constexpression
            if (rows.Count() == 1)
            {
                return new ResultQuerySource<T>(new SelectQuery { From = new SqlConstantSource(rows[0].ToArray(), def.Columns, "us") });
            }
                
            SqlUnionSource union = new SqlUnionSource(unionAll, def.Columns, "ul");
            foreach (var row in values)
            {
                var constValues = def.FieldsAndProps.Select(f => f.GetValue(row));
                union.Sources.Add(new SqlConstantSource(constValues.ToArray(), def.Columns, null));
            }
            var select = new SelectQuery { From = union };
            return new ResultQuerySource<T>(select);
        }

        // SubQuery
        public QuerySource<T> From<T>(ResultQuerySource<T> source)
        {
            return new QuerySource<T>(source.Query);
        }

        // Tables to be declared in derived table
        public Database()
        {
            // Instantiate all members
            // and get column fields
            var tables = this.GetType()
                .GetProperties()
                .Where(p => p.PropertyType.GetGenericTypeDefinition() == typeof(DatabaseTable<>));

            foreach (var prop in tables)
            {
                var instance = Activator.CreateInstance(prop.PropertyType);
                prop.SetValue(this, instance);

                var baseQuery =  instance as BaseQuerySource;
                var def = GetTableDefinition(prop.PropertyType.GenericTypeArguments[0]);
                baseQuery.Query = new SelectQuery
                {
                    From = new SqlTableSource(def, "a")
                };
            }
        }
        
        public static TableDefinition GetTableDefinition(Type type)
        {
            var ret = new TableDefinition();
             // Need to cater for attributes columnName, StringLength etc.
            ret.Name = type.Name;
            ret.FieldsAndProps = ReflectionHelper.GetFiedAndProps(type);
            
            ret.Columns = ret.FieldsAndProps
                .Select(f => new ColumnDefinition(f.Name, ToSqlField(f.Type)))
                .ToArray();
                
            return ret;
        }

        public static  TableDefinition GetTableDefinition<T>()
        {
            return GetTableDefinition(typeof(T));
        }

        private static FieldType ToSqlField(Type propertyType)
        {
            var nullableType = Nullable.GetUnderlyingType(propertyType);
            if (nullableType != null)
            {
                // Nullable type
                propertyType = nullableType;
            }
            switch (Type.GetTypeCode(propertyType))
            {
                case TypeCode.Boolean:
                    return FieldType.Bool;
                case TypeCode.Byte:
                case TypeCode.SByte:
                    return FieldType.Char;
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return FieldType.Int;
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return FieldType.Double;
                case TypeCode.String:
                    return FieldType.String;
                default:
                    throw new Exception($"Could not map type C# type '{propertyType.ToString()}' to SQL type ");
            }
        }
    }


    public class DatabaseTable<T> : QuerySource<T>
    {
        
    }
}