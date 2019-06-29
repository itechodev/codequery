using System;
using System.Text;

namespace codequery
{
    public class SqlGenerator
    {
        private StringBuilder builder;
        private int indent;

        public SqlGenerator()
        {
            builder = new StringBuilder();
            indent = 0;
        }

        public void Indent()
        {
            indent++;
        }

        public void UnIndent()
        {
            indent--;
        }
        
        public void Add(string content)
        {
            builder.Append(content);
        }

        public void AddLine(string line)
        {
            builder.Append(new String(' ', indent * 4) + line);
            builder.AppendLine();
        }

        public string Generate()
        {
            return builder.ToString();
        }
    }
}