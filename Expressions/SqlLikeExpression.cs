namespace codequery.Expressions
{
    public enum SqlLikePattern
    {
        Start,
        End,
        Both
    }

    public class SqlLikeExpression : SqlExpression
    {
        public SqlLikeExpression(SqlExpression body, string pattern, SqlLikePattern patternType) : base(FieldType.Bool)
        {
            Body = body;
            Pattern = pattern;
            PatternType = patternType;
        }

        public SqlExpression Body { get; }
        public string Pattern { get; }
        public SqlLikePattern PatternType { get; }
    }

}
