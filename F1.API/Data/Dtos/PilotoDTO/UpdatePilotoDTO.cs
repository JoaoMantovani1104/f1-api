using System.ComponentModel.DataAnnotations;

namespace F1.API.Data.Dtos.PilotoDTO;

public class UpdatePilotoDTO
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    [Required]
    public int Numero { get; set; }
    public int Idade { get; set; }

    [Required]
    [StringLength(50)]
    public string Nacionalidade { get; set; }

    [Required]
    public int EquipeId { get; set; }
}
