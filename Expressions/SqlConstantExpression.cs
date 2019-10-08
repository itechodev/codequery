
namespace codequery.Expressions
{

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

}
