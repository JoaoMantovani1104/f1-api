using Moq;
using AutoMapper;
using F1.Lib.Modelos;
using System.Linq.Expressions;
using F1.API.Data.Dtos.GrandePremioDTO;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.GpServices.Services;

namespace F1.Teste.Services.GPServices;

public class ReadGPServiceTests
{
    [Fact]
    public async Task LerGPPorIdAsync_DeveRetornarNull_QuandoNaoEncontrarGP()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IGrandePremioQuery>();

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((GrandePremio?)null);

        var service = new ReadGrandePremioService(mockMapper.Object, mockQuery.Object);

        var resultado = await service.LerGPPorIdAsync(99);

        Assert.Null(resultado);
        mockMapper.Verify(mapper => mapper.Map<ReadGpDTO>(It.IsAny<GrandePremio>()), Times.Never());
    }

    [Fact]
    public async Task LerGPPorIdAsync_DeveRetornarReadGpDTO_QuandoEncontrarGP()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IGrandePremioQuery>();

        var gpBuscado = new GrandePremio { Id = 1, Nome = "GP Teste" };
        var gpBuscadoMapeado = new ReadGpDTO { Id = 1, Nome = "GP Teste" };

        var listaGps = new List<GrandePremio> { gpBuscado };

        mockMapper
            .Setup(mapper => mapper.Map<ReadGpDTO>(gpBuscado))
            .Returns(gpBuscadoMapeado);

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

        var service = new ReadGrandePremioService(mockMapper.Object, mockQuery.Object);

        var resultado = await service.LerGPPorIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);

        mockMapper.Verify(mapper => mapper.Map<ReadGpDTO>(gpBuscado), Times.Once());
        mockQuery.Verify(query => query.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<GrandePremio, bool>>>(),
            It.IsAny<Expression<Func<GrandePremio, object?>>[]>()),
            Times.Once());
    }

    [Fact]
    public async Task LerGPsAsync_DeveRetornarListaVazia_QuandoNaoExistirGPCadastrado()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IGrandePremioQuery>();

        var listaGps = new List<GrandePremio>();
        var listaGpsMapeados = new List<ReadGpDTO>();

        mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ReadGpDTO>>(listaGps))
            .Returns(listaGpsMapeados);

        mockQuery
            .Setup(query => query.ObterTodosAsync(It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync(listaGps);

        var service = new ReadGrandePremioService(mockMapper.Object, mockQuery.Object);

        var resultado = await service.LerGPsAsync();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);

        mockMapper.Verify(mapper => mapper.Map<IEnumerable<ReadGpDTO>>(listaGps), Times.Once());
        mockQuery.Verify(query => query.ObterTodosAsync(It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Once());
    }

    [Fact]
    public async Task LerGPsAsync_DeveRetornarListaReadGpDTO_QuandoExistirAoMenosUmGPCadastrado()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IGrandePremioQuery>();

        var gp = new GrandePremio { Id = 1, Nome = "GP Teste" };
        var gpDTO = new ReadGpDTO { Id = 1, Nome = "GP Teste" };

        var listaGps = new List<GrandePremio> { gp };
        var listaGpsMapeados = new List<ReadGpDTO> { gpDTO };

        mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ReadGpDTO>>(listaGps))
            .Returns(listaGpsMapeados);

        mockQuery
            .Setup(query => query.ObterTodosAsync(It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync(listaGps);

        var service = new ReadGrandePremioService(mockMapper.Object, mockQuery.Object);

        var resultado = await service.LerGPsAsync();

        Assert.NotNull(resultado);
        Assert.NotEmpty(resultado);
        Assert.Single(resultado);

        mockMapper.Verify(mapper => mapper.Map<IEnumerable<ReadGpDTO>>(listaGps), Times.Once());
        mockQuery.Verify(query => query.ObterTodosAsync(It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Once());
    }
}
