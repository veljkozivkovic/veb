namespace WebTemplate.Models;

public class IspitContext : DbContext
{
    // DbSet kolekcije!

    public DbSet<Korisnik> Korisnik { get; set; }

    public DbSet<Kola> Kola { get; set; }

    public DbSet<Iznajmljivanja> Iznajmljivanja { get; set; }

    public DbSet<Modeli> Modeli { get; set; }

    public IspitContext(DbContextOptions options) : base(options)
    {
        
    }
}
