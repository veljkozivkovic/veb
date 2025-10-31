namespace Models;

public class Biblioteka
{
    [Key]
    public int ID { get; set; }

    public required string Ime { get; set; }

    public required string Adresa { get; set; }

    public required string EmailAdresa { get; set; }
    
    public List<Biblioteka_Knjiga>? Biblioteka_Knjigas { get; set; }

    
    public List<Izdavanje>? Izdavanja { get; set; }
    


}