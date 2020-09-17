namespace CodeQuery.Definitions
{
    public enum SqlColumnType
    {
        Bool,
        
        // single character
        Char,
        
        // Real single precision floating-point number (32-bit)
        Single,
        // double precision floating-point number (64-bit) 
        Double,
        
        // signed 8-bit integer
        Int8,
        // signed 16-bit integer
        Int16,
        // signed 32-bit integer
        Int32,
        // signed 64-bit integer
        Int64,
        
        // unsigned 8-bit integer
        UInt8,
        // unsigned 16-bit integer
        UInt16,
        // unsigned 32-bit integer
        UInt32,
        // unsigned 64-bit integer
        UInt64,
        
        // Just a date (mm dd yyyy) 
        Date,
        Interval,
        // date and time
        TimeStamp,
        // date and time with timezone
        TimeStampWithZone,
        // only time with timzone
        TimeWithZone,

        // binary or array of bytes or blob
        Binary,
        // textual JSON data
        Json,
        // binary JSON data, decomposed
        JsonB,
        // variable-length character string. You can limit the maximum length 
        Varchar,
        
        // exact numeric of selectable precision
        Numeric,
        // variable-length character string. No maximum length
        Text,
        // UUID (universally unique identifier) / GUI (globally unique identifier) 
        UniqueIdentifier,
        Xml,
    }
}