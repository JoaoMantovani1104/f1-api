using Moq;
using F1.Lib.Modelos;
using System.Linq.Expressions;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.EquipeServices.Services;

namespace F1.Teste.Services.EquipeServices;

public class DeleteEquipeServiceTests
{
    [Fact]
    public async Task DeletarEquipeAsync_DeveRetornarFalse_QuandoNaoEncontrarEquipeParaDeletar()
    {
        var mockQuery = new Mock<IEquipeQuery>();
        var mockRepo = new Mock<IRepositoryBase<Equipe>>();

        mockQuery
            .Setup(q => q.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Equipe?)null);

        var service = new DeleteEquipeService(mockQuery.Object, mockRepo.Object);

        var resultado = await service.DeletarEquipeAsync(1);

        Assert.False(resultado);
        mockQuery.Verify(q => q.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), 
            Times.Once);
    }

    [Fact]
    public async Task DeletarEquipeAsync_DeveRetornarFalse_QuandoIdForDiferente()
    {
        var mockQuery = new Mock<IEquipeQuery>();
        var mockRepo = new Mock<IRepositoryBase<Equipe>>();
        var idPesquisado = 99;
        var listaEquipes = new List<Equipe> 
        {
            new() { Id = 1, Nome = "Ferrari" }
        };

        mockQuery
            .Setup(q => q.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(), 
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Expression<Func<Equipe, bool>> expression,
                           Expression<Func<Equipe, object?>>[] includes) =>
            {
                var func = expression.Compile();
                return listaEquipes.FirstOrDefault(func);
            });

        var service = new DeleteEquipeService(mockQuery.Object, mockRepo.Object);

        var resultado = await service.DeletarEquipeAsync(idPesquisado);

        Assert.False(resultado);
        mockQuery.Verify(q => q.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), 
            Times.Once);
    }

    [Fact]
    public async Task DeletarEquipeAsync_DeveLancarInvalidOperationException_QuandoEquipePossuirPilotos()
    {
        var mockQuery = new Mock<IEquipeQuery>();
        var mockRepo = new Mock<IRepositoryBase<Equipe>>();

        var equipeADeletar = new Equipe { Id = 1, Nome = "Equipe Teste"};
        var equipeComPiloto = new Equipe
        {
            Id = 1,
            Nome = "Equipe Teste",
            Pilotos = new List<Piloto> { new Piloto { Id = 1, Nome = "Piloto Teste" } }
        };
        var listaEquipes = new List<Equipe> { equipeComPiloto };

        mockQuery
            .Setup(q => q.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Expression<Func<Equipe, bool>> expression, 
                            Expression<Func<Equipe, object?>>[] includes) =>
            {
                var func = expression.Compile();
                return listaEquipes.FirstOrDefault(func);
            });

        var service = new DeleteEquipeService(mockQuery.Object, mockRepo.Object);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.DeletarEquipeAsync(1));


        Assert.Equal("Não é possível deletar uma equipe que possui pilotos associados.", resultado.Message);
        mockQuery.Verify(q => q.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), 
            Times.Exactly(2));
    }

    [Fact]
    public async Task DeletarEquipeAsync_DeveRetornarFalse_QuandoDelecaoFalhar()
    {
        var mockQuery = new Mock<IEquipeQuery>();
        var mockRepo = new Mock<IRepositoryBase<Equipe>>();

        var equipeADeletar = new Equipe { Id = 1, Nome = "Equipe Teste" };

        var listaEquipes = new List<Equipe> { equipeADeletar };

        mockQuery
            .Setup(q => q.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Expression<Func<Equipe, bool>> expression,
                            Expression<Func<Equipe, object?>>[] includes) =>
            {
                var func = expression.Compile();
                return listaEquipes.FirstOrDefault(func);
            });

        mockRepo
            .Setup(r => r.DeletarAsync(It.IsAny<Equipe>()))
            .ReturnsAsync(false);

        var service = new DeleteEquipeService(mockQuery.Object, mockRepo.Object);

        var resultado = await service.DeletarEquipeAsync(1);

        Assert.False(resultado);
        mockQuery.Verify(q => q.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), 
            Times.Exactly(2));
        mockRepo.Verify(r => r.DeletarAsync(It.IsAny<Equipe>()), Times.Once);
    }

    [Fact]
    public async Task DeletarEquipeAsync_DeveRetornarTrue_QuandoDelecaoSucesso()
    {
        var mockQuery = new Mock<IEquipeQuery>();
        var mockRepo = new Mock<IRepositoryBase<Equipe>>();

        var equipeADeletar = new Equipe { Id = 1, Nome = "Equipe Teste" };
        var listaEquipes = new List<Equipe> { equipeADeletar };

        mockQuery
            .Setup(q => q.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Expression<Func<Equipe, bool>> expression,
                            Expression<Func<Equipe, object?>>[] includes) =>
            {
                var func = expression.Compile();
                return listaEquipes.FirstOrDefault(func);
            });

        mockRepo
            .Setup(r => r.DeletarAsync(It.IsAny<Equipe>()))
            .ReturnsAsync(true);

        var service = new DeleteEquipeService(mockQuery.Object, mockRepo.Object);

        var resultado = await service.DeletarEquipeAsync(1);

        Assert.True(resultado);
        mockQuery.Verify(q => q.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), 
            Times.Exactly(2));
        mockRepo.Verify(r => r.DeletarAsync(It.IsAny<Equipe>()), Times.Once);
    }
}
