using expenceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenceTracker.Context
{
    public class TrackerContext : DbContext
    {
        public TrackerContext(DbContextOptions<TrackerContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }


    }
}
