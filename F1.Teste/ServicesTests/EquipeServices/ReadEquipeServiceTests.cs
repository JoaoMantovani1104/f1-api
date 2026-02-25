using Moq;
using F1.Lib.Modelos;
using F1.Teste.Bogus;
using System.Linq.Expressions;
using F1.API.Data.Dtos.EquipeDTO;

namespace F1.Teste.ServicesTests.EquipeServices;

public class ReadEquipeServiceTests : BaseEquipeServiceTests
{
    [Fact]
    public async Task LerEquipePorIdAsync_DeveRetornarReadEquipeDTO_QuandoEncontrarEquipe()
    {
        var equipeBuscada = EquipeFaker.Gerar().Generate();

        var equipeLeitura = EquipeDTOFaker.GerarRead().Generate();
        equipeLeitura.Id = equipeBuscada.Id;
        equipeLeitura.Nome = equipeBuscada.Nome;

        mockMapper
            .Setup(mapper => mapper.Map<ReadEquipeDTO>(equipeBuscada))
            .Returns(equipeLeitura);

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(equipeBuscada);

        var resultado = await readService.LerEquipePorIdAsync(equipeBuscada.Id);

        Assert.NotNull(resultado);
        Assert.Equal(equipeBuscada.Id, resultado.Id);

        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), 
            Times.Once);

        mockMapper.Verify(m => m.Map<ReadEquipeDTO>(equipeBuscada), Times.Once);
    }

    [Fact]
    public async Task LerEquipePorIdAsync_DeveRetornarNull_QuandoNaoEncontrarEquipe()
    {
        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(), 
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Equipe?)null);

        var resultado = await readService.LerEquipePorIdAsync(1);

        Assert.Null(resultado);

        mockMapper.Verify(m => m.Map<ReadEquipeDTO>(It.IsAny<Equipe>()), Times.Never);
    }

    [Fact]
    public async Task LerEquipesAsync_DeveRetornarListaDeEquipes_QuandoEncontrarAoMenosUmaEquipeCadastrada()
    {
        var listaEquipesBuscada = EquipeFaker.Gerar().Generate(10);
        var listaEquipesDTO = listaEquipesBuscada.Select(e => new ReadEquipeDTO
        {
            Id = e.Id,
            Nome = e.Nome,
        }).ToList();

        mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ReadEquipeDTO>>(listaEquipesBuscada))
            .Returns(listaEquipesDTO);

        mockQuery
            .Setup(query => query.ObterTodosAsync(It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(listaEquipesBuscada);

        var resultado = await readService.LerEquipesAsync();

        Assert.NotNull(resultado);
        Assert.NotEmpty(resultado);
        Assert.Equal(10, resultado.Count());

        mockQuery.Verify(query => query.ObterTodosAsync(
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), Times.Once); 

        mockMapper.Verify(mapper => mapper.Map<IEnumerable<ReadEquipeDTO>>(listaEquipesBuscada), Times.Once);
    }

    [Fact]
    public async Task LerEquipesAsync_DeveRetornarListaVazia_QuandoNaoEncontrarEquipesCadastradas()
    {
        mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ReadEquipeDTO>>(It.IsAny<IEnumerable<Equipe>>()))
            .Returns(new List<ReadEquipeDTO>());

        mockQuery
            .Setup(query => query.ObterTodosAsync(It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(new List<Equipe>());

        var resultado = await readService.LerEquipesAsync();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);

        mockQuery.Verify(q => q.ObterTodosAsync(
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), Times.Once);

        mockMapper.Verify(m => m.Map<IEnumerable<ReadEquipeDTO>>(
            It.IsAny<IEnumerable<Equipe>>()), Times.Once);
    }
}