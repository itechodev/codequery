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

    public class QuerySource<T> : BaseQuerySource
    {
        private SelectQuery _query { get; set; }

        public QuerySource()
        {
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