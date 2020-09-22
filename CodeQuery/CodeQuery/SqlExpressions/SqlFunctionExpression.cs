namespace CodeQuery.SqlExpressions
{
    public enum SqlFunctionType
    {
        // String
        Concat,
        Left, 
        Length,
        Right,
        
        // DateTime
        CurrentDate,
        CurrentTime,
        CurrentTimeStamp
    }
    
    // all math, string, int and date functions
    // ie. abs, ceil, exp, floor, ln, log, mod, substring, trim etc.
    public class SqlFunctionExpression
    {
        // DateTime.Now
    }
}