using Models;

namespace WebTemplate.Models;

public class IspitContext : DbContext
{
    public DbSet<Biblioteka_Knjiga> Biblioteka_Knjiga { get; set; }

    public DbSet<Biblioteka> Biblioteka { get; set; }

    public DbSet<Izdavanje> Izdavanje { get; set; }

    public DbSet<Knjiga> Knjiga { get; set; }

    public IspitContext(DbContextOptions options) : base(options)
    {
        
    }
}
