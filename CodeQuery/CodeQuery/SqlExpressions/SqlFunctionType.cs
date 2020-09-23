namespace CodeQuery.SqlExpressions
{
    public enum SqlFunctionType
    {
        // String
        Concat,
        Left, 
        Length,
        Right,
        Trim,
        Substr,
        
        // DateTime
        CurrentDate,
        CurrentTime,
        CurrentTimeStamp,
        TimestampToString
    }
}