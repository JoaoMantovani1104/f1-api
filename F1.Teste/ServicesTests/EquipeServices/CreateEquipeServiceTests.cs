using Moq;
using F1.Lib.Modelos;
using F1.Teste.Bogus;
using System.Linq.Expressions;
using F1.API.Data.Dtos.EquipeDTO;

namespace F1.Teste.ServicesTests.EquipeServices;

public class CreateEquipeServiceTests : BaseEquipeServiceTests
{
    [Fact]
    public async Task AdicionarEquipeAsync_DeveRetornarReadEquipeDTO_QuandoEquipeValida()
    {
        var equipeEntrada = EquipeFaker.Gerar().Generate();

        var equipeCreateDTO = EquipeDTOFaker.GerarCreate().Generate();
        equipeCreateDTO.Nome = equipeEntrada.Nome;

        var equipeReadDTO = EquipeDTOFaker.GerarRead().Generate();
        equipeReadDTO.Nome = equipeEntrada.Nome;
        equipeReadDTO.Id = equipeEntrada.Id;

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Equipe?)null);

        mockMapper
            .Setup(mapper => mapper.Map<Equipe>(equipeCreateDTO))
            .Returns(equipeEntrada);

        mockMapper
            .Setup(mapper => mapper.Map<ReadEquipeDTO>(equipeEntrada))
            .Returns(equipeReadDTO);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(true);

        var resultado = await createService.AdicionarEquipeAsync(equipeCreateDTO);

        Assert.NotNull(resultado);
        Assert.Equal(equipeReadDTO.Id, resultado.Id);

        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), Times.Once());

        mockMapper.Verify(mapper => mapper.Map<Equipe>(equipeCreateDTO), Times.Once());
        mockRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<Equipe>()), Times.Once);
        mockMapper.Verify(mapper => mapper.Map<ReadEquipeDTO>(equipeEntrada), Times.Once());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task AdicionarEquipeAsync_DeveLancarInvalidOperationException_QuandoEquipeJaExistente()
    {
        var equipeEntrada = EquipeFaker.Gerar().Generate();

        var equipeAAdicionar = EquipeDTOFaker.GerarCreate().Generate();
        equipeAAdicionar.Nome = equipeEntrada.Nome;

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(equipeEntrada);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => createService.AdicionarEquipeAsync(equipeAAdicionar));

        Assert.Equal($"Equipe com nome '{equipeAAdicionar.Nome}' já existente.", resultado.Message);

        mockRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<Equipe>()), Times.Never);
        mockMapper.Verify(mapper => mapper.Map<Equipe>(It.IsAny<CreateEquipeDTO>()), Times.Never);
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task AdicionarEquipeAsync_DeveLancarException_QuandoBancoFalharAoSalvar()
    {
        var equipeEntrada = EquipeFaker.Gerar().Generate();

        var equipeCreateDTO = EquipeDTOFaker.GerarCreate().Generate();
        equipeCreateDTO.Nome = equipeEntrada.Nome;

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Equipe?)null);

        mockMapper
            .Setup(mapper => mapper.Map<Equipe>(equipeCreateDTO))
            .Returns(equipeEntrada);
        
        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(false);

        var resultado = await Assert.ThrowsAsync<Exception> ( 
            () => createService.AdicionarEquipeAsync(equipeCreateDTO));

        Assert.Equal("Ocorreu um erro ao salvar a equipe no banco de dados.", resultado.Message);

        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), Times.Once());

        mockRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<Equipe>()), Times.Once);
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }
}