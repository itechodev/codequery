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
using codequery.QuerySources;

namespace codequery.Expressions
{
    // There are three types of source
    // 1. Constant sources. Select 1, 2, 3
    // 2. Table souces. The most obvious: Select * from table
    // 3. SubQuery. Query from another query. Select * from (select 1)

    public abstract class SqlQuerySource
    {
        public SqlQuerySource(ColumnDefinition[] columns, string alias)
        {
            Alias = alias;
            Columns = columns;
        }
        public string Alias { get; set; }
        public ColumnDefinition[] Columns { get; }

        public FieldType GetColumnType(string name)
        {
            return FieldType.String;
        }

        public SqlReferencedField FieldByName(string name)
        {
            return new SqlReferencedField
            {
                Source = this,
                Column = Columns[0]
            };
        }

        public SqlReferencedField FieldByIndex(int index)
        {
            return new SqlReferencedField
            {
                Source = this,
                Column = Columns[index]
            };
        }
    }

}
