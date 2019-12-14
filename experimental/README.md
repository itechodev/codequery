Most ORM's today try to do too much. Code first Query ORM is different.

It's a code first approach to write SQL queries.
-Write more efficient queries. CodeQuery lets you still use all your relevant SQL skills.
-No fancy features like object tracking. Just SQL.
-No local complex executing plans. Either it transpiles to SQL or not at all.
-True database agnostic and no all database drivers are written by die codeQuery team.
-Completly un-opinionted. Works with your design patterns in your application.

Simple, but difficult achievable through other ORMS's.

db
    .Products
    .Delete(p => p.Value <= 10)

Procudes: "Delete from Products where Value <= 10"

db
    .Products
    .Set(p => p.Value, p => p.Value * 2)
    .Where(p => p.Active);
    .Update();

Produces: "Update Products set Value = Value * 2 Where Active"

Be explicit in the types of joins

var res = db.Books
    .LeftJoin(db.Category)
    .InnerJoin(db.Author)
    .Select((category, book, author) => {
        Book = book.Name,
        Author = author.Name,
        Category = category?.Name ?? "No category"
    })
    .FetchMultiple();
    
Any amount of sub queries.

var a = db.Books
    .GroupBy(b => b.AuthorId)
    .Select(aggr => {
        AuthorId = aggr.Value,
        Count = aggr.Count()
    })
    .InnerJoin(b.Author, (b,a) => b.AuthorId == a.Id)
    .Select((book, author) => {

    })
    .FetchMultiple();

Multiple sub queries with aggregates
    var b = db.Enquiries
        .GroupBy(e => e.UserId);
        .Select(e => {
            UserID = e.Key,
            Used = e.Count()
        })
        .InnerJoin(db.Topup.., on, (l, r) => new { Used = L, Topup = r; })
        .InnerJoin(db.Users, .., (prev, r) => new { Userd = prev.Used, Topup = prev.Topup, Users = r) })
        .Select(u => {
            Used = u.Left.Right.Topups - u.left.Count(),
            Name = u.Right.Name,
        })
        
