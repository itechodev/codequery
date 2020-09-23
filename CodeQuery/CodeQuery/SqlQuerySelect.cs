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
    }
    
    // Select const fields without a source
    // Select 1, 2
    public class SqlNoSource : SqlSource
    {
        
    }

    // ie. select * from generate(1, 100)
    public class SqlFuncSource : SqlSource
    {
        
    }
    
    public class SqlQuerySource : SqlSource
    {
        
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