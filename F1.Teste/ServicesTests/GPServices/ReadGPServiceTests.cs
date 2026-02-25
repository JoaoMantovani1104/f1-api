using Moq;
using F1.Lib.Modelos;
using F1.Teste.Bogus;
using System.Linq.Expressions;
using F1.API.Data.Dtos.GrandePremioDTO;

namespace F1.Teste.ServicesTests.GPServices;

public class ReadGPServiceTests : BaseGPServiceTests
{
    [Fact]
    public async Task LerGPPorIdAsync_DeveRetornarNull_QuandoNaoEncontrarGP()
    {
        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((GrandePremio?)null);

        var resultado = await readService.LerGPPorIdAsync(99);

        Assert.Null(resultado);
        mockMapper.Verify(mapper => mapper.Map<ReadGpDTO>(It.IsAny<GrandePremio>()), Times.Never());
    }

    [Fact]
    public async Task LerGPPorIdAsync_DeveRetornarReadGpDTO_QuandoEncontrarGP()
    {
        var gpBuscado = GPFaker.Gerar().Generate();

        var gpBuscadoMapeado = GrandePremioDTOFaker.GerarRead().Generate();
        gpBuscadoMapeado.Id = gpBuscado.Id;
        gpBuscadoMapeado.Nome = gpBuscado.Nome;

        mockMapper
            .Setup(mapper => mapper.Map<ReadGpDTO>(It.IsAny<GrandePremio>()))
            .Returns(gpBuscadoMapeado);

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync(gpBuscado);
        
        var resultado = await readService.LerGPPorIdAsync(gpBuscado.Id);

        Assert.NotNull(resultado);
        Assert.Equal(gpBuscadoMapeado.Id, resultado.Id);

        mockMapper.Verify(mapper => mapper.Map<ReadGpDTO>(gpBuscado), Times.Once());
        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<GrandePremio, bool>>>(),
            It.IsAny<Expression<Func<GrandePremio, object?>>[]>()),
            Times.Once());
    }

    [Fact]
    public async Task LerGPsAsync_DeveRetornarListaVazia_QuandoNaoEncontrarGPsCadastrados()
    {
        mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ReadGpDTO>>(It.IsAny<IEnumerable<GrandePremio>>()))
            .Returns(new List<ReadGpDTO>());

        mockQuery
            .Setup(query => query.ObterTodosAsync(It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync(new List<GrandePremio>());

        var resultado = await readService.LerGPsAsync();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);

        mockMapper.Verify(mapper => mapper.Map<IEnumerable<ReadGpDTO>>(It.IsAny<IEnumerable<GrandePremio>>()), Times.Once());
        mockQuery.Verify(query => query.ObterTodosAsync(It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Once());
    }

    [Fact]
    public async Task LerGPsAsync_DeveRetornarListaReadGpDTO_QuandoExistirAoMenosUmGPCadastrado()
    {
        var listaGps = GPFaker.Gerar().Generate(5);
        var listaGpsMapeados = listaGps.Select(gp => new ReadGpDTO 
        { 
            Id = gp.Id, 
            Nome = gp.Nome 
        }).ToList();

        mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ReadGpDTO>>(listaGps))
            .Returns(listaGpsMapeados);

        mockQuery
            .Setup(query => query.ObterTodosAsync(It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync(listaGps);

        var resultado = await readService.LerGPsAsync();

        Assert.NotNull(resultado);
        Assert.NotEmpty(resultado);
        Assert.Equal(5, resultado.Count());

        mockMapper.Verify(mapper => mapper.Map<IEnumerable<ReadGpDTO>>(It.IsAny<IEnumerable<GrandePremio>>()), Times.Once());
        mockQuery.Verify(query => query.ObterTodosAsync(It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Once());
    }
}
