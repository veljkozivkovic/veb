namespace Models;

public class Biblioteka_Knjiga
{
    [Key]
    public int ID { get; set; }

    public Biblioteka? Biblioteka { get; set; }

    public Knjiga? Knjiga { get; set; }
    
    public required int BrojKnjige{ get; set; }
}