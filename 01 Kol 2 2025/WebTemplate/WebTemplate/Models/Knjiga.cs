namespace Models;

public class Knjiga
{
    [Key]
    public int ID { get; set; }

    public required string Naziv { get; set; }

    public required string Autor { get; set; }

    [Range(1456, 2025)]
    public required int GodinaIzdavanja { get; set; }

    public required string NazivIzdavaca { get; set; }
    
    public List<Biblioteka_Knjiga>? Biblioteka_Knjigas { get; set; }
    
    public List<Izdavanje>? Izdavanja { get; set; }

}