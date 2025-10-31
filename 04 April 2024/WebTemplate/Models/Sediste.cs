using System.ComponentModel;

namespace WebTemplate.Models;

public class Sediste
{
    [Key]
    public int ID { get; set; }

    public  int BrojReda { get; set; }
    
    public  int BrojSedista { get; set; }

    public Sala? Salaa { get; set; }

    public bool? Zauzeto { get; set; }

    public double? Cena{ get; set; }
}