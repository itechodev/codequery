namespace CodeQuery.Definitions
{
    public class SqlColumnDefinition
    {
        public SqlTable Owner { get; }
        public string Name { get; }
        public SqlColumnType Type { get; }
        public object DefaultValue { get; }
        
        public int Length { get;  }
        public int Precision { get;  }
        
        // fro postgres 10 generate as identity 
        public bool Identity { get; }
        
        // constraints
        public bool Nullable { get; }
        public bool PrimaryKey { get; }
        public bool Unique { get; }
        public SqlColumnDefinition ForeignKey { get; }
    }
}