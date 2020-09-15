using System;

namespace codequery.Definitions
{
    public enum ColumnType
    {
        Boolean,
        Char,
        DateTime,
        Time,
        String,

        UInt8,
        Int8,

        UInt16,
        Int16,

        UInt32,
        Int32,

        UInt64,
        Int64,

        Float,
        Double,
        Decimal,

        Binary
    }

    public class ColumnDefinition
    {
        public ColumnType ColumnType { get; set; }
        public bool Nullable { get; set; }
        public int Size { get; set; }
        public int Precision { get; set; }
        public string Name { get; set; }

        public static ColumnType FromType(Type type)
        {
            if (!type.IsPrimitive)
            {
                throw new Exception($"Cannot convert {type.Name} to column type. Only primitive types allowed.");
            }
            // if (type.IsEnum)
            // {
            // }
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return ColumnType.Boolean;
                case TypeCode.Char:
                    return ColumnType.Char;
                case TypeCode.SByte:
                    return ColumnType.Int8;
                case TypeCode.Byte:
                    return ColumnType.UInt8;
                case TypeCode.Int16:
                    return ColumnType.Int16;
                case TypeCode.UInt16:
                    return ColumnType.UInt16;
                case TypeCode.Int32:
                    return ColumnType.Int32;
                case TypeCode.UInt32:
                    return ColumnType.UInt32;
                case TypeCode.Int64:
                    return ColumnType.Int64;
                case TypeCode.UInt64:
                    return ColumnType.UInt64;
                case TypeCode.Single:
                    return ColumnType.Float;
                case TypeCode.Double:
                    return ColumnType.Double;
                case TypeCode.Decimal:
                    return ColumnType.Decimal;
                case TypeCode.DateTime:
                    return ColumnType.DateTime;
                case TypeCode.String:
                    return ColumnType.String;
            }
            throw new Exception($"Cannot convert {type.Name} to column type. No supported.");
        }

        public static ColumnDefinition GetColumnDef(Type type, string name)
        {
            return new ColumnDefinition
            {
                ColumnType = FromType(type),
                Name = name,
                Nullable = System.Nullable.GetUnderlyingType(type) != null,
                Precision = 0,
                Size = 0
            };
        }
    }

    public class TableDefinition
    {
        public string Name { get; set; }
        public ColumnDefinition[] Columns { get; set; }
    }

}
