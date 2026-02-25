using Moq;
using F1.Lib.Modelos;
using F1.Teste.Bogus;
using System.Linq.Expressions;

namespace F1.Teste.ServicesTests.GPServices;

public class DeleteGPServiceTests : BaseGPServiceTests
{
    [Fact]
    public async Task DeletarGPAsync_DeveRetornarFalse_QuandoNaoEncontrarGP()
    {
        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((GrandePremio?)null);

        var resultado = await deleteService.DeletarGPAsync(1);

        Assert.False(resultado);

        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.Deletar(It.IsAny<GrandePremio>()), Times.Never());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task DeletarGPAsync_DeveLancarInvalidOperationException_QuandoGPTerVencedorAssociado()
    {

        var vencedor = PilotoFaker.Gerar().Generate();
        var gpComVencedor = GPFaker.Gerar().Generate();
        gpComVencedor.Vencedor = vencedor;

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync(gpComVencedor);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => deleteService.DeletarGPAsync(gpComVencedor.Id));

        Assert.Equal("Não é possível deletar Grandes Prêmios com vencedores associados.", resultado.Message);

        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.Deletar(It.IsAny<GrandePremio>()), Times.Never());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task DeletarGPAsync_DeveRetornarFalse_QuandoDelecaoFalhar()
    {
        var gp = GPFaker.Gerar().Generate();
        gp.Vencedor = null;

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync(gp);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(false);

        var resultado = await deleteService.DeletarGPAsync(1);

        Assert.False(resultado);

        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.Deletar(It.IsAny<GrandePremio>()), Times.Once());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task DeletarGPAsync_DeveRetornarTrue_QuandoDeletarComSucesso()
    {
        var gp = GPFaker.Gerar().Generate();
        gp.Vencedor = null;

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync(gp);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(true);

        var resultado = await deleteService.DeletarGPAsync(1);

        Assert.True(resultado);

        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.Deletar(It.IsAny<GrandePremio>()), Times.Once());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }
}
