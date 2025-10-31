 namespace Models;

public class Izdavanje
{
    [Key]
    public int ID { get; set; }

    public Knjiga? Knjiga { get; set; }

    public Biblioteka? Biblioteka { get; set; }

    public required DateTime DatumIzdavanja { get; set; }

    public DateTime? DatumVracanja { get; set; }

}