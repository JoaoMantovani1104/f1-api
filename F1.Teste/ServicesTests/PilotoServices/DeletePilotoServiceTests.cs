using Moq;
using F1.Lib.Modelos;
using F1.Teste.Bogus;
using System.Linq.Expressions;

namespace F1.Teste.ServicesTests.PilotoServices;

public class DeletePilotoServiceTests : BasePilotoServiceTests
{
    [Fact]
    public async Task DeletarPilotoAsync_DeveRetornarFalse_QuandoNaoEncontrarPiloto()
    {
        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync((Piloto?)null);

        var resultado = await deleteService.DeletarPilotoAsync(1);

        Assert.False(resultado);
        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.Deletar(It.IsAny<Piloto>()), Times.Never());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task DeletarPilotoAsync_DeveRetornarFalse_QuandoDelecaoFalhar()
    {
        var pilotoADeletar = PilotoFaker.Gerar().Generate();

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync(pilotoADeletar);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(false);

        var resultado = await deleteService.DeletarPilotoAsync(pilotoADeletar.Id);

        Assert.False(resultado);

        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.Deletar(It.IsAny<Piloto>()), Times.Once());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task DeletarPilotoAsync_DeveRetornarTrue_QuandoDeletarComSucesso()
    {
        var pilotoADeletar = PilotoFaker.Gerar().Generate();

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync(pilotoADeletar);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(true);

        var resultado = await deleteService.DeletarPilotoAsync(pilotoADeletar.Id);

        Assert.True(resultado);

        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.Deletar(It.IsAny<Piloto>()), Times.Once());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }
}
