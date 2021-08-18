using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeQuery.SqlExpressions;

namespace CodeQuery.SqlGenerators
{
    public class GenericSqlGenerator : ISqlGenerator
    {
        public string Select(SqlSelectExpression query)
        {
            var sql = new StringBuilder();
            // SELECT query.fields from [Source]
            // JOINS
            // WHERE
            
            // query.
            // query.sour
            // query.Fields.
            // var fields = query.Fields.Expressions.Select(Generate);
            // // query.Fields
            throw new System.NotImplementedException();
        }

        private string ToCommaList(IEnumerable<SqlExpression> list)
        {
            if (list == null)
            {
                return null;
            }
            return string.Join(", ", list.Select(l => Generate(l)));
        }

        private string Generate(SqlExpression exp)
        {
            switch (exp)
            {
                case SqlBinaryExpression binary:
                    return $"{Generate(binary.Left)} {binary.Operator} {Generate(binary.Right)}"; 
                case SqlColumnExpression column:
                    return column.Definition.Name;
                case SqlConstExpression @const:
                    return @const.Value.ToString();
                case SqlFunctionExpression func:
                    return $"{func.Type}({ToCommaList(func.Arguments)})";
            }

            return null;
        }
    }
}