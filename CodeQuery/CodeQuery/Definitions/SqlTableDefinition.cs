using System;
using System.Collections.Generic;
using System.Reflection;

namespace CodeQuery.Definitions
{
    
    public class SqlTableDefinition
    {
        // Table name as in the database
        public string Name { get; }
        public List<SqlColumnDefinition> Columns { get;  }

        public SqlTableDefinition(string name)
        {
            Name = name;
            Columns = new List<SqlColumnDefinition>();
        }

        public SqlTableDefinition(Type t)
        {
            var customName = t.GetCustomAttribute<Table>();
            var tableName = customName?.Name ?? t.Name;
            
            Columns = new List<SqlColumnDefinition>();
            foreach (var prop in t.GetProperties(BindingFlags.Public))
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    Columns.Add(ToSqlColumn(prop));
                }
            }
        }

        private static SqlColumnDefinition ToSqlColumn(PropertyInfo info)
        {
            // override any if type is overwrote
            var colAttr = info.GetCustomAttribute<Column>();
            var type = colAttr?.ResolvedType ?? InferType(info.PropertyType);
            
            return null;
        }

        private static SqlColumnType InferType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return SqlColumnType.Bool;
                case TypeCode.Byte:
                    return SqlColumnType.Int8;
                case TypeCode.Char:
                    return SqlColumnType.Char;
                case TypeCode.DateTime:
                    return SqlColumnType.TimeStamp;
                case TypeCode.Decimal:
                    return SqlColumnType.Numeric;
                case TypeCode.Double:
                    return SqlColumnType.Double;
                case TypeCode.Int16:
                    return SqlColumnType.Int16;
                case TypeCode.Int32:
                    return SqlColumnType.Int32;
                case TypeCode.Int64:
                    return SqlColumnType.Int64;
                case TypeCode.SByte:
                    return SqlColumnType.UInt8;
                case TypeCode.Single:
                    return SqlColumnType.Single;
                case TypeCode.String:
                    return SqlColumnType.Varchar;
                case TypeCode.UInt16:
                    return SqlColumnType.UInt16;
                case TypeCode.UInt32:
                    return SqlColumnType.UInt32;
                case TypeCode.UInt64:
                    return SqlColumnType.UInt64;
                default:
                    throw new ArgumentOutOfRangeException($"Cannot infer SQL type from '{type.Name}'.\nIf you want to store this as JSON or XML you must define it explicitly using the Column attribute.");
            }
        }
    }

   
}