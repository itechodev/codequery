namespace codequery.Expressions
{
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

}
