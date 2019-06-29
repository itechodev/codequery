namespace codequery
{
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

    // Select 1
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
