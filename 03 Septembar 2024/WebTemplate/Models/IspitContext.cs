namespace WebTemplate.Models;

public class IspitContext : DbContext
{
    // DbSet kolekcije!
    public DbSet<Stan> Stan { get; set; }

    public DbSet<Racun> Racun { get; set; }
    public IspitContext(DbContextOptions options) : base(options)
    {
        
    }
}
