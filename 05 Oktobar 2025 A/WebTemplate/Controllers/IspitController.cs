namespace WebTemplate.Controllers;

[ApiController]
[Route("[controller]")]
public class IspitController : ControllerBase
{
    public IspitContext Context { get; set; }

    public IspitController(IspitContext context)
    {
        Context = context;
    }

    // dodaj kucu
    [HttpPost("DodajKucu")]
    public async Task<ActionResult> DodajKucu([FromBody]Kuca kuca)
    {
        try
        {
            await Context.Kuca.AddAsync(kuca);
            await Context.SaveChangesAsync();
            return Ok($"Dodata je kuca sa ID {kuca.ID}");
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // vrati sve kuce
    [HttpGet("VratiSveKuce")]
    public async Task<ActionResult> VratiSveKuce()
    {
        try
        {
            return Ok(await Context.Kuca.Include(p => p.Kategorije).Select(p => new
            {
                ID = p.ID,
                Naziv = p.Naziv,
                Kategorije = p.Kategorije
            }).ToListAsync());
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    // vrati kucu sa dati ID
    [HttpGet("VratiKucu/{kucaId}")]
    public async Task<ActionResult> VratiKucu(int kucaId)
    {
        try
        {
            return Ok(await Context.Kuca.Include(p => p.Kategorije).Where(p => p.ID == kucaId).Select(p => new
            {
                ID = p.ID,
                Naziv = p.Naziv,
                Kategorije = p.Kategorije
            }).FirstOrDefaultAsync());
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("VratiKategorijeZaKucu/{kucaId}")]
    public async Task<ActionResult> VratiKategorijeZaKucu(int kucaId)
    {
        try
        {
            return Ok(await Context.Kategorija.Include(p => p.ProdukcijskaKuca).Include(p=>p.Filmovi)
            .Where(p=> p.ProdukcijskaKuca!.ID == kucaId)
            .Select(p => new
            {
                ID = p.ID,
                Naziv = p.Naziv,
                ProdukcijskaKuca = p.ProdukcijskaKuca,
                Filmovi = p.Filmovi
            }).ToListAsync());
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // dodaj kategoriju
    [HttpPost("DodajKategoriju/{kucaId}")]
    public async Task<ActionResult> DodajKategoriju([FromBody]Kategorija k, int kucaId)
    {
        try
        {
            var kuca = await Context.Kuca.Include(p => p.Kategorije).Where(p => p.ID == kucaId).FirstOrDefaultAsync();
            if(kuca == null)
            {
                return BadRequest($"Nema kuce sa ID-em {kucaId}");
            }
            Kategorija kat = new()
            {
                Naziv = k.Naziv,
                ProdukcijskaKuca = kuca
            };
            if(kuca.Kategorije == null)
            {
                kuca.Kategorije = new List<Kategorija>();
            }
            kuca.Kategorije.Add(kat);
            await Context.Kategorija.AddAsync(kat);
            await Context.SaveChangesAsync();
            return Ok($"Dodata je kategorija sa ID {kat.ID}");

        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // vrati sve kategorije
    [HttpGet("VratiSveKategorije")]
    public async Task<ActionResult> VratiSveKategorije()
    {
        try
        {
            return Ok(await Context.Kategorija.Include(p => p.ProdukcijskaKuca).Include(p=>p.Filmovi).Select(p => new
            {
                ID = p.ID,
                Naziv = p.Naziv,
                ProdukcijskaKuca = p.ProdukcijskaKuca,
                Filmovi = p.Filmovi
            }).ToListAsync());
        }
        catch(Exception e)
        { return BadRequest(e.Message); }
    }
    // vrati kategorije sa dati ID
    [HttpGet("VratiKategoriju/{kategorijaId}")]
    public async Task<ActionResult> VratiKategoriju(int kategorijaId)
    {
        try
        {
            return Ok(await Context.Kategorija.Include(p => p.ProdukcijskaKuca).Include(p => p.Filmovi).Where(p => p.ID == kategorijaId).Select(p => new
            {
                ID = p.ID,
                Naziv = p.Naziv,
                ProdukcijskaKuca = p.ProdukcijskaKuca,
                Filmovi = p.Filmovi
            }).FirstOrDefaultAsync());
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // dodaj film
    [HttpPost("DodajFilm/{kategorijaId}")]
    public async Task<ActionResult> DodajFilm([FromBody]Film f, int kategorijaId)
    {
        try
        {
            var kategorija = await Context.Kategorija.Include(p => p.Filmovi).Where(p => p.ID == kategorijaId).FirstOrDefaultAsync();
            if (kategorija == null)
            {
                return BadRequest($"Ne postoji kategorija sa ID: {kategorijaId}");
            }
            Film film = new()
            {
                Naziv = f.Naziv,
                KategorijaFilma = kategorija,
                Ocene = new List<double>()
            };
            if(kategorija.Filmovi == null)
            {
                kategorija.Filmovi = new List<Film>();
            }
            kategorija.Filmovi.Add(film);
            await Context.Film.AddAsync(film);
            await Context.SaveChangesAsync();
            return Ok($"Dodat je film sa id {film.ID}");
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("OceniFilm/{filmId}/{ocena}")]
    public async Task<ActionResult> OceniFilm(int filmId, double ocena)
    {
        try
        {
            var film = await Context.Film.Where(p => p.ID == filmId).FirstOrDefaultAsync();
            if (film == null)
            {
                return BadRequest($"Ne postoji film sa ID {filmId}");
            }
            if (film.Ocene == null)
            {
                film.Ocene = new List<double>();
            }
            film.Ocene.Add(ocena);
            await Context.SaveChangesAsync();
            return Ok($" Za film koji si ocenio {filmId} sad je prosecna ocena {film.Ocene.Average()}");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("VratiFilm/{filmId}")]
    public async Task<ActionResult> VratiFilm(int filmId)
    {
        try
        {
            return Ok(
                await Context.Film.Include(p => p.KategorijaFilma).Where(p => p.ID == filmId).Select(p => new
                {
                    ID = p.ID,
                    Naziv = p.Naziv,
                    KategorijaFilma = p.KategorijaFilma,
                    Ocena = p.Ocene!.Average()
                }
                ).FirstOrDefaultAsync()
            );
        }
        catch(Exception e)
        { return BadRequest(e.Message); }
    }

    // vrati sve filmove
    [HttpGet("VratiSveFilmove")]
    public async Task<ActionResult> VratiSveFilmove()
    {
        try
        {
            return Ok(
                await Context.Film.Include(p => p.KategorijaFilma).Select(p => new
                {
                    ID = p.ID,
                    Naziv = p.Naziv,
                    KategorijaFilma = p.KategorijaFilma,
                    Ocena = p.Ocene!.Average()
                }
                ).ToListAsync()
            );
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // vrati film sa dati ID
    [HttpGet("VratiFilmoveZaKategoriju/{kategorijaId}")]
    public async Task<ActionResult> VratiSveFilmoveZaKategoriju(int kategorijaId)
    {
        try
        {
            return Ok(
                await Context.Film.Include(p=>p.KategorijaFilma).Where(p => p.KategorijaFilma!.ID == kategorijaId).Select(p=> new
                {
                    ID = p.ID,
                    Naziv = p.Naziv,
                    KategorijaFilma = p.KategorijaFilma,
                    Ocena = p.Ocene!.Average()      
                }).ToListAsync()
            );
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    // vrati najbolje ocenjen film u kategoriji
    [HttpGet("NajgoreOcenjenFilm/{kategorijaId}")]
    public async Task <ActionResult> NajgoreOcenjenFilm(int kategorijaId)
    {
        try
        {
            return Ok(
                await Context.Film.Include(p => p.KategorijaFilma).Where(p => p.KategorijaFilma!.ID == kategorijaId)
                .OrderBy(p => p.Ocene!.Average()).Select(p => new
                {
                    ID = p.ID,
                    Naziv = p.Naziv,
                    Ocena = p.Ocene!.Average()
                })
                .FirstOrDefaultAsync()
            );
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // vrati srednje ocenjen film u kategoriji
    [HttpGet("SrednjeOcenjenFilm/{kategorijaId}")]
    public async Task<ActionResult> SrednjeOcenjenFilm(int kategorijaId)
    {
        try
        {
            Console.WriteLine("Uso sam u try");
            var kategorija = await Context.Kategorija.Include(p => p.Filmovi).Where(p => p.ID == kategorijaId).FirstOrDefaultAsync();
            if (kategorija == null)
            {
                BadRequest($" Nema kategorije sa ID {kategorijaId}");
            }
            int brojFilmova = kategorija!.Filmovi!.Count();
            Console.WriteLine("====================================" + brojFilmova);
            return Ok(
                await Context.Film.Include(p => p.KategorijaFilma)
                .Where(p => p.KategorijaFilma!.ID == kategorijaId)
                .OrderBy(p => p.Ocene!.Average())
                .Select(p => new
                {
                    ID = p.ID,
                    Naziv = p.Naziv,
                    Ocena = p.Ocene!.Average()
                }).ElementAtAsync(brojFilmova / 2)
            );
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //vrati najgore ocenjen film u kategoriji
    [HttpGet("NajboljeOcenjenFilm/{kategorijaId}")]
    public async Task<ActionResult> NajboljeOcenjenFilm(int kategorijaId)
    {
        try
        {
            return Ok(
                await Context.Film.Include(p => p.KategorijaFilma)
                                    .Where(p => p.KategorijaFilma!.ID == kategorijaId)
                                    .OrderByDescending(p => p.Ocene!.Average())
                                    .Select(p => new
                                    {
                                        ID = p.ID,
                                        Naziv = p.Naziv,
                                        Ocena = p.Ocene!.Average()
                                    }).FirstOrDefaultAsync()
            );
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}
