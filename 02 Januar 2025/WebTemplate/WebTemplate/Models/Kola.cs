namespace WebTemplate.Models;

public class Kola
{
    [Key]
    public int ID { get; set; }

    public required uint Kilometraza { get; set; }

    [Range(1930, 2026)]
    public required uint Godiste { get; set; }

    public required uint BrojSedista { get; set; }
    public required uint CenaPoDanu { get; set; }
    
    public  Modeli? Model { get; set; }

    [ForeignKey("IznajmljivanjeFK")]
    public Iznajmljivanja? Iznajmljivanje { get; set; }

    public required bool DaLiJeIznajmljen{ get; set; }

}