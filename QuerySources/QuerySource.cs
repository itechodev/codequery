using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using codequery.Expressions;
using codequery.Parser;

namespace codequery.QuerySources
{
    // Each query source needs to a definition of all the columns
    public class BaseQuerySource
    {
        public TableDefinition Definition { get; set; }
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

    public class QQ3<A,B,C>
    {
        public Tuple<A,B,C> Get()
        {
            return null;
        }
    }

    public class QQ2<A,B>
    {
        public QQ3<A,B,C> Query<C>()
        {
            return null;
        }
        public Tuple<A, B> Get()
        {
            return null;
        }
    }

    public class QQ1<T>
    {
        public QQ2<T, G> Query<G>()
        {
            return null;
        }

        public T Get()
        {
            return default(T);
        }
    }

    public class QuerySource<T> : BaseQuerySource
    {
        private SelectQuery _query { get; set; }

        public QuerySource()
        {
            var three = 
                new QQ1<int>()
                .Query<string>()
                .Query<double>()
                .Get();


            _query = new SelectQuery();
        }

        public PostSelectQuerySource<N> Select<N>(Expression<Func<T, N>> fields)
        {
            return new PostSelectQuerySource<N>();
        }

        public QuerySource<T> Where(Expression<Func<T, bool>> predicate)
        {
            // x => x.Field. > 10...
            // .Where(s => s.Active)
            // .Where(s => s.UID.Contains("11"))
            var parser = new ExpressionParser(_query);
            var clause = parser.ToSqlExpression(predicate);
            
            return this;
        }

        public QuerySource<T, A> InnerJoin<A>(Func<A> select)
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

    public class PostSelectQuerySource<T>
    {
        public T FetchSingle()
        {
            return default(T);
        }

        public T[] FetchArray()
        {
            return null;
        }

        // Selects, Where etc.
        // public QuerySource<T> From<T>(PostSelectQuerySource<T> source)
        // {
        //     return new QuerySource<T>();
        // }

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
        public PostSelectQuerySource<T> Select<T>(T fields)
        {
            return new PostSelectQuerySource<T>();
        }

        // SubQuery
        public QuerySource<T> From<T>(PostSelectQuerySource<T> source)
        {
            return new QuerySource<T>();
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
                baseQuery.Definition = GetTableDefinition(prop.PropertyType.GenericTypeArguments[0]);
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