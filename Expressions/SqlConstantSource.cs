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
using System.Linq;
using codequery.QuerySources;

namespace codequery.Expressions
{
    public class SqlConstantSource : SqlQuerySource
    {
        public SqlConstantSource(object[] rowValues, ColumnDefinition[] columns, string alias) : base(columns, alias)
        {
            // tuple or anonymous object
            RowValues = rowValues;
        }

        public object[] RowValues { get; }

        public SqlConstantExpression[] ValueExpressions
        {
            get
            {
                return Columns
                    .Select((c,i) => new SqlConstantExpression(c.Type, RowValues[i]))
                    .ToArray();
            }
        }
    }

}
