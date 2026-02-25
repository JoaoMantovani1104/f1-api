using Moq;
using F1.Lib.Modelos;
using F1.Teste.Bogus;
using System.Linq.Expressions;
using F1.API.Data.Dtos.GrandePremioDTO;

namespace F1.Teste.ServicesTests.GPServices;

public class CreateGPServiceTests : BaseGPServiceTests
{
    [Fact]
    public async Task AdicionarGPAsync_DeveLancarInvalidOperationException_QuandoGPJaExistente()
    {
        var gpJaExistente = GPFaker.Gerar().Generate();

        var createGpDTO = GrandePremioDTOFaker.GerarCreate().Generate();
        createGpDTO.Nome = gpJaExistente.Nome;

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync(gpJaExistente);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => createService.AdicionarGPAsync(createGpDTO));

        Assert.Equal("Grande Prêmio já existente.", resultado.Message);

        mockRepo.Verify(repo => repo.AdicionarAsync(gpJaExistente), Times.Never);
        mockMapper.Verify(mapper => mapper.Map<GrandePremio>(It.IsAny<CreateGpDTO>()), Times.Never);
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task AdicionarGPAsync_DeveLancarException_QuandoBancoFalharAoSalvar()
    {
        var gpCreateMapeado = GPFaker.Gerar().Generate();

        var createGpDTO = GrandePremioDTOFaker.GerarCreate().Generate();
        createGpDTO.Nome = gpCreateMapeado.Nome;

        mockMapper
            .Setup(mapper => mapper.Map<GrandePremio>(createGpDTO))
            .Returns(gpCreateMapeado);

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((GrandePremio?)null);

        mockUow.Setup(uow => uow.CommitAsync()).ReturnsAsync(false);

        var resultado = await Assert.ThrowsAsync<Exception>( 
            () => createService.AdicionarGPAsync(createGpDTO));

        mockRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<GrandePremio>()), Times.Once());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task AdicionarGPAsync_DeveLancarInvalidOperationException_QuandoNaoEncontrarPilotoVencedor()
    {
        var createGpDTO = GrandePremioDTOFaker.GerarCreate().Generate();
        createGpDTO.VencedorId = -1;

        mockPilotoQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync((Piloto?)null);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => createService.AdicionarGPAsync(createGpDTO));

        Assert.Equal($"O Piloto vencedor com ID {createGpDTO.VencedorId} não foi encontrado.", resultado.Message);

        mockRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<GrandePremio>()), Times.Never());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task AdicionarGPAsync_DeveRetornarReadGpDTO_QuandoGPValidoEComVencedorExistente()
    {
        var gpCreateMapeado = GPFaker.Gerar().Generate();
        var createGpDTO = GrandePremioDTOFaker.GerarCreate().Generate();
        createGpDTO.Nome = gpCreateMapeado.Nome;

        createGpDTO.VencedorId = 1;

        var vencedor = PilotoFaker.Gerar().Generate();
        vencedor.Id = 1;

        var readGpDTO = GrandePremioDTOFaker.GerarRead().Generate();
        readGpDTO.Id = gpCreateMapeado.Id;
        readGpDTO.Nome = gpCreateMapeado.Nome;

        mockPilotoQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync(vencedor);

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((GrandePremio?)null);

        mockMapper
            .Setup(mapper => mapper.Map<GrandePremio>(createGpDTO))
            .Returns(gpCreateMapeado);

        mockMapper
            .Setup(mapper => mapper.Map<ReadGpDTO>(gpCreateMapeado))
            .Returns(readGpDTO);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(true);

        var resultado = await createService.AdicionarGPAsync(createGpDTO);

        Assert.NotNull(resultado);
        Assert.Equal(readGpDTO.Id, resultado.Id);

        mockPilotoQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once);
    }
}