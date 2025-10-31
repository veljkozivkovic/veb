namespace WebTemplate.Models;

public class Iznajmljivanja
{
    [Key]
    public int ID { get; set; }

    public  Korisnik? Korisnik { get; set; }

    public  Kola? Kola { get; set; }

    public required uint BrojDana { get; set; }


}