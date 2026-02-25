using System.ComponentModel.DataAnnotations;

namespace F1.API.Data.Dtos.EquipeDTO;

public class UpdateEquipeDTO
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; }
}
