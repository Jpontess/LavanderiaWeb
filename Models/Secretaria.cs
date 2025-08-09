using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebLavApp.Models;

public class Secretaria
{
     public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Nome { get; set; }

    [Required ]
    [MaxLength(100)]
    public string? Secretarias { get; set; }

    [Required]
    public string? Material { get; set; }

    public string? Cor { get; set; }

    [NotNull]
    public int Quantidade { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Data { get; set; } = DateTime.Today;
}