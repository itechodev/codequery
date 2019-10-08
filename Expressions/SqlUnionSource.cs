// Select coalesce( datetime('now') > null, 0), 'str' == 'aa', (select 10) + 10

//  SELECT
//     [Field...]
// FROM
//     [SOURCE]
// JOIN [SOURCE] on [ConditionalExpression...]
// WHERE [ConditionalExpression...] 
// ORDER BY [FieldName...] ASC/DESC
// GROUP BY [FieldName...]
// HAVING [HavingExpression] 

// ConditionalExpression: [Field] [Operator] [Field]
// Field: FieldConstant, FieldName, FieldAggregate, FieldRowFunction, FieldExpression
// FieldExpression: Func([Field], [Field] [Operator] [Field]
// HavingExpression: [FieldAggregate] [Operator] [Field]

// FieldSelectExpression: FieldConstant, FieldName, FieldAggregate, FieldRowFunction, FieldOperator, FieldBoolean, FieldExpression
// FieldWhere: FieldBoolean 


// FieldMath
// FieldFunction

// This only lists the clases necessary to generate SQL
// It's no safegauard for generating invalid SQL
using System.Collections.Generic;
using codequery.QuerySources;

namespace codequery.Expressions
{
    public class SqlUnionSource : SqlQuerySource
    {
        public SqlUnionSource(bool unionAll, ColumnDefinition[] columns, string alias) : base(columns, alias)
        {
            Sources = new List<SqlQuerySource>();
            // Is it a union or union all?
            UnionAll = unionAll;
        }
        
        public SqlUnionSource(SqlQuerySource top, SqlQuerySource bottom, bool unionAll, ColumnDefinition[] columns, string alias) : base(columns, alias)
        {
            Sources = new List<SqlQuerySource> { top, bottom };
            // Is it a union or union all?
            UnionAll = unionAll;
        }

        public List<SqlQuerySource> Sources { get; set; }
        public bool UnionAll { get; }
    }

}
