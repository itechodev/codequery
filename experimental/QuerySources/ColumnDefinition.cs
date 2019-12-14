using codequery.Expressions;

namespace codequery.QuerySources
{
    public class ColumnDefinition
    {
        public ColumnDefinition(string name, FieldType type)
        {
            this.Name = name;
            this.Type = type;

        }
        public ColumnDefinition(string name, FieldType type, string alias) 
        {
            this.Name = name;
            this.Type = type;
        }
        // More to come like precision length etc.
        public string Name { get; set; }
        public FieldType Type { get; set; }
    }
}