using CodeQuery;
using CodeQuery.Interfaces;
using Sample.Tables;

namespace Sample
{
    public class TestDb : DbSchema
    {
        public void Query()
        {
            var results = From<TopUp>()
                .Join<User>(JoinType.Inner)
                .Where((up, user) => up.Id < 100 && user.Id > 200)
                .GroupBy((up, user) => user.Id)
                .Select(a => new
                {
                    UserId = a.Key,
                    MinId = a.Min((t, _) => t.Id),
                    MaxId = a.Max((t, _) => t.Id),
                    Count = a.Sum((t, _) => t.Count),
                });
        }
        
    }
}