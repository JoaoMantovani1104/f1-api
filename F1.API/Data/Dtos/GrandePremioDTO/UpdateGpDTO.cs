using System.ComponentModel.DataAnnotations;

namespace F1.API.Data.Dtos.GrandePremioDTO;

public class UpdateGpDTO
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    [Required]
    [StringLength(50)]
    public string Localizacao { get; set; }

    [Required]
    public int Voltas { get; set; }

    public int Ordem { get; set; }

    public int? VencedorId { get; set; }
}
