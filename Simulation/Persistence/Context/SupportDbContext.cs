using Microsoft.EntityFrameworkCore;
using Simulation.Entities;

namespace Simulation.Persistence.Context;

public class SupportDbContext(DbContextOptions<SupportDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<SupportRequest> SupportRequests { get; set;  }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SupportDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}