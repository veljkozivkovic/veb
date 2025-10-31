namespace WebTemplate.Models;

public class Modeli
{
    [Key]
    public int ID { get; set; }

    public required string Naziv { get; set; }
    public List<Kola>? Kolas { get; set; }
}