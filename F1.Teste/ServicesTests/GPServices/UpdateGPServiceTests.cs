using Moq;
using F1.Lib.Modelos;
using F1.Teste.Bogus;
using System.Linq.Expressions;
using F1.Lib.Interfaces.Especificas.Query;

namespace F1.Teste.ServicesTests.GPServices;


public class UpdateGPServiceTests : BaseGPServiceTests
{
    [Fact]
    public async Task AtualizarGPAsync_DeveLancarInvalidOperationException_QuandoVencedorIDInvalido()
    {
        var gp = GPFaker.Gerar().Generate();
        var gpDTO = GrandePremioDTOFaker.GerarUpdate().Generate();
        gpDTO.VencedorId = -1;

        mockPilotoQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync((Piloto?)null);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => updateService.AtualizarGPAsync(gp.Id, gpDTO));

        Assert.Equal($"O Piloto vencedor com ID {gpDTO.VencedorId} não foi encontrado.", resultado.Message);
        mockRepo.Verify(repo => repo.Atualizar(It.IsAny<GrandePremio>()), Times.Never());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task AtualizarGPAsync_DeveRetornarFalse_QuandoNaoEncontrarGP()
    {
        var gpDTO = GrandePremioDTOFaker.GerarUpdate().Generate();

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((GrandePremio?)null);

        var resultado = await updateService.AtualizarGPAsync(1, gpDTO);

        Assert.False(resultado);

        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Once());

        mockRepo.Verify(repo => repo.Atualizar(It.IsAny<GrandePremio>()), Times.Never());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task AtualizarGPAsync_DeveLancarInvalidOperationException_QuandoJaHouverGPComMesmoNome()
    {
        var gp = GPFaker.Gerar().Generate();
        var gpComMesmoNome = GPFaker.Gerar().Generate();
        var gpDTO = GrandePremioDTOFaker.GerarUpdate().Generate();

        mockQuery
            .SetupSequence(query => query.BuscarPorPropriedadeAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync(gp)
            .ReturnsAsync(gpComMesmoNome);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => updateService.AtualizarGPAsync(1, gpDTO));

        Assert.Equal($"Já existe outro Grande Prêmio cadastrado com o nome '{gpDTO.Nome}'.", resultado.Message);

        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Exactly(2));

        mockRepo.Verify(repo => repo.Atualizar(It.IsAny<GrandePremio>()), Times.Never());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task AtualizarGPAsync_DeveRetornarFalse_QuandoAtualizacaoFalhar()
    {
        var gp = GPFaker.Gerar().Generate();
        var gpDTO = GrandePremioDTOFaker.GerarUpdate().Generate();

        mockMapper
            .Setup(mapper => mapper.Map(gpDTO, gp));

        mockQuery
            .SetupSequence(query => query.BuscarPorPropriedadeAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync(gp)          
            .ReturnsAsync((GrandePremio?)null);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(false);

        var resultado = await updateService.AtualizarGPAsync(gp.Id, gpDTO);

        Assert.False(resultado);

        mockMapper.Verify(mapper => mapper.Map(gpDTO, gp), Times.Once());
        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.Atualizar(It.IsAny<GrandePremio>()), Times.Once());
        
    }

    [Fact]
    public async Task AtualizarGPAsync_DeveRetornarTrue_QuandoAtualizarComSucesso()
    {
        var gp = GPFaker.Gerar().Generate();
        var gpDTO = GrandePremioDTOFaker.GerarUpdate().Generate();

        gpDTO.VencedorId = 1;

        var vencedor = PilotoFaker.Gerar().Generate();
        vencedor.Id = 1;

        mockPilotoQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync(vencedor);

        mockMapper
            .Setup(mapper => mapper.Map(gpDTO, gp));

        mockQuery
            .SetupSequence(query => query.BuscarPorPropriedadeAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync(gp)
            .ReturnsAsync((GrandePremio?)null);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(true);

        var resultado = await updateService.AtualizarGPAsync(gp.Id, gpDTO);

        Assert.True(resultado);

        mockPilotoQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once);

        mockMapper.Verify(mapper => mapper.Map(gpDTO, gp), Times.Once());

        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Exactly(2));

        mockRepo.Verify(repo => repo.Atualizar(It.IsAny<GrandePremio>()), Times.Once());

        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }
}
