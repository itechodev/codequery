namespace codequery.Expressions
{
    // Named query field like a.Age, a.name from [Source] a
    public class SqlColumnExpression : SqlExpression
    {
        public SqlColumnExpression(SqlReferencedField field): base(field.Column.Type)
        {
            Name = field.Column.Name;
            Source = field.Source;
        }

        public SqlColumnExpression(FieldType type, string name, SqlQuerySource source) : base(type)
        {
            Name = name;
            Source = source;
        }

        public string Name { get; set; }
        public SqlQuerySource Source { get; set; }
    }

}
