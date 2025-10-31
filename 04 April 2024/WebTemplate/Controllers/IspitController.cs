using Microsoft.AspNetCore.Components.Forms;

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

    [HttpPost("DodajSalu")]
    public async Task<ActionResult> DodajSalu([FromBody] Sala s)
    {
        try
        {
            await Context.Sala.AddAsync(s);
            await Context.SaveChangesAsync();
            return Ok($"Sacuvana je sala sa id-em {s.ID} ");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpGet("VratiSveSale")]
    public async Task<ActionResult> VratiSveSale()
    {
        try
        {
            return Ok(await Context.Sala.Select(p => new
            {
                ID = p.ID,
                ImeFilma = p.ImeFilma,
                VremeReprodukcije = p.VremeReprodukcije,
                BrojSale = p.BrojSale,
                Sifra = p.Sifra,
                BrojRedova = p.BrojRedova,
                BazicnaCena = p.BazicnaCena,
                Sedista = p.Sedista,
                KapacitetSedista = p.KapacitetSedista,
                TrenutniBrojSedista = p.Sedista!.Count()
            }).ToListAsync());
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpPut("PromeniImeProjekcije/{salaId}/{novoIme}")]
    public async Task<ActionResult> PromeniImeProjekcije(int salaId, string novoIme)
    {
        try
        {
            var sala = await Context.Sala.FindAsync(salaId);
            if (sala == null)
            {
                return BadRequest($"Ne postoji sala sa id {salaId}");
            }
            sala.ImeFilma = novoIme;
            await Context.SaveChangesAsync();
            return Ok($"Promenjeno je ime sale sa id {sala.ID} u {sala.ImeFilma}");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //svaki red ima kartu 3% jeftiniju od prethodnog
    [HttpPost("DodajSediste/{salaId}")]
    public async Task<ActionResult> DodajSediste( int salaId)
    {
        try
        {
            var sala = await Context.Sala.Include(p => p.Sedista).Where(p=> p.ID == salaId).FirstOrDefaultAsync();


            
            if (sala == null)
            {
                return BadRequest($"Nema sale sa id: {salaId}");
            }
            if (sala.Sedista == null)
            {
                sala.Sedista = new List<Sediste>();
            }
            
            Console.WriteLine(sala.Sedista.ToArray().ToString());
            Console.WriteLine(sala.Sedista.Count());

            int brojSed = sala.Sedista.Count();
            int red = (int)brojSed / (sala.KapacitetSedista / sala.BrojRedova);
            double cena;

            Console.WriteLine(brojSed);
            Console.WriteLine(red);


            if (red == 0)
            {
                cena = sala.BazicnaCena;
            }
            else
            {
                cena = sala.BazicnaCena * (1.0 - red * 0.03);
            }


            Console.WriteLine($"Broj sed {brojSed}, red {red}, cena {cena}");

            if (sala.KapacitetSedista <= (brojSed + 1))
            {
                return BadRequest($"Kapacitet sale je {sala.KapacitetSedista}, a ovo sto dodajes bi bilo indeksa {brojSed}");
            }


            Sediste s = new()
            {
                BrojReda = red,
                BrojSedista = brojSed,
                Salaa = sala,
                Zauzeto = false,
                Cena = cena
            };
            s.BrojReda = red;
            s.BrojSedista = brojSed;
            s.Salaa = sala;
            Console.WriteLine(s.BrojReda);

            sala.Sedista.Add(s);
            await Context.Sediste.AddAsync(s);

            Console.WriteLine("AAAAAAAAAAAAAAAAAAA");

            Console.WriteLine(sala.Sedista.ToArray());
            

            await Context.SaveChangesAsync();

            return Ok($"Dodato je sediste {s.ID} u salu {sala.ID}");
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("VratiSedistaZaSalu/{salaId}")]
    public async Task<ActionResult> VratiSedistaZaSalu(int salaId)
    {
        try
        {
            return Ok(await Context.Sediste.Include(p => p.Salaa).Where(p => p.Salaa!.ID == salaId).Select(p => new
            {
                ID = p.ID,
                BrojReda = p.BrojReda,
                BrojSedista = p.BrojSedista,
                Cena = p.Cena,
                Zauzeto = p.Zauzeto
            }).ToListAsync());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("VratiSediste/{sedisteId}")]
    public async Task<ActionResult> VratiSediste(int sedisteId)
    {
        try
        {
            return Ok(await Context.Sediste.Include(p => p.Salaa).Where(p => p.ID == sedisteId).Select(p => new
            {
                ID = p.ID,
                BrojSedista = p.BrojSedista,
                BrojReda = p.BrojReda,
                Sifra = p.Salaa!.Sifra
            }).FirstOrDefaultAsync());
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //kupi kartu
    [HttpPut("KupiKartu/{sedisteId}")]
    public async Task<ActionResult> KupiKartu(int sedisteId)
    {
        try
        {
            var sediste = await Context.Sediste.FindAsync(sedisteId);
            if(sediste == null)
            {
                return BadRequest($"Greska, ne postoji sediste sa dati ID {sedisteId}");
            }
            sediste.Zauzeto = true;
            await Context.SaveChangesAsync();
            return Ok($"Kupljena je karta za sediste sa id {sediste.ID}");
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
