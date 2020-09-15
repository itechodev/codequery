Most ORM's today try to do too much. Code Query ORM is different. It actually tries to do only one thing and that is generating efficient SQL from code.

Features:
* Write more efficient queries. CodeQuery lets you still use all your relevant SQL skills.
* No fancy features like object tracking. Just SQL.
* No complex executing plans. Either it transpiles to SQL or not at all.
* True database agnostic and all database drivers are written by die CodeQuery team.
* Completely un-opinioneded. Works with your design patterns in your application.

Simple, but difficult achievable through other ORMS's.

###Simple delete staments:
```c#
db
  .Products
  .Delete(p => p.Value <= 10)
```
Produce:
```sql
DELETE FROM Products WHERE Value <= 10
```

###Simple update without prefetching anything:
```c#
db
    .Products
    .Set(p => p.Value, p => p.Value * 2)
    .Where(p => p.Active);
    .Update();
```
Produce:
```sql
UPDATE Products SET Value = Value * 2 WHERE Active
```

### Explicit joins made easy:
```c#
var res = db.Books
  .LeftJoin(db.Category)
  .InnerJoin(db.Author)
  .Select((category, book, author) => {
    Book = book.Name,
    Author = author.Name,
    Category = category?.Name ?? "No category"
  })
.FetchMultiple();
```
Produce:
```sql
SELECT
   b.Name as 'Book',
   author.Name as 'Author',
   coalesce(c.Name, 'No Category') as 'Category' 
FROM Books b
LEFT JOIN Category c on c.Id = b.CategoryId
INNER JOIN Author a on a.Id = b.AuthorId      
 ```

### No limit to sub queries:

```c#
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
```

### Aggregation supported
```c#
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
```