using Moq;
using F1.Lib.Modelos;
using System.Linq.Expressions;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.GpServices.Services;

namespace F1.Teste.Services.GPServices;

public class DeleteGPServiceTests
{
    [Fact]
    public async Task DeletarGPAsync_DeveRetornarFalse_QuandoNaoEncontrarGP()
    {
        var mockQuery = new Mock<IGrandePremioQuery>();
        var mockRepo = new Mock<IRepositoryBase<GrandePremio>>();

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((GrandePremio?)null);

        var service = new DeleteGrandePremioService(mockQuery.Object, mockRepo.Object);

        var resultado = await service.DeletarGPAsync(1);

        Assert.False(resultado);

        mockQuery.Verify(query => query.BuscarPorCampoAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.DeletarAsync(It.IsAny<GrandePremio>()), Times.Never());
    }

    [Fact]
    public async Task DeletarGPAsync_DeveLancarInvalidOperationException_QuandoGPTerVencedorAssociado()
    {
        var mockQuery = new Mock<IGrandePremioQuery>();
        var mockRepo = new Mock<IRepositoryBase<GrandePremio>>();

        var vencedor = new Piloto { Nome = "Vencedor" };
        var gpComVencedor = new GrandePremio { Id = 1 , Nome = "GP Teste", Vencedor = vencedor };

        var listaGps = new List<GrandePremio> { gpComVencedor };

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((Expression<Func<GrandePremio, bool>> expression,
                Expression<Func<GrandePremio, object?>>[] includes) =>
            {
                var func = expression.Compile();
                return listaGps.FirstOrDefault(func);
            });

        var service = new DeleteGrandePremioService(mockQuery.Object, mockRepo.Object);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.DeletarGPAsync(1));

        Assert.Equal("Não é possível deletar Grandes Prêmios com vencedores associados.", resultado.Message);

        mockQuery.Verify(query => query.BuscarPorCampoAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.DeletarAsync(It.IsAny<GrandePremio>()), Times.Never());
    }

    [Fact]
    public async Task DeletarGPAsync_DeveRetornarFalse_QuandoDelecaoFalhar()
    {
        var mockQuery = new Mock<IGrandePremioQuery>();
        var mockRepo = new Mock<IRepositoryBase<GrandePremio>>();

        var gp = new GrandePremio { Id = 1, Nome = "GP Teste" , Vencedor = null };

        var listaGps = new List<GrandePremio> { gp };

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((Expression<Func<GrandePremio, bool>> expression,
                Expression<Func<GrandePremio, object?>>[] includes) =>
            {
                var func = expression.Compile();
                return listaGps.FirstOrDefault(func);
            });

        mockRepo
            .Setup(repo => repo.DeletarAsync(gp))
            .ReturnsAsync(false);

        var service = new DeleteGrandePremioService(mockQuery.Object, mockRepo.Object);

        var resultado = await service.DeletarGPAsync(1);

        Assert.False(resultado);

        mockQuery.Verify(query => query.BuscarPorCampoAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.DeletarAsync(It.IsAny<GrandePremio>()), Times.Once());
    }

    [Fact]
    public async Task DeletarGPAsync_DeveRetornarTrue_QuandoDelecaoSucesso()
    {
        var mockQuery = new Mock<IGrandePremioQuery>();
        var mockRepo = new Mock<IRepositoryBase<GrandePremio>>();

        var gp = new GrandePremio { Id = 1, Nome = "GP Teste", Vencedor = null };

        var listaGps = new List<GrandePremio> { gp };

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((Expression<Func<GrandePremio, bool>> expression,
                Expression<Func<GrandePremio, object?>>[] includes) =>
            {
                var func = expression.Compile();
                return listaGps.FirstOrDefault(func);
            });

        mockRepo
            .Setup(repo => repo.DeletarAsync(gp))
            .ReturnsAsync(true);

        var service = new DeleteGrandePremioService(mockQuery.Object, mockRepo.Object);

        var resultado = await service.DeletarGPAsync(1);

        Assert.True(resultado);

        mockQuery.Verify(query => query.BuscarPorCampoAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.DeletarAsync(It.IsAny<GrandePremio>()), Times.Once());
    }
}
