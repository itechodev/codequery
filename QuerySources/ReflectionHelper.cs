using System;
using System.Linq;

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
    public static class ReflectionHelper
    {
        public static FieldProp[] GetFiedAndProps(Type source)
        {
            // combine fields for tupes and properties for normal classes
            return source
                .GetProperties()
                .Select(p => new FieldProp(p.PropertyType, p.Name, (obj) => p.GetValue(obj)))
                .Concat(
                    source.GetFields()
                    .Select(f => new FieldProp(f.FieldType, f.Name, obj => f.GetValue(obj)))
                )
                .ToArray();
        }
    }
}