namespace WebTemplate.Models;

public class Sala
{
    [Key]
    public int ID { get; set; }

    public required string ImeFilma { get; set; } // da l ce tupe kurac sto ne cuvam projekciju za sebe jebemliga

    public required DateTime VremeReprodukcije { get; set; }

    public required int BrojSale { get; set; }

    [Length(3,3)]
    public required string Sifra { get; set; }

    public required int BrojRedova { get; set; }

    public required int KapacitetSedista{ get; set; }
    public required double BazicnaCena { get; set; }
    
    public List<Sediste>? Sedista { get; set; }


}