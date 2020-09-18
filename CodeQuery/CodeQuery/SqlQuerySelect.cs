using System.Collections.Generic;
using CodeQuery.Definitions;

namespace CodeQuery
{
    public abstract class SqlSource
    {
        
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
    
    public class SqlTableSource : SqlSource
    {
        public SqlTableDefinition Definition { get; }

        public SqlTableSource(SqlTableDefinition definition)
        {
            Definition = definition;
        }
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