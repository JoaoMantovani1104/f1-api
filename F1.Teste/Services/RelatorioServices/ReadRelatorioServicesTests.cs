using Moq;
using F1.Lib.Modelos;
using F1.API.Services.Relatorio.Services;
using F1.Lib.Interfaces.Especificas.Query;

namespace F1.Teste.Services.RelatorioServices;

public class ReadRelatorioServicesTests
{
    [Fact]
    public async Task LerRelatorioGeralAsync_DeveRetornarRelatorioDTO_QuandoDadosValidos()
    {
        var mockGpQuery = new Mock<IGrandePremioQuery>();
        var mockPilotoQuery = new Mock<IPilotoQuery>();
        var mockEquipeQuery = new Mock<IEquipeQuery>();

        var pilotoVencedor = new Piloto
        {
            Nome = "Senna",
            GpsVencidos = new List<GrandePremio> { new(), new(), new() } 
        };

        var equipeVencedora = new Equipe { Nome = "McLaren" };

        mockGpQuery.Setup(q => q.ContarAsync()).ReturnsAsync(100);
        mockPilotoQuery.Setup(q => q.ContarAsync()).ReturnsAsync(20);
        mockEquipeQuery.Setup(q => q.ContarAsync()).ReturnsAsync(10);

        mockPilotoQuery
            .Setup(q => q.ObterMediaIdade())
            .ReturnsAsync(25.567);

        mockPilotoQuery
            .Setup(q => q.ObterPilotoComMaisVitoriasAsync())
            .ReturnsAsync(pilotoVencedor);

        mockEquipeQuery
            .Setup(q => q.ObterEquipeComMaisVitoriasAsync())
            .ReturnsAsync(equipeVencedora);

        var service = new ReadRelatorioService(mockPilotoQuery.Object, mockEquipeQuery.Object, mockGpQuery.Object);

        var resultado = await service.LerRelatorioGeralAsync();

        Assert.NotNull(resultado);
        Assert.Equal(100, resultado.TotalGps);
        Assert.Equal(20, resultado.TotalPilotos);
        Assert.Equal(10, resultado.TotalEquipes);

        Assert.Equal(25.6, resultado.MediaIdade);

        Assert.Equal("Senna com 3 vitórias!", resultado.PilotoMaisVencedor);
        Assert.Equal("McLaren", resultado.EquipeMaisVencedora);
    }

    [Fact]
    public async Task LerRelatorioGeralAsync_DeveRetornarNA_QuandoNaoHouverVencedores()
    {
        var mockGpQuery = new Mock<IGrandePremioQuery>();
        var mockPilotoQuery = new Mock<IPilotoQuery>();
        var mockEquipeQuery = new Mock<IEquipeQuery>();

        mockPilotoQuery
            .Setup(q => q.ObterPilotoComMaisVitoriasAsync())
            .ReturnsAsync((Piloto?)null);

        mockEquipeQuery
            .Setup(q => q.ObterEquipeComMaisVitoriasAsync())
            .ReturnsAsync((Equipe?)null);

        var service = new ReadRelatorioService(mockPilotoQuery.Object, mockEquipeQuery.Object, mockGpQuery.Object);

        var resultado = await service.LerRelatorioGeralAsync();

        Assert.Equal("N/A", resultado.PilotoMaisVencedor);
        Assert.Equal("N/A", resultado.EquipeMaisVencedora);
    }
}
