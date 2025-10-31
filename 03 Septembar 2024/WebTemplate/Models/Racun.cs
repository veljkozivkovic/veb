namespace WebTemplate.Models;

public class Racun
{
    [Key]
    public int ID { get; set; }

    [Range(1, 12)]
    public required uint Mesec { get; set; }
    
    public uint? Struja { get; set; }

    public uint? Usluge { get; set; }

    public required uint Voda { get; set; }


    
    public bool? Placen { get; set; }


    public Stan? Stann { get; set; }

}