
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLavApp.Models;

public class Fechamento
{
    [Key]
    public int Id { get; set; }  
    
    public int Ano { get; set; }
    public int Mes { get; set; }
    
    [NotMapped]
    public string? Secretaria { get; set; }

    [NotMapped]
    public string? Modalidade { get; set; }

    [NotMapped]
    public string? Internos { get; set; }
    
    public int TotalServicos { get; set; }
    public int ClientesAtendidos { get; set; }
    public int ItensProcessados { get; set; } 
}