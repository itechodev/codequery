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
namespace codequery.Expressions
{
    // There are three types of source
    // 1. Constant sources. Select 1, 2, 3
    // 2. Table souces. The most obvious: Select * from table
    // 3. SubQuery. Query from another query. Select * from (select 1)

    public abstract class QuerySource
    {
        public QuerySource(string alias)
        {
            Alias = alias;
        }
        public string Alias { get; set; }
    }

    public class TableSource : QuerySource
    {
        public string Table { get; set; }

        public TableSource(string table, string alias) : base(alias)
        {
            Table = table;
        }
    }

    public class ConstantSource : QuerySource
    {
        public ConstantSource(string alias, params SelectField[] fields) : base(alias)
        {
            Fields = fields;
        }

        public SelectField[] Fields { get; private set; }
    }

    public class SubQuerySource : QuerySource
    {
        public SubQuerySource(QuerySource source, string alias) : base(alias)
        {
            Source = source;
        }

        public QuerySource Source { get; private set; }
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
    public abstract class FieldExpression
    {
        public FieldType FieldType { get; set; }

        public FieldExpression(FieldType type) 
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
    public class ConstantExpression : FieldExpression
    {
        public ConstantExpression(FieldType type, object value): base(type)
        {
            this.Value = value;
        }
        public object Value { get; set; }
    }

    // Not a function or aggregate
    // CAST(9.5 AS INT)
    
    public class CastExpession : FieldExpression
    {
        // Casst field to type
        public CastExpession(FieldType type, FieldExpression field): base(type)
        {
            Field = field;
        }

        public FieldExpression Field { get; set; }
    }

    // Named query field like a.Age, a.name from [Source] a
    public class SourceFieldExpression : FieldExpression
    {
        public SourceFieldExpression(FieldType type, string name, QuerySource source): base(type)
        {
            Name = name;
            Source = source;
        }
        
        public string Name { get; set; }
        public QuerySource Source { get; set; }
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

    public abstract class CallbackExpression : FieldExpression
    {
          public CallbackExpression(FieldType type, params FieldExpression[] arguments): base(type)
        {
            this.Arguments = arguments;
        }

        public FieldExpression[] Arguments { get; set; }
    }

    // count(a.id), sum(b.value)
    public class AggregateExpression : CallbackExpression
    {
        public AggregateExpression(FieldType type, AggregateFunction function, params FieldExpression[] arguments): base(type, arguments)
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
    public class RowFunctionExpression : CallbackExpression
    {
        public RowFunctionExpression(FieldType type, FieldRowFunctionType function, params FieldExpression[] arguments): base(type, arguments)
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
    
    public class FunctionExpression : CallbackExpression
    {
        public FunctionExpression(FieldType type, FieldFunctionType function, params FieldExpression[] arguments): base(type, arguments)
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

    public class MathExpression : FieldExpression
    {
        public FieldExpression Left { get; private set; }
        public FieldExpression Right { get; private set; }
        public FieldMathOperator Op { get; private set; }

        public MathExpression(FieldType type, FieldExpression left, FieldMathOperator op, FieldExpression right) : base(type)
        {
            Left = left;
            Right = right;
            Op = op;
        }
    }


    // Any expressino with an optional alias
    public abstract class SelectField
    {
        public SelectField(FieldExpression exp, string alias)
        {
            Expression = exp;
            Alias = alias;
        }
        public string Alias { get; set; }
        public FieldExpression Expression { set; get; }
    }

    public class OrderByClause
    {
        public SourceFieldExpression By { get; set; }
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
        public FieldExpression OnClause { get; set; }
        public QuerySource Source { get; set; }
    }

    public class SelectQuery
    {
        public SelectField[] Fields { get; set; }
        public QuerySource From { get; set; }
        public JoinClause[] Joins { get; set; }
        public FieldExpression Where { get; set; }
        public SourceFieldExpression[] GroupBy { get; set; }
        public OrderByClause[] OrderBy { get; set; }
    }
    
}
