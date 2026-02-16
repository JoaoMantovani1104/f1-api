using Moq;
using AutoMapper;
using F1.Lib.Modelos;
using System.Linq.Expressions;
using F1.API.Data.Dtos.EquipeDTO;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.EquipeServices.Services;

namespace F1.Teste.Services.EquipeServices;

public class ReadGPServiceTests
{
    [Fact]
    public async Task LerEquipePorIdAsync_DeveRetornarReadEquipeDTO_QuandoEncontrarEquipe()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IEquipeQuery>();

        var equipeLeitura = new ReadEquipeDTO { Id = 1, Nome = "Equipe 1" };
        var equipeBuscada = new Equipe { Id = 1, Nome = "Equipe 1" };
        var listaEquipes = new List<Equipe> { equipeBuscada };

        mockMapper
            .Setup(mapper => mapper.Map<ReadEquipeDTO>(It.IsAny<Equipe>()))
            .Returns(equipeLeitura);

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Expression<Func<Equipe, bool>> expression, 
                           Expression<Func<Equipe, object?>>[] includes) => 
            {
                var func = expression.Compile();
                return listaEquipes.FirstOrDefault(func);
            });

        var service = new ReadEquipeService(mockMapper.Object, mockQuery.Object);

        var resultado = await service.LerEquipePorIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);
        Assert.Equal("Equipe 1", resultado.Nome);

        mockQuery.Verify(query => query.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()
            ), Times.Once);
    }

    [Fact]
    public async Task LerEquipePorIdAsync_DeveRetornarNull_QuandoNaoEncontrarGP()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IEquipeQuery>();

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(), 
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Equipe?)null);

        var service = new ReadEquipeService(mockMapper.Object, mockQuery.Object);

        var resultado = await service.LerEquipePorIdAsync(1);

        Assert.Null(resultado);
        mockMapper.Verify(m => m.Map<ReadEquipeDTO>(It.IsAny<Equipe>()), Times.Never);
    }

    [Fact]
    public async Task LerEquipesAsync_DeveRetornarListaDeEquipes_QuandoExistirAoMenosUmaEquipeCadastrada()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IEquipeQuery>();

        var equipe = new Equipe { Id = 1, Nome = "Equipe 1" };
        var equipeDTO = new ReadEquipeDTO { Id = 1, Nome = "Equipe 1" };

        var listaEquipesDTO = new List<ReadEquipeDTO> { equipeDTO };
        var listaEquipesBuscada = new List<Equipe> { equipe };

        mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ReadEquipeDTO>>(It.IsAny<IEnumerable<Equipe>>()))
            .Returns(listaEquipesDTO);

        mockQuery
            .Setup(query => query.ObterTodosAsync(It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(listaEquipesBuscada);

        var service = new ReadEquipeService(mockMapper.Object, mockQuery.Object);

        var resultado = await service.LerEquipesAsync();

        Assert.NotNull(resultado);
        Assert.NotEmpty(resultado);
        Assert.Single(resultado);

        mockQuery.Verify(query => query.ObterTodosAsync(It.IsAny<Expression<Func<Equipe, object?>>[]>()), Times.Once); 
        mockMapper.Verify(mapper => mapper.Map<IEnumerable<ReadEquipeDTO>>(It.IsAny<IEnumerable<Equipe>>()), Times.Once);
    }

    [Fact]
    public async Task LerEquipesAsync_DeveRetornarListaVazia_QuandoNaoExistirEquipeCadastrada()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IEquipeQuery>();

        mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ReadEquipeDTO>>(It.IsAny<IEnumerable<Equipe>>()))
            .Returns(new List<ReadEquipeDTO>());

        mockQuery
            .Setup(query => query.ObterTodosAsync(It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(new List<Equipe>());

        var service = new ReadEquipeService(mockMapper.Object, mockQuery.Object);

        var resultado = await service.LerEquipesAsync();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
        mockMapper.Verify(m => m.Map<IEnumerable<ReadEquipeDTO>>(
            It.Is<IEnumerable<Equipe>>(e => e != null && !e.Any()) 
            ), Times.Once);
    }
}