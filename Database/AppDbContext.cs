using Microsoft.EntityFrameworkCore;
using TikectingBooking.Api.Database.Entities;

namespace TikectingBooking.Api.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Concert> Concerts => Set<Concert>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Kita konfigurasi kolom Version ini sebagai alat "Concurrency Token"
        modelBuilder.Entity<Concert>()
            .Property(c => c.Version)
            .IsRowVersion(); 
    }
}
