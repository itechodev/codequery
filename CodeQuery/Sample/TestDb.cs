using CodeQuery;
using CodeQuery.Definitions;
using CodeQuery.Interfaces;
using Sample.Tables;

namespace Sample
{
    
    // generated code...
    public class TestDb : DbSchema
    {
        public IDbTable<TopUp> TopUp { get; set; }
        public IDbTable<User> User { get; set; }
        public IDbTable<Enquiries> Enquiry { get; set; }

        public TestDb()
        {
            TopUp = CreateTable<TopUp>();
            User = CreateTable<User>();
            Enquiry = CreateTable<Enquiries>();
        }
    }
}