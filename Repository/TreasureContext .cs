using Microsoft.EntityFrameworkCore;
using TreasureBackEnd.Model;

namespace TreasureBackEnd.Repository
{
    public class TreasureContext : DbContext

    {
        public TreasureContext(DbContextOptions<TreasureContext> options)
       : base(options)
        {
        }
        public DbSet<TreasureRecord> TreasureRecords { get; set; }
    }
}
