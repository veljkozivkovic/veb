namespace WebTemplate.Models;

public class Film
{
    [Key]
    public int ID { get; set; }


    public required string Naziv { get; set; }

    public Kategorija? KategorijaFilma { get; set; }

    public List<double>? Ocene{ get; set; }

}