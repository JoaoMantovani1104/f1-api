using F1.API.Data.Dtos.RelatorioDTO;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.RelatorioServices.Interfaces;

namespace F1.API.Services.RelatorioServices.Services;

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
        var pilotoVencedor = await pilotoQuery.ObterEntidadeComMaisVitoriasAsync(p => p.GpsVencidos.Count);
        var equipeVencedora = await equipeQuery.ObterEntidadeComMaisVitoriasAsync(e => e.Pilotos.Sum(p => p.GpsVencidos.Count));

        return new RelatorioDTO
        {
            TotalPilotos = totalPilotos,
            TotalEquipes = totalEquipes,
            TotalGps = totalGrandesPremios,
            MediaIdade = Math.Round(mediaIdade, 1),
            PilotoMaisVencedor = pilotoVencedor != null ?
                                 pilotoVencedor.Nome
                                 : "N/A",
            EquipeMaisVencedora = equipeVencedora?.Nome ?? "N/A"
        };
    }
}