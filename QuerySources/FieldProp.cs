using System;

namespace codequery.QuerySources
{
    public class FieldProp
    {
        public FieldProp(Type type, string name, Func<object, object> getValue)
        {
            this.Type = type;
            this.Name = name;
            this.GetValue = getValue;

        }
        public Type Type { get; set; }
        public string Name { get; set; }
        public Func<object, object> GetValue { get; set; }
    }
}