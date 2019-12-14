using System.Collections.Generic;
using codequery.Expressions;

namespace codequery.Drivers
{
    public interface IDatabaseDriver
    {
        string GenerateSelect(SqlSelectQuery query);
    }
}