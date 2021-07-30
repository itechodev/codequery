using System;
using CodeQuery;

namespace Sample.Tables
{
    public class TopUp : DbTable
    {
        public int Id { get; set; }
        public int UserId { get; set;  }
        public DateTime Added { get; set; }
        public int Count { get; set; }
    }
}