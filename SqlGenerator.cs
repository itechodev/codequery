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
        
        public void Add(string content, bool newline = false)
        {
            builder.Append(content);
            if (newline)
            {
                NewLine();
            }
        }

        public void NewLineUnIndent()
        {
            UnIndent();
            NewLine();
        }

        public void NewLineIndent()
        {
            Indent();
            NewLine();
        }

        public void NewLine()
        {
            builder.AppendLine();
            builder.Append(new String(' ', indent * 4));
        }
        public string Generate()
        {
            return builder.ToString();
        }
    }
}