using EF.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace EF.Data
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }

        public DbSet<URLCollection> URLCollections { get; set; }
    }
}
