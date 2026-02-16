using Moq;
using F1.Lib.Modelos;
using System.Linq.Expressions;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.PilotoServices.Services;

namespace F1.Teste.Services.PilotoService;

public class DeletePilotoServiceTests
{
    [Fact]
    public async Task DeletarPilotoAsync_DeveRetornarFalse_QuandoNaoEncontrarPiloto()
    {
        var mockQuery = new Mock<IPilotoQuery>();
        var mockRepo = new Mock<IRepositoryBase<Piloto>>();

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync((Piloto?)null);

        var service = new DeletePilotoService(mockQuery.Object, mockRepo.Object);

        var resultado = await service.DeletarPilotoAsync(1);

        Assert.False(resultado);
        mockQuery.Verify(query => query.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.DeletarAsync(It.IsAny<Piloto>()), Times.Never());
    }

    [Fact]
    public async Task DeletarPilotoAsync_DeveRetornarFalse_QuandoDelecaoFalha()
    {
        var mockQuery = new Mock<IPilotoQuery>();
        var mockRepo = new Mock<IRepositoryBase<Piloto>>();

        var pilotoADeletar = new Piloto { Id = 1, Nome = "Piloto Teste" };

        var listaPilotos = new List<Piloto> { pilotoADeletar };

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync((Expression<Func<Piloto, bool>> expression,
                           Expression<Func<Piloto, object?>>[] includes) =>
            {
                var func = expression.Compile();
                return listaPilotos.FirstOrDefault(func);
            });

        mockRepo
            .Setup(repo => repo.DeletarAsync(pilotoADeletar))
            .ReturnsAsync(false);

        var service = new DeletePilotoService(mockQuery.Object, mockRepo.Object);

        var resultado = await service.DeletarPilotoAsync(1);

        Assert.False(resultado);

        mockQuery.Verify(query => query.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.DeletarAsync(It.IsAny<Piloto>()), Times.Once());
    }

    [Fact]
    public async Task DeletarPilotoAsync_DeveRetornarTrue_QuandoDelecaoFalha()
    {
        var mockQuery = new Mock<IPilotoQuery>();
        var mockRepo = new Mock<IRepositoryBase<Piloto>>();

        var pilotoADeletar = new Piloto { Id = 1, Nome = "Piloto Teste" };

        var listaPilotos = new List<Piloto> { pilotoADeletar };

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync((Expression<Func<Piloto, bool>> expression,
                           Expression<Func<Piloto, object?>>[] includes) =>
            {
                var func = expression.Compile();
                return listaPilotos.FirstOrDefault(func);
            });

        mockRepo
            .Setup(repo => repo.DeletarAsync(pilotoADeletar))
            .ReturnsAsync(true);

        var service = new DeletePilotoService(mockQuery.Object, mockRepo.Object);

        var resultado = await service.DeletarPilotoAsync(1);

        Assert.True(resultado);

        mockQuery.Verify(query => query.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.DeletarAsync(It.IsAny<Piloto>()), Times.Once());
    }
}
