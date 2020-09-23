using System.Linq;
using System.Text;
using CodeQuery.SqlExpressions;

namespace CodeQuery.SqlGenerators
{
    public class GenericSqlGenerator : ISqlGenerator
    {
        public string Select(SqlQuerySelect query)
        {
            var sql = new StringBuilder();
            var fields = query.Fields.Expressions.Select(Generate);
            // query.Fields
            throw new System.NotImplementedException();
        }

        private string Generate(SqlExpression exp)
        {
            switch (exp)
            {
                case SqlConstExpression @const:
                    return "";
            }

            return null;
        }
    }
}