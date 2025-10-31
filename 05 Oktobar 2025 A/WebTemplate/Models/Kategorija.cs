namespace WebTemplate.Models;

public class Kategorija
{
    [Key]
    public int ID { get; set; }

    public required string Naziv { get; set; }

    public Kuca? ProdukcijskaKuca { get; set; }

    public List<Film>? Filmovi { get; set; }
}