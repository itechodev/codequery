using System;
using System.Collections.Generic;
using CodeQuery.Definitions;

namespace CodeQuery
{
    public abstract class SqlSource
    {
        public SqlSource Parent { get; }
        public Type ReflectedType { get;  }
        public List<SqlColumnDefinition> Columns { get; protected set; }

        protected SqlSource(SqlSource parent, Type reflectedType)
        {
            Parent = parent;
            ReflectedType = reflectedType;
            Columns = new List<SqlColumnDefinition>();
        }

        protected SqlSource(SqlSource parent, Type reflectedType, List<SqlColumnDefinition> columns)
        {
            Parent = parent;
            ReflectedType = reflectedType;
            Columns = columns ?? new List<SqlColumnDefinition>();
        }
    }
    
    // Select const fields without a source
    // Select 1, 2
    public class SqlNoSource : SqlSource
    {
        public SqlNoSource(SqlSource parent, Type reflectedType, List<SqlColumnDefinition> columns) : base(parent, reflectedType, columns)
        {
        }
    }

    // ie. select * from generate(1, 100)
    public class SqlFuncSource : SqlSource
    {
        public SqlFuncSource(SqlSource parent, Type reflectedType, List<SqlColumnDefinition> columns) : base(parent, reflectedType, columns)
        {
        }
    }
    
    public class SqlQuerySource : SqlSource
    {
        public SqlQuerySource(SqlSource parent, Type reflectedType, List<SqlColumnDefinition> columns) : base(parent, reflectedType, columns)
        {
        }
    }
    
    public class SqlQuerySelect
    {
        public SqlQuerySelect(SqlSource source)
        {
            Sources = new List<SqlSource>() { source };
        }

        public string[] Fields { get; set; }
        
        public List<SqlSource> Sources { get; set; }
        
        public string GroupBy { get; set; }
        public string Where { get; set; }
        public string Join { get; set; }
    }
}