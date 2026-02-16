using AutoMapper;
using F1.API.Data.Dtos.GrandePremioDTO;
using F1.API.Data.Dtos.PilotoDTO;
using F1.API.Services.PilotoServices.Services;
using F1.Lib.Interfaces.Especificas.Query;
using F1.Lib.Modelos;
using Moq;
using System.Linq.Expressions;

namespace F1.Teste.Services.PilotoService;

public class ReadPilotoServiceTests
{
    [Fact]
    public async Task LerPilotoPorIdAsync_DeveRetornarNull_QuandoNaoEncontrarPiloto()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IPilotoQuery>();

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync((Piloto?)null);

        var service = new ReadPilotoService(mockMapper.Object, mockQuery.Object);

        var resultado = await service.LerPilotoPorIdAsync(99);

        Assert.Null(resultado);
        mockMapper.Verify(mapper => mapper.Map<ReadPilotoDTO>(It.IsAny<Piloto>()), Times.Never());
    }

    [Fact]
    public async Task LerPilotosPorIdAsync_DeveRetornarReadPilotoDTO_QuandoEncontrarPiloto()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IPilotoQuery>();

        var pilotoBuscado = new Piloto { Id = 1, Nome = "Piloto Teste" };
        var pilotoBuscadoMapeado = new ReadPilotoDTO { Id = 1, Nome = "Piloto Teste" };

        var listaPilotos = new List<Piloto> { pilotoBuscado };

        mockMapper
            .Setup(mapper => mapper.Map<ReadPilotoDTO>(pilotoBuscado))
            .Returns(pilotoBuscadoMapeado);

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

        var service = new ReadPilotoService(mockMapper.Object, mockQuery.Object);

        var resultado = await service.LerPilotoPorIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);

        mockMapper.Verify(mapper => mapper.Map<ReadPilotoDTO>(It.IsAny<Piloto>()), Times.Once());
        mockQuery.Verify(query => query.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()),
            Times.Once());
    }

    [Fact]
    public async Task LerPilotosAsync_DeveRetornarListaVazia_QuandoNaoExistirPilotosCadastrados()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IPilotoQuery>();

        var listaPilotos = new List<Piloto>();
        var listaPilotosMapeados = new List<ReadPilotoDTO>();

        mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ReadPilotoDTO>>(listaPilotos))
            .Returns(listaPilotosMapeados);

        mockQuery
            .Setup(query => query.ObterTodosAsync(It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync(listaPilotos);

        var service = new ReadPilotoService(mockMapper.Object, mockQuery.Object);

        var resultado = await service.LerPilotosAsync();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);

        mockMapper.Verify(mapper => mapper.Map<IEnumerable<ReadPilotoDTO>>(listaPilotos), Times.Once());
        mockQuery.Verify(query => query.ObterTodosAsync(It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
    }

    [Fact]
    public async Task LerPilotosAsync_DeveRetornarListaReadPilotoDTO_QuandoNaoExistirPilotosCadastrados()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IPilotoQuery>();

        var piloto = new Piloto { Id = 1, Nome = "Piloto Teste" };
        var pilotoDTO = new ReadPilotoDTO { Id = 1, Nome = "Piloto Teste" };

        var listaPilotos = new List<Piloto> { piloto };
        var listaPilotosMapeados = new List<ReadPilotoDTO> { pilotoDTO };

        mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ReadPilotoDTO>>(listaPilotos))
            .Returns(listaPilotosMapeados);

        mockQuery
            .Setup(query => query.ObterTodosAsync(It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync(listaPilotos);

        var service = new ReadPilotoService(mockMapper.Object, mockQuery.Object);

        var resultado = await service.LerPilotosAsync();

        Assert.NotNull(resultado);
        Assert.NotEmpty(resultado);
        Assert.Single(resultado);

        mockMapper.Verify(mapper => mapper.Map<IEnumerable<ReadPilotoDTO>>(listaPilotos), Times.Once());
        mockQuery.Verify(query => query.ObterTodosAsync(It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
    }
}
