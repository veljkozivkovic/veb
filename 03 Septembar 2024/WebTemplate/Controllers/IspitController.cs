using System.Reflection.Emit;
using Microsoft.Identity.Client;

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


    [HttpPost("DodajStan")]
    public async Task<ActionResult> DodajStan([FromBody] Stan s)
    {
        try
        {
            await Context.Stan.AddAsync(s);
            await Context.SaveChangesAsync();
            return Ok($"Dodat je stan sa id {s.ID}");
            
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("DodajRacun/{stanID}")]
    public async Task<ActionResult> DodajRacun([FromBody] Racun r, int stanID)
    {
        try
        {
            var stan = await Context.Stan.FindAsync(stanID);

            if (stan == null)
            {
                return BadRequest("Ne postoji stanat");
            }
            Racun rac = new()
            {
                Mesec = r.Mesec,
                Struja = 100 * stan.BrojClanova,
                Usluge = 150 * stan.BrojClanova,
                Voda = r.Voda,
                Stann = stan,
                Placen = false
            };

            if (stan.Racuni == null)
            {
                stan.Racuni = new List<Racun>();
            }

            stan.Racuni.Add(rac);
            
            await Context.Racun.AddAsync(rac);
            await Context.SaveChangesAsync();
            return Ok($"Dodat racun {rac.ID} za stanat {stan.ID}");

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }



    [HttpGet("VratiSveStanove")]
    public async Task<ActionResult> VratiSveStanove()
    {
        try
        {
            return Ok(await Context.Stan.Include(p => p.Racuni).Select(p => new
            {
                ID = p.ID,
                ImeVlasnika = p.ImeVlasnika,
                BrojClanova = p.BrojClanova,
                Racuni = p.Racuni
            }).ToListAsync()
            );
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("VratiStan/{stanId}")]
    public async Task<ActionResult> VratiStan(int stanId)
    {
        try
        {
            return Ok(await Context.Stan.Include(p => p.Racuni).
            Where(p => p.ID == stanId)
            .Select(p => new
            {
                ID = p.ID,
                ImeVlasnika = p.ImeVlasnika,
                BrojClanova = p.BrojClanova,
                Racuni = p.Racuni
            }).ToListAsync()
            );
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    
    [HttpGet("VratiSveRacuneIkad")]
    public async Task<ActionResult> VratiSveRacuneIkad()
    {
        try
        {
            return Ok(await Context.Racun.Include(p => p.Stann)
            .Select(p => new
            {
                ID = p.ID,
                Mesec = p.Mesec,
                Struja = p.Struja,
                Usluge = p.Usluge,
                Voda = p.Voda,
                Placen = p.Placen,
                Stann = p.Stann
            }).ToListAsync()
            );
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //Vrati ukupno zaduzenje
    [HttpGet("UkupnoZaduzenjeZaStan/{stanId}")]
        public async Task<ActionResult> UkupnoZaduzenjeZaStan(int stanId)
        {
            try
            {
                return Ok(await Context.Racun.Include(p => p.Stann)
                .Where(p => p.Stann!.ID == stanId && p.Placen == false)
                .SumAsync(p=> p.Struja + p.Usluge + p.Voda)
                );

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    //Vrati za stan sve racune
    [HttpGet("VratiRacuneZaStan/{stanId}")]
    public async Task<ActionResult> VratiRacuneZaStan(int stanId)
    {
        try
        {
            return Ok(await Context.Racun.Include(p => p.Stann)
                                         .Where(p => p.Stann!.ID == stanId)
                                         .Select(p => new
                                         {
                                             ID = p.ID,
                                             Mesec = p.Mesec,
                                             Struja = p.Struja,
                                             Usluge = p.Usluge,
                                             Voda = p.Voda,
                                             Placen = p.Placen,
                                             Stann = p.Stann
                                         }).ToListAsync()
            );
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("IzbrisiRacun/{racunId}")]
    public async Task<ActionResult> IzbrisiRacun(int racunId)
    {
        try
        {
            var racun = await Context.Racun.FindAsync(racunId);

            if (racun == null)
            {
                return BadRequest("Nema ga rachun");
            }

            Context.Racun.Remove(racun);
            await Context.SaveChangesAsync();
            return Ok($"Uklonjen je uspesno racun sa id {racun.ID} ");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("UplatiRacun/{racunId}")]
    public async Task<ActionResult> UplatiRacun(int racunId)
    {
        try
        {
            var racun = await Context.Racun.FindAsync(racunId);
            if(racun == null)
            {
                return BadRequest("Ne postoji rachun");
            }
            racun.Placen = true;
            await Context.SaveChangesAsync();
            return Ok($"Placen provera {racun.Placen}");
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}
