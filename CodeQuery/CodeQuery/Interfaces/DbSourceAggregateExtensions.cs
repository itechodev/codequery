using System;
using System.Linq.Expressions;

namespace CodeQuery.Interfaces
{
    public static class DbSourceAggregateExtensions
    {
        public static int Count<T>(this T self, Expression<Func<T, object>> field) where T: IDbSource
        {
            return default;
        }
        
        // Sum, min, max can be any primitive depending on the type
        public static TAny Sum<T, TAny>(this T self, Expression<Func<T, TAny>> field) 
            where T: IDbSource
            where TAny : struct 
        {
            return default;
        }
        
        public static TAny Min<T, TAny>(this T self, Expression<Func<T, TAny>> field) 
            where T: IDbSource 
            where TAny: struct
        {
            return default;
        }
        
        public static TAny Max<T, TAny>(this T self, Expression<Func<T, TAny>> field) 
            where T: IDbSource 
            where TAny: struct
        {
            return default;
        }
        
        public static double Average<T, TAny>(this T self, Expression<Func<T, TAny>> field) 
            where T: IDbSource 
            where TAny: struct
        {
            return default;
        }
        
    }
}