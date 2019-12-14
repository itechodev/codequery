namespace codequery.SqlExpressions
{
    public enum FieldType
    {
        Byte,
        SmallInt,
        String,
        Char,
        Double,
        Binary,
        Bool
    }

    public class ColumnDefinition
    {
        public ColumnDefinition(string name, FieldType type)
        {
            this.Name = name;
            this.Type = type;
        }
        public string Name { get; set; }
        public FieldType Type { get; set; }
    }

    public class TableDefinition
    {
        public string Name { get; set; }
        public ColumnDefinition[] Columns { get; set; }
    }

    public abstract class SqlExpression
    {

    }

    public class SqlColumnExpression : SqlExpression
    {
        public SqlColumnExpression(ColumnDefinition definition)
        {
            Definition = definition;
        }

        public ColumnDefinition Definition { get; }
    }

    public class SqlSelectAliasExpression : SqlExpression
    {
        public SqlSelectAliasExpression(SqlExpression body, string alias)
        {
            Body = body;
            Alias = alias;
        }

        public SqlExpression Body { get; }
        public string Alias { get; }
    }

  
    public enum SqlBooleanOperator
    {
        And,
        Or,
        Xor,
        Equal,
        GreaterThen,
        LessThen,
        GreateEqualThan,
        LessEqualThan,
    }
    
    public class SqlNegateExpression : SqlExpression
    {

    }

    // a == b, a >= b, 
    // a where a is boolean
    // a || b, a && b where a 
    public class SqlBooleanExpression : SqlExpression
    {
        public SqlBooleanExpression(SqlBooleanOperator sqlBinaryOperator, SqlExpression left, SqlExpression right)
        {
            SqlBinaryOperator = sqlBinaryOperator;
            Left = left;
            Right = right;
        }

        public SqlBooleanOperator SqlBinaryOperator { get; }
        public SqlExpression Left { get; }
        public SqlExpression Right { get; }
    }

    public enum SqlBinaryOperator
    {
        Plus,
        Divice,
        Multiply,
        Minus,
    }
    

    public class SqlBinaryExpression : SqlExpression
    {
        public SqlBinaryExpression(SqlBinaryOperator sqlBinaryOperator, SqlExpression left, SqlExpression right)
        {
            SqlBinaryOperator = sqlBinaryOperator;
            Left = left;
            Right = right;
        }

        public SqlBinaryOperator SqlBinaryOperator { get; }
        public SqlExpression Left { get; }
        public SqlExpression Right { get; }
    }



    public class SqlSelectExpression : SqlExpression
    {

    }

// "Enquired"
//     SqlColumnExpression
// ::date
//     SqlCastExpression / SqlConvertExpression
// sum(..)
//     SqlAggregateExpression
// CASE ... END
//     SqlCaseExpression
// WHEN {BoolExpe} THEN
//     SqlWhenThenExpression
// ELSE ..
//     SqlElseExpression
// as "CompletedCount
//     SqlAliasExpression
// FROM "Tables"
//     SqlTableExpression
// Where {booleanExpression}
//     SqlBooleanExperssion
// Group By { SqlEpxression[] }
//     SqlGroupbyExpression
// Order by { SqlEpxression[] }


}