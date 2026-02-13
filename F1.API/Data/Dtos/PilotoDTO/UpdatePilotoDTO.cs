using System.ComponentModel.DataAnnotations;

namespace F1.API.Data.Dtos.PilotoDTO;

public class UpdatePilotoDTO
{
    [Required]
    public string Nome { get; set; }

    [Required]
    public int Numero { get; set; }
    public int Idade { get; set; }

    [Required]
    public string Nacionalidade { get; set; }

    [Required]
    public int EquipeId { get; set; }
}
