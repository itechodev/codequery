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
using System;
using System.Linq;
using codequery.QuerySources;

namespace codequery.Expressions
{
    // There are three types of source
    // 1. Constant sources. Select 1, 2, 3
    // 2. Table souces. The most obvious: Select * from table
    // 3. SubQuery. Query from another query. Select * from (select 1)

   

    public abstract class SqlQuerySource
    {
        public SqlQuerySource(string alias)
        {
            Alias = alias;
        }
        public string Alias { get; set; }
        
        public FieldType GetColumnType(string name)
        {
            return FieldType.String;
        }
    }

    public class SqlTableSource : SqlQuerySource
    {
    
        public SqlTableSource(TableDefinition def, string alias) : base(alias)
        {
            Definition = def;
        }

        public TableDefinition Definition { get; }
    }

    public class SqlConstantSource : SqlQuerySource
    {
        public SqlConstantSource(string alias, params SelectField[] fields) : base(alias)
        {
            Fields = fields;
        }

        public SelectField[] Fields { get; private set; }
    }

    public class SqlSubQuerySource : SqlQuerySource
    {
        public SqlSubQuerySource(SqlQuerySource source, string alias) : base(alias)
        {
            Source = source;
        }

        public SqlQuerySource Source { get; private set; }
    }

    public enum FieldType
    {
        Int,
        String,
        Char,
        Double,
        Binary,
        Bool
    }

    // The base of any expression as in: select [expression].. where [expression]
    public abstract class SqlExpression
    {
        public FieldType FieldType { get; set; }

        public SqlExpression(FieldType type)
        {
            FieldType = type;
        }
    }

    public enum ConstantSpecialType
    {
        Star
    }

    // Select 1
    // Can also be a count(*)
    public class SqlConstantExpression : SqlExpression
    {
        public SqlConstantExpression(FieldType type, object value) : base(type)
        {
            this.Value = value;
        }
        public object Value { get; set; }
    }

    // Not a function or aggregate
    // CAST(9.5 AS INT)

    public class SqlCastExpression : SqlExpression
    {
        // Casst field to type
        public SqlCastExpression(FieldType type, SqlExpression field) : base(type)
        {
            Field = field;
        }

        public SqlExpression Field { get; set; }
    }

    // Named query field like a.Age, a.name from [Source] a
    public class SqlColumnExpression : SqlExpression
    {
        public SqlColumnExpression(FieldType type, string name, SqlQuerySource source) : base(type)
        {
            Name = name;
            Source = source;
        }

        public string Name { get; set; }
        public SqlQuerySource Source { get; set; }
    }

    // List of aggregate functions codeQuery supports
    // Item to be added
    public enum AggregateFunction
    {
        Count,
        Sum,
        Min,
        Max,
        Average
    }

    public abstract class SqlCallbackExpression : SqlExpression
    {
        public SqlCallbackExpression(FieldType type, SqlExpression body, params SqlExpression[] arguments) : base(type)
        {
            this.Arguments = arguments;
            this.Body = body;
        }

        public SqlExpression[] Arguments { get; set; }
        public SqlExpression Body { get; set; }
    }

    // count(a.id), sum(b.value)
    public class AggregateExpression : SqlCallbackExpression
    {
        public AggregateExpression(FieldType type, SqlExpression body, AggregateFunction function, params SqlExpression[] arguments) : base(type, body, arguments)
        {
            this.Function = function;
        }

        public AggregateFunction Function { get; set; }
    }

    public enum FieldRowFunctionType
    {
        Distinct
        // Don't know of any any row functions yet
    }

    // Not the same a function
    public class RowFunctionExpression : SqlCallbackExpression
    {
        public RowFunctionExpression(FieldType type, SqlExpression body, FieldRowFunctionType function, params SqlExpression[] arguments) : base(type, body, arguments)
        {
            this.Function = function;
        }

        public FieldRowFunctionType Function { get; set; }
    }

    // Inmature list. To be added
    public enum FieldFunctionType
    {
        Lower,
        Upper,
        Substring,
        LeftTrim,
        RightTrim,
        Trim,
        Length,
        Replace,
        Contains
    }

    public class SqlFunctionExpression : SqlCallbackExpression
    {
        public SqlFunctionExpression(FieldType type, SqlExpression body, FieldFunctionType function, params SqlExpression[] arguments) : base(type, body, arguments)
        {
            this.Function = function;

        }
        public FieldFunctionType Function { get; set; }
    }

    public enum FieldMathOperator
    {
        Plus,
        Minus,
        Multiply,
        Divide,
        Equal,
        GreaterThan,
        LessThan,
        GreaterEqualThan,
        LessEqualThan,
        Or,
        And,
        StringConcat
    }

    public class SqlMathExpression : SqlExpression
    {
        public SqlExpression Left { get; private set; }
        public SqlExpression Right { get; private set; }
        public FieldMathOperator Op { get; private set; }

        public SqlMathExpression(FieldType type, SqlExpression left, FieldMathOperator op, SqlExpression right) : base(type)
        {
            Left = left;
            Right = right;
            Op = op;
        }
    }


    // Any expressino with an optional alias
    public class SelectField
    {
        public SelectField(SqlExpression exp, string alias)
        {
            Expression = exp;
            Alias = alias;
        }
        public string Alias { get; set; }
        public SqlExpression Expression { set; get; }
    }

    public class OrderByClause
    {
        public SqlColumnExpression By { get; set; }
        public bool Ascending { get; set; } = true;
    }

    public enum JoinType
    {
        Inner,
        Left,
        Rigth,
        Full,
        Cross
    }

    public class JoinClause
    {
        public string JoinType { get; set; }
        public SqlExpression OnClause { get; set; }
        public SqlQuerySource Source { get; set; }
    }

    public class SelectQuery
    {
        public SelectField[] Fields { get; set; }
        public SqlQuerySource From { get; set; }
        public JoinClause[] Joins { get; set; }
        public SqlExpression Where { get; set; }
        public SqlColumnExpression[] GroupBy { get; set; }
        public OrderByClause[] OrderBy { get; set; }
    }

}
