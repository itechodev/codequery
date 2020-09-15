using System;
using codequery.QuerySources;

namespace codequery.App
{
    public class Author 
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Book 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Author Author { get; set; }
        public int AuthorId { get; set; }
    }


    public class MyDatabase : Database
    {
        public DatabaseTable<Author> Authors { get; set; }
        public DatabaseTable<Book> Books { get; set; }
    }
}