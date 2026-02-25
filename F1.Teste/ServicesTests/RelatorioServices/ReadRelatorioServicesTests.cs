using F1.API.Services.RelatorioServices.Services;
using F1.Lib.Interfaces.Especificas.Query;
using F1.Lib.Modelos;
using F1.Teste.Bogus;
using Moq;
using System.Linq.Expressions;

namespace F1.Teste.ServicesTests.RelatorioServices;

public class ReadRelatorioServicesTests
{
    [Fact]
    public async Task LerRelatorioGeralAsync_DeveRetornarRelatorioDTO_QuandoGerarComSucesso()
    {
        var mockGpQuery = new Mock<IGrandePremioQuery>();
        var mockPilotoQuery = new Mock<IPilotoQuery>();
        var mockEquipeQuery = new Mock<IEquipeQuery>();

        var pilotoVencedor = PilotoFaker.Gerar().Generate();
        pilotoVencedor.GpsVencidos = new List<GrandePremio> { new(), new(), new() };

        var equipeVencedora = EquipeFaker.Gerar().Generate();

        mockGpQuery.Setup(q => q.ContarAsync()).ReturnsAsync(100);
        mockPilotoQuery.Setup(q => q.ContarAsync()).ReturnsAsync(20);
        mockEquipeQuery.Setup(q => q.ContarAsync()).ReturnsAsync(10);
        mockPilotoQuery.Setup(q => q.ObterMediaIdade()).ReturnsAsync(25.567);
        mockPilotoQuery
            .Setup(q => q.ObterEntidadeComMaisVitoriasAsync(It.IsAny<Expression<Func<Piloto, int>>>()))
            .ReturnsAsync(pilotoVencedor);
        mockEquipeQuery
            .Setup(q => q.ObterEntidadeComMaisVitoriasAsync(It.IsAny<Expression<Func<Equipe, int>>>()))
            .ReturnsAsync(equipeVencedora);

        var service = new ReadRelatorioService(mockPilotoQuery.Object, mockEquipeQuery.Object, mockGpQuery.Object);

        var resultado = await service.LerRelatorioGeralAsync();

        Assert.NotNull(resultado);
        Assert.Equal(100, resultado.TotalGps);
        Assert.Equal(20, resultado.TotalPilotos);
        Assert.Equal(10, resultado.TotalEquipes);
        Assert.Equal(25.6, resultado.MediaIdade);
        Assert.Equal(pilotoVencedor.Nome, resultado.PilotoMaisVencedor);
        Assert.Equal(equipeVencedora.Nome, resultado.EquipeMaisVencedora);
    }

    [Fact]
    public async Task LerRelatorioGeralAsync_DeveRetornarNA_QuandoNaoHouverVencedores()
    {
        var mockGpQuery = new Mock<IGrandePremioQuery>();
        var mockPilotoQuery = new Mock<IPilotoQuery>();
        var mockEquipeQuery = new Mock<IEquipeQuery>();

        mockPilotoQuery
            .Setup(q => q.ObterEntidadeComMaisVitoriasAsync(It.IsAny<Expression<Func<Piloto, int>>>()))
            .ReturnsAsync((Piloto?)null);
        mockEquipeQuery
            .Setup(q => q.ObterEntidadeComMaisVitoriasAsync(It.IsAny<Expression<Func<Equipe, int>>>()))
            .ReturnsAsync((Equipe?)null);

        var service = new ReadRelatorioService(mockPilotoQuery.Object, mockEquipeQuery.Object, mockGpQuery.Object);

        var resultado = await service.LerRelatorioGeralAsync();

        Assert.Equal("N/A", resultado.PilotoMaisVencedor);
        Assert.Equal("N/A", resultado.EquipeMaisVencedora);
    }
}
