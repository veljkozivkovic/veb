namespace WebTemplate.Models;


public class Korisnik
{
    [Key]
    public int ID { get; set; }

    public required string ImeIPrezime { get; set; }

    [Length(13, 13)]
    public required string JMBG { get; set; }
    
    [Length(9, 9)]
    public required string BrojDozvole { get; set; }
    
    public List<Iznajmljivanja>? Iznajmljivanjas { get; set; }


}