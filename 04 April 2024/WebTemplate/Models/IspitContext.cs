namespace WebTemplate.Models;

public class IspitContext : DbContext
{
    // DbSet kolekcije!
    public DbSet<Sala> Sala { get; set; }
    public DbSet<Sediste> Sediste { get; set; }
    public IspitContext(DbContextOptions options) : base(options)
    {
        
    }
}
