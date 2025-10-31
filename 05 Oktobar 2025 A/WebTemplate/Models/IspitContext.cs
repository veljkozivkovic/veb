namespace WebTemplate.Models;

public class IspitContext : DbContext
{
    // DbSet kolekcije!
    public DbSet<Kuca> Kuca { get; set; }
    public DbSet<Kategorija> Kategorija { get; set; }
    public DbSet<Film> Film { get; set; }
    
    public IspitContext(DbContextOptions options) : base(options)
    {
        
    }
}
