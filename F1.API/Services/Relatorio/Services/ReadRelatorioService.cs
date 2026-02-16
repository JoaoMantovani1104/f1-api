using F1.API.Data.Dtos.RelatorioDTO;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.Relatorio.Interfaces;

namespace F1.API.Services.Relatorio.Services;

public class ReadRelatorioService : IReadRelatorioService
{
    private readonly IPilotoQuery pilotoQuery;
    private readonly IEquipeQuery equipeQuery;
    private readonly IGrandePremioQuery grandePremioQuery;

    public ReadRelatorioService(IPilotoQuery pilotoQuery,
        IEquipeQuery equipeQuery, IGrandePremioQuery grandePremioQuery)
    {
        this.pilotoQuery = pilotoQuery;
        this.equipeQuery = equipeQuery;
        this.grandePremioQuery = grandePremioQuery;
    }

    public async Task<RelatorioDTO> LerRelatorioGeralAsync()
    {
        var totalGrandesPremios = await grandePremioQuery.ContarAsync();
        var totalPilotos = await pilotoQuery.ContarAsync();
        var totalEquipes = await equipeQuery.ContarAsync();

        var mediaIdade = await pilotoQuery.ObterMediaIdade();

        var pilotoVencedor = await pilotoQuery.ObterPilotoComMaisVitoriasAsync();
        var equipeVencedora = await equipeQuery.ObterEquipeComMaisVitoriasAsync();

        return new RelatorioDTO
        {
            TotalPilotos = totalPilotos,
            TotalEquipes = totalEquipes,
            TotalGps = totalGrandesPremios,
            MediaIdade = Math.Round(mediaIdade, 1),
            PilotoMaisVencedor = pilotoVencedor != null ?
                                 $"{pilotoVencedor.Nome} com {pilotoVencedor.GpsVencidos.Count} vitórias!"
                                 : "N/A",
            EquipeMaisVencedora = equipeVencedora?.Nome ?? "N/A"
        };
    }
}