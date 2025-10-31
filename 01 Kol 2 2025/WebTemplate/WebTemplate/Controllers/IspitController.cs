using Models;

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

    [HttpPost("DodajBiblioteku")]
    public async Task<IActionResult> DodajBiblioteku([FromBody] Biblioteka b)
    {
        try
        {
            await Context.Biblioteka.AddAsync(b);
            await Context.SaveChangesAsync();
            return Ok($"Dodao si uspesno biblioteku! {b.ID}");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }


    [HttpPost("DodajKnjigu/{bibliotekaID}/{brojKnjiga}")]
    public async Task<IActionResult> DodajKnjigu([FromBody] Knjiga k, int bibliotekaID, int brojKnjiga)
    {
        try
        {

            Biblioteka? b = await Context.Biblioteka.FindAsync(bibliotekaID);

            if (b == null)
            {
                return NotFound($"Ne postoji biblioteka sa datim id-em: {bibliotekaID}");
            }

            await Context.Knjiga.AddAsync(k);

            Biblioteka_Knjiga bk = new()
            {
                Biblioteka = b,
                Knjiga = k,
                BrojKnjige = brojKnjiga
            };

            await Context.Biblioteka_Knjiga.AddAsync(bk);
            await Context.SaveChangesAsync();

            return Ok($"Dodao si uspesno knjigu {k.ID} u biblioteci {b.ID} sa kolicinom {brojKnjiga}");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("SveBiblioteke")]
    public async Task<IActionResult> SveBiblioteke()
    {
        try
        {
            return Ok(await Context.Biblioteka.Select(x => new
            {
                Id = x.ID,
                Naziv = x.Ime
            }).ToListAsync());
        }
        catch (Exception ec)
        {
            return BadRequest(ec.Message);
        }
    }


    [HttpGet("NadjiKnjigu/{BibliotekaID}")]
    public async Task<IActionResult> NadjiKnjigu(int BibliotekaID, [FromQuery] string? pretraga)
    {
        try
        {
            if(pretraga == null)
            {
                return BadRequest("Nisi uneo pretragu");
            }
            var knjiga = await Context.Biblioteka_Knjiga.Include(p => p.Knjiga).Include(p => p.Biblioteka)
                                                        .Where(p => p.Biblioteka!.ID == BibliotekaID && (p.Knjiga!.Autor.Contains(pretraga) || p.Knjiga!.Naziv.Contains(pretraga)))
                                                        .Select(p => new
                                                        {
                                                            Id = p.Knjiga!.ID,
                                                            Naziv = p.Knjiga!.Naziv,
                                                            Izdata = p.Knjiga!.Izdavanja!.Any(x => x.DatumVracanja == null)
                                                        })
                                                        .ToListAsync();
            return Ok(knjiga);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpPut("IzdajVrati/{KnjigaID}/{BibliotekaID}")]
    public async Task<IActionResult> IzdajVrati(int KnjigaID, int BibliotekaID)
    {
        try
        {
            var i = await Context.Izdavanje.Include(p => p.Knjiga).Include(p => p.Biblioteka)
                                           .Where(p => p.Knjiga!.ID == KnjigaID && p.Biblioteka!.ID == BibliotekaID && p.DatumVracanja == null).FirstOrDefaultAsync();

            if (i != null)
            {
                i.DatumVracanja = DateTime.Now;
                Context.Izdavanje.Update(i);
            }
            else
            {
                Biblioteka? b = await Context.Biblioteka.FindAsync(BibliotekaID);
                Knjiga? k = await Context.Knjiga.FindAsync(KnjigaID);
                if (k == null || b == null)
                {
                    return BadRequest("Ne postoji ili biblioteka ili knjiga sa datim ID-em");
                }

                Izdavanje izd = new()
                {
                    Knjiga = k,
                    Biblioteka = b,
                    DatumIzdavanja = DateTime.Now,
                    DatumVracanja = null
                };

                await Context.Izdavanje.AddAsync(izd);

            }

            await Context.SaveChangesAsync();

            return Ok($"Promenjeno je izdavanje ");

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("NajcitanijaKnjiga/{BibliotekaID}")]
    public async Task<IActionResult> NajcitanijeKnjiga(int BibliotekaID)
    {
        try
        {
            Knjiga? k = await Context.Biblioteka_Knjiga.Include(p => p.Knjiga).ThenInclude(w => w!.Izdavanja)
                                                      .Include(p => p.Biblioteka)
                                                      .Where(p => p.Biblioteka!.ID == BibliotekaID)
                                                      .OrderByDescending(p => p.Knjiga!.Izdavanja!.Count())
                                                      .Select(x => x.Knjiga)
                                                      .FirstOrDefaultAsync();

            if (k == null)
            {
                return BadRequest("Greska, nema knjige");
            }

            return Ok(k);
                                           
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }

    }


}
