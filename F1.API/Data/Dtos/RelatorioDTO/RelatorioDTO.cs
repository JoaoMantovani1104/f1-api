namespace F1.API.Data.Dtos.RelatorioDTO;

public class RelatorioDTO
{
    public int TotalPilotos { get; set; }
    public int TotalEquipes { get; set; }
    public int TotalGps { get; set; }
    public double MediaIdade { get; set; }
    public string PilotoMaisVencedor { get; set; }
    public string EquipeMaisVencedora { get; set; }
}
