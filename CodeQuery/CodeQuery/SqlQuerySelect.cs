using System;
using System.Collections.Generic;
using CodeQuery.Definitions;
using CodeQuery.SqlExpressions;

namespace CodeQuery
{
    /// <summary>
    /// The structure as returned from a query, table or generation function
    /// </summary>
    public class ResultDefinition
    {
        public List<SqlColumnDefinition> Columns { get; protected set; }
    }
    
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
    
    public class SqlQuerySource : SqlSource
    {
        
    }

    public class SqlTableSource : SqlSource
    {
        
    }

    public class SqlJoinSource : SqlSource
    {
        public SqlSource Left;
        public SqlSource Right;
    }

    public class SqlUnionSource : SqlSource
    {
        public SqlSource Top;
        public SqlSource Bottom;
    }
    
    public class SqlQuerySelect
    {
        public SqlSource Source { get; set; }

    }
}