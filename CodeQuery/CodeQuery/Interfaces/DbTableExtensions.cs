using System;
using System.Linq.Expressions;

namespace CodeQuery.Interfaces
{
    public static class DbTableExtensions
    {
        public static int Count<T>(this T self, Expression<Func<T, object>> field) where T: DbTable
        {
            return default;
        }
        
        // Sum, min, max can be any primitive depending on the type
        public static TAny Sum<T, TAny>(this IDbTable<T> self, Expression<Func<T, TAny>> field)
            where TAny : struct 
        {
            return default;
        }
        
        public static TAny Min<T, TAny>(this T self, Expression<Func<T, TAny>> field) 
            where T: DbTable 
            where TAny: struct
        {
            return default;
        }
        
        public static TAny Max<T, TAny>(this T self, Expression<Func<T, TAny>> field) 
            where T: DbTable 
            where TAny: struct
        {
            return default;
        }
        
        public static double Average<T, TAny>(this T self, Expression<Func<T, TAny>> field) 
            where T: DbTable 
            where TAny: struct
        {
            return default;
        }
        
    }
}