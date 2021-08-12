using CodeQuery;
using CodeQuery.Interfaces;
using Sample.Tables;

namespace Sample
{
    
    // generated code...
    public class TestDb : DbSchema
    {
        public IDbQueryable<TopUp> TopUp { get; set; }
        public IDbQueryable<User> User { get; set; }
        public IDbQueryable<Enquiries> Enquiry { get; set; }
    }

    // Generated extensions...
    public static class JoinExtensions
    {
        public static IDbQueryable2<TopUp, User> JoinUser(this IDbQueryable<TopUp> query)
        {
            return query.Join<User>(JoinType.Inner);
        }
    }
}