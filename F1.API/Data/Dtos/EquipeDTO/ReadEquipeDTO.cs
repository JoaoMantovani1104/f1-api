using F1.Lib.Modelos;

namespace F1.API.Data.Dtos.EquipeDTO;

public class ReadEquipeDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public ICollection<string> Pilotos { get; set; }
}
