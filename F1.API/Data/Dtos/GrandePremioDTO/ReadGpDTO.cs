namespace F1.API.Data.Dtos.GrandePremioDTO;

public class ReadGpDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Localizacao { get; set; }
    public int Voltas { get; set; }
    public int Ordem { get; set; }
    public int? VencedorId { get; set; }
    public string Vencedor { get; set; }
}
