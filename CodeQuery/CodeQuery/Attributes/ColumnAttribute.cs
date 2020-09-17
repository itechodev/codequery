using System;
using CodeQuery.Definitions;

namespace CodeQuery.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public int Length
        {
            get => ResolvedLength ?? default;
            set => ResolvedLength = value;
        }

        public SqlColumnType ColumnType
        {
            get => ResolvedType ?? default;
            set => ResolvedType = value;
        }

        public string Name { get; set; }
        public SqlColumnType? ResolvedType { get; set; }
        public int? ResolvedLength { get; set;  }
        
    }
}