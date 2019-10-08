using System;
using System.Collections.Generic;
using System.Linq;
using codequery.Expressions;

namespace codequery.QuerySources
{
    public class Database
    {
        // Constant source queries
        public ResultQuerySource<T> Select<T>(T fields)
        {
            return null;
        }

        // Tables to be declared in derived table
        public Database()
        {
            // Instantiate all members
            // and get column fields
            // var tables = this.GetType()
            //     .GetProperties()
            //     .Where(p => p.PropertyType.GetGenericTypeDefinition() == typeof(DatabaseTable<>));

            // foreach (var prop in tables)
            // {
            //     var instance = Activator.CreateInstance(prop.PropertyType);
            //     prop.SetValue(this, instance);

            //     var baseQuery =  instance as BaseQuerySource;
            //     var def = GetTableDefinition(prop.PropertyType.GenericTypeArguments[0]);
            //     baseQuery.Query = new SelectQuery
            //     {
            //         From = new SqlTableSource(def, "a")
            //     };
            // }
        }
        
        public static TableDefinition GetTableDefinition(Type type)
        {
            var ret = new TableDefinition();
             // Need to cater for attributes columnName, StringLength etc.
            ret.Name = type.Name;
            // ret.FieldsAndProps = ReflectionHelper.GetFiedAndProps(type);
            
            ret.Columns = type.GetProperties()
                .Select(f => new ColumnDefinition(f.Name, ToSqlField(f.PropertyType)))
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
}