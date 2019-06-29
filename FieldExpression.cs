namespace codequery
{
    public class QuerySource
    {

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
        public FieldExpression Left { get; set; }
        public FieldExpression Right { get; set; }

        public MathExpression(FieldType type, FieldExpression left, FieldMathOperator op, FieldExpression right) : base(type)
        {
            Left = left;
            Right = right;
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
    
}
