using WebTemplate.DTOs;

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


    [HttpPost("DodajKorisnika")]
    public async Task<ActionResult> DodajKorisnika([FromBody]Korisnik k)
    {
        try
        {
            await Context.Korisnik.AddAsync(k);
            await Context.SaveChangesAsync();
            return Ok($"Dodat je korisnik sa ID: {k.ID}");
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("DodajIznajmljivanje/{kolaId}/{korisnikID}")]
    public async Task<ActionResult> DodajIznajmljivanje([FromBody] Iznajmljivanja i, int kolaId, int korisnikID)
    {
        try
        {
            var kola = await Context.Kola.FindAsync(kolaId);
            var korisnik = await Context.Korisnik.FindAsync(korisnikID);

            if (kola != null && korisnik != null)
            {
                Iznajmljivanja iz = new()
                {
                    Korisnik = korisnik,
                    Kola = kola,
                    BrojDana = i.BrojDana
                };

                kola.DaLiJeIznajmljen = true;
                Context.Kola.Update(kola); //nzm treba li
                await Context.Iznajmljivanja.AddAsync(iz);
                await Context.SaveChangesAsync();

                return Ok($"Dodato iznajmljivanje sa ID: {iz.ID}");
            }
            else
            {
                return BadRequest("Nesto ne valja, null je ili auto ili korisnik.");
            }
            

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpPost("DodajModel")]
    public async Task<ActionResult> DodajModel([FromBody] Modeli model)
    {
        try
        {
            await Context.Modeli.AddAsync(model);
            await Context.SaveChangesAsync();
            return Ok($"Dodat je model sa ID {model.ID}");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("DodajKola/{modelID}")]
    public async Task<ActionResult> DodajKola([FromBody] Kola k, int modelID)
    {
        try
        {
            var model = await Context.Modeli.FindAsync(modelID);
            if (model != null)
            {
                Kola kola = new()
                {
                    Kilometraza = k.Kilometraza,
                    Godiste = k.Godiste,
                    BrojSedista = k.BrojSedista,
                    CenaPoDanu = k.CenaPoDanu,
                    Model = model,
                    DaLiJeIznajmljen = false
                };
                if (model.Kolas == null)
                {
                    model.Kolas = new List<Kola>();
                }
                model.Kolas.Add(kola);
                await Context.Kola.AddAsync(kola);
                await Context.SaveChangesAsync();
                return Ok($"Dodata su kola sa id {kola.ID}");
            }
            else
            {
                return BadRequest("Ne postoji model");
            }


        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpGet("VratiSvaKola")]
    public async Task<ActionResult> VratiSvaKola()
    {
        try
        {
            return Ok(await Context.Kola.Include(p => p.Model).Include(p=> p.Iznajmljivanje).Select(p => new
            {
                Id = p.ID,
                Kilometraza = p.Kilometraza,
                Godiste = p.Godiste,
                BrojSedista = p.BrojSedista,
                CenaPoDanu = p.CenaPoDanu,
                Model = p.Model!.Naziv,
                DaLiJeIznajmljen = p.DaLiJeIznajmljen

            }).ToListAsync());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("PretraziKola")]
    public async Task<ActionResult> PretraziKola([FromQuery]UpitDTO upit)
    {
        try
        {
            var kola = await Context.Kola.Include(p => p.Model) // mora include jer je Model tip klase Modeli i mora include ako oces da izvuces njihova atribut
                                          .Where(p => p.Kilometraza == upit.PredjenaKilometraza || p.BrojSedista == upit.BrojSedista || p.CenaPoDanu == upit.Cena || p.Model!.ID == upit.ModelID) // u sustini ovako postavljas uslove where
                                          .Select(p => new
                                          {
                                              ID = p.ID,
                                              Kilometraza = p.Kilometraza,
                                              Godiste = p.Godiste,
                                              Model = p.Model!.Naziv,
                                              CenaPoDanu = p.CenaPoDanu,
                                              brojSedista = p.BrojSedista,
                                              DaLiJeIznajmljen = p.DaLiJeIznajmljen
                                          }).ToListAsync();
            return Ok(kola);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("VratiSveModele")]
    public async Task<ActionResult> VratiSveModele()
    {
        try
        {
            return Ok(await Context.Modeli.Select(p => new
            {
                ID = p.ID,
                Naziv = p.Naziv
            }).ToListAsync());
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }


}
