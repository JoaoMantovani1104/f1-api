namespace F1.API.Data.Dtos.PilotoDTO;

public class ReadPilotoDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Numero { get; set; }
    public int Idade { get; set; }
    public string Nacionalidade { get; set; }
    public int EquipeId { get; set; }
    public string NomeEquipe { get; set; }
    public List<string> GpsVencidos { get; set; }
}
