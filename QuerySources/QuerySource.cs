using System;
using System.Linq;
using System.Linq.Expressions;
using codequery.Expressions;
using codequery.Parser;

namespace codequery.QuerySources
{
    public class QuerySource<T>
    {
        private SelectQuery _query { get; set; }

        private ExpressionParser _parser { get; set; }

        public QuerySource()
        {
            _query = new SelectQuery();
            _parser = new ExpressionParser(_query);
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
            var clause = _parser.ToSqlExpression(predicate);
            
            return this;
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
                var instance = Activator.CreateInstance(prop.PropertyType) as QuerySource;
                instance.Columns = instance
                    .GetType()
                    .GetProperties()
                    .Select(p => new QuerySourceField(p.Name, ToSqlField(p.PropertyType)))
                    .ToArray();

                prop.SetValue(this, instance);
            }            
        }

        private FieldType ToSqlField(Type propertyType)
        {
            switch (Type.GetTypeCode(propertyType.GetType()))
            {
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