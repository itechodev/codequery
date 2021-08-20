using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CodeQuery.Definitions;
using CodeQuery.Interfaces;
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
        public SqlSelectQuery SelectQuery { get; }
        public string Alias { get; }

        public SqlQuerySource(SqlSelectQuery selectQuery, string alias)
        {
            SelectQuery = selectQuery;
            Alias = alias;
        }
        
    }

    public class SqlTableSource : SqlSource
    {
        public string TableName { get; }

        public SqlTableSource(string tableName)
        {
            TableName = tableName;
        }
    }

    public class SqlJoinSource : SqlSource
    {
        public SqlSource Source;
        public SqlExpression Condition;
        public JoinType JoinType;

        public SqlJoinSource(SqlSource source, SqlExpression condition, JoinType joinType)
        {
            Source = source;
            Condition = condition;
            JoinType = joinType;
        }
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