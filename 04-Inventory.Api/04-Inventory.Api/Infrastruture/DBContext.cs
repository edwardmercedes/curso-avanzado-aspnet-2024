using _04_Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace _04_Inventory.Api.Infrastruture
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext>options) : base(options) { }
        public DbSet<ARTICULOS> ARTICULOS { get; set; }
        public DbSet<CATEGORIA> CATEGORIA { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ARTICULOS>().HasKey(x => new {x.CODIGO});
            modelBuilder.Entity<CATEGORIA>().HasKey(x => new {x.CODIGO});
        }
    }
}
