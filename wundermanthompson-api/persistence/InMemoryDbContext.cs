using Microsoft.EntityFrameworkCore;
using wundermanthompson_api.model;

namespace wundermanthompson_api.persistence;

public class InMemoryDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("wundermanthompson-api");
    }

    public DbSet<DataJob> DataJobs { get; set; }
    public DbSet<Result> Results { get; set; }
    public DbSet<Link> Links { get; set; }
}