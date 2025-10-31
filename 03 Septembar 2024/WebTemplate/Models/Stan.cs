using Microsoft.Identity.Client;

namespace WebTemplate.Models;

public class Stan
{
    [Key]
    public int ID { get; set; }

    public required string ImeVlasnika { get; set; }

    [Range(1, 6)]
    public required uint BrojClanova { get; set; }
    
    public List<Racun>? Racuni { get; set; }
}