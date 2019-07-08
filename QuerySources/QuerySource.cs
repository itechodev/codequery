using System;
using System.Collections.Generic;
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

    public class QuerySource<T> : BaseQuerySource
    {
        public QuerySource(SelectQuery select = null): base(select)
        {
            
        }

        public ResultQuerySource<N> Select<N>(Expression<Func<T, N>> fields)
        {
            var parser = new ExpressionParser(new QuerySourceType[] 
            {
                new QuerySourceType(Query.From, typeof(T))
            });

            if (fields.Body is NewExpression newx)
            {
                Query.Fields = newx.Arguments.Select((a,i) => 
                    new SelectField(parser.ToSqlExpression(a), newx.Members[i].Name)
                ).ToArray();
            } 
            else
            {
                Query.Fields = new SelectField[1] { new SelectField(parser.ToSqlExpression(fields), null)};
            }

            return new ResultQuerySource<N>(Query);
        }

        public QuerySource<T> Where(Expression<Func<T, bool>> predicate, WhereType type = WhereType.And)
        {
            // x => x.Field. > 10...
            // .Where(s => s.Active)
            // .Where(s => s.UID.Contains("11"))
            
            var parser = new ExpressionParser(new QuerySourceType[] 
            {
                new QuerySourceType(Query.From, typeof(T))
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
            // Keetp the same source and column definitions
            var bottom = new SqlConstantSource(fields, Query.From.Columns, "b");
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
             // Keetp the same source and column definitions
            var bottom = new SqlConstantSource(fields, Query.From.Columns, "b");
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
            var select = new SelectQuery
            {
                From = new SqlConstantSource(fields, def.Columns, "b") 
            };
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
            // Cater for attributes
            ret.Name = type.Name;
            ret.Columns = type
                .GetProperties()
                .Select(p => new ColumnDefinition(p.Name, ToSqlField(p.PropertyType)))
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