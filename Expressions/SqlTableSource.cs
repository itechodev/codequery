using codequery.QuerySources;

namespace codequery.Expressions
{

    public class SqlTableSource : SqlQuerySource
    {
    
        public SqlTableSource(TableDefinition def, string alias) : base(def.Columns, alias)
        {
            Name = def.Name;
        }

        public string Name { get; }
    }

    public enum FieldType
    {
        Int,
        String,
        Char,
        Double,
        Binary,
        Bool
    }

   

}
