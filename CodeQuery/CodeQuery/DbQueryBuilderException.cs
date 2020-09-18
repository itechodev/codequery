using System;

namespace CodeQuery
{
    public class DbQueryBuilderException: Exception
    {
        public DbQueryBuilderException(string message) : base(message)
        {
        }
    }
}