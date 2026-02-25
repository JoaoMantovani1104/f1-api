using Moq;
using F1.Lib.Modelos;
using F1.Teste.Bogus;
using System.Linq.Expressions;
using F1.API.Data.Dtos.PilotoDTO;

namespace F1.Teste.ServicesTests.PilotoServices;

public class ReadPilotoServiceTests : BasePilotoServiceTests
{
    [Fact]
    public async Task LerPilotoPorIdAsync_DeveRetornarNull_QuandoNaoEncontrarPiloto()
    {
        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync((Piloto?)null);

        var resultado = await readService.LerPilotoPorIdAsync(99);

        Assert.Null(resultado);
        mockMapper.Verify(mapper => mapper.Map<ReadPilotoDTO>(It.IsAny<Piloto>()), Times.Never());
    }

    [Fact]
    public async Task LerPilotoPorIdAsync_DeveRetornarReadPilotoDTO_QuandoEncontrarPiloto()
    {
        var pilotoBuscado = PilotoFaker.Gerar().Generate();

        var pilotoBuscadoMapeado = PilotoDTOFaker.GerarRead().Generate();
        pilotoBuscadoMapeado.Id = pilotoBuscado.Id;
        pilotoBuscadoMapeado.Nome = pilotoBuscado.Nome;

        mockMapper
            .Setup(mapper => mapper.Map<ReadPilotoDTO>(pilotoBuscado))
            .Returns(pilotoBuscadoMapeado);

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync(pilotoBuscado);

        var resultado = await readService.LerPilotoPorIdAsync(pilotoBuscado.Id);

        Assert.NotNull(resultado);
        Assert.Equal(pilotoBuscado.Id, resultado.Id);

        mockMapper.Verify(mapper => mapper.Map<ReadPilotoDTO>(pilotoBuscado), Times.Once());
        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
    }

    [Fact]
    public async Task LerPilotosAsync_DeveRetornarListaVazia_QuandoNaoEncontrarPilotosCadastrados()
    {
        mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ReadPilotoDTO>>(It.IsAny<IEnumerable<Piloto>>()))
            .Returns(new List<ReadPilotoDTO>());

        mockQuery
            .Setup(query => query.ObterTodosAsync(It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync(new List<Piloto>());

        var resultado = await readService.LerPilotosAsync();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);

        mockMapper.Verify(mapper => mapper.Map<IEnumerable<ReadPilotoDTO>>(
            It.IsAny<IEnumerable<Piloto>>()), Times.Once());
        mockQuery.Verify(query => query.ObterTodosAsync(
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
    }

    [Fact]
    public async Task LerPilotosAsync_DeveRetornarListaReadPilotoDTO_QuandoEncontrarPilotosCadastrados()
    {
        var listaPilotos = PilotoFaker.Gerar().Generate(5);
        var listaPilotosMapeados = listaPilotos.Select(p => new ReadPilotoDTO 
        { 
            Id = p.Id, 
            Nome = p.Nome 
        }).ToList();

        mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ReadPilotoDTO>>(listaPilotos))
            .Returns(listaPilotosMapeados);

        mockQuery
            .Setup(query => query.ObterTodosAsync(It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync(listaPilotos);

        var resultado = await readService.LerPilotosAsync();

        Assert.NotNull(resultado);
        Assert.NotEmpty(resultado);
        Assert.Equal(5, resultado.Count());

        mockMapper.Verify(mapper => mapper.Map<IEnumerable<ReadPilotoDTO>>(listaPilotos), Times.Once());
        mockQuery.Verify(query => query.ObterTodosAsync(
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
    }
}
