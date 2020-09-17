using System;
using CodeQuery.Interfaces;

namespace CodeQuery
{
    public abstract class DatabaseContext
    {
        public IDbQueryable<TFields> Const<TFields>(Func<TFields> fields)
        {
            return null;
        }

        protected void Initialize()
        {
            // Read all DbTables from this instance and convert this into table definitions
        }
    }
}