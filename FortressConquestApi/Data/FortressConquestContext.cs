using FortressConquestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FortressConquestApi.Data
{
    public class FortressConquestContext : DbContext
    {
        public FortressConquestContext(DbContextOptions<FortressConquestContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Fortress> Fortresses { get; set; } = null!;
        public DbSet<Character> Characters { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.FortressesCreated)
                .WithOne(f => f.Creator)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.FortressesOwned)
                .WithOne(f => f.Owner)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
