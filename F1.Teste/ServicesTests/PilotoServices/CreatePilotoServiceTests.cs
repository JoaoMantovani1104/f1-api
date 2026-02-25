using Moq;
using F1.Lib.Modelos;
using F1.Teste.Bogus;
using System.Linq.Expressions;
using F1.API.Data.Dtos.PilotoDTO;
using F1.Lib.Interfaces.Especificas.Query;

namespace F1.Teste.ServicesTests.PilotoServices;

public class CreatePilotoServiceTests : BasePilotoServiceTests
{
    [Fact]
    public async Task AdicionarPilotoAsync_DeveLancarInvalidOperationException_QuandoEquipeNaoExistir()
    {
        var createDto = PilotoDTOFaker.GerarCreate().Generate();
        createDto.EquipeId = -1;

        mockEquipeQuery
            .Setup(q => q.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Equipe?)null);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => createService.AdicionarPilotoAsync(createDto));

        Assert.Equal($"A equipe com o ID {createDto.EquipeId} não foi encontrada", resultado.Message);

        mockRepo.Verify(r => r.AdicionarAsync(It.IsAny<Piloto>()), Times.Never);
    }

    [Fact]
    public async Task AdicionarPilotoAsync_DeveLancarInvalidOperationException_QuandoJaExistePilotoComMesmoNumero()
    {
        var pilotoComMesmoNumero = PilotoFaker.Gerar().Generate();

        var pilotoCreateDTO = PilotoDTOFaker.GerarCreate().Generate();
        pilotoCreateDTO.Numero = pilotoComMesmoNumero.Numero;

        var equipe = EquipeFaker.Gerar().Generate();
        equipe.Id = pilotoCreateDTO.EquipeId;

        mockEquipeQuery
            .Setup(q => q.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(equipe);

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync(pilotoComMesmoNumero);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => createService.AdicionarPilotoAsync(pilotoCreateDTO));

        Assert.Equal($"Piloto com o número {pilotoCreateDTO.Numero} já existente", resultado.Message);
        
        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<Piloto>()), Times.Never());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task AdicionarPilotoAsync_DeveRetornarReadPilotoDTO_QuandoPilotoValido()
    {
        var piloto = PilotoFaker.Gerar().Generate();

        var pilotoCreateDTO = PilotoDTOFaker.GerarCreate().Generate(); 
        pilotoCreateDTO.Nome = piloto.Nome;

        var pilotoReadDTO = PilotoDTOFaker.GerarRead().Generate();
        pilotoReadDTO.Id = piloto.Id;
        pilotoReadDTO.Nome = piloto.Nome;

        var equipe = EquipeFaker.Gerar().Generate();
        equipe.Id = pilotoCreateDTO.EquipeId;

        mockEquipeQuery
            .Setup(q => q.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(equipe);

        mockMapper
            .Setup(mapper => mapper.Map<Piloto>(pilotoCreateDTO))
            .Returns(piloto);

        mockMapper
            .Setup(mapper => mapper.Map<ReadPilotoDTO>(piloto))
            .Returns(pilotoReadDTO);

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync((Piloto?)null);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(true);

        var resultado = await createService.AdicionarPilotoAsync(pilotoCreateDTO);

        Assert.NotNull(resultado);
        Assert.Equal(piloto.Id, resultado.Id);

        mockMapper.Verify(mapper => mapper.Map<Piloto>(pilotoCreateDTO), Times.Once());
        mockMapper.Verify(mapper => mapper.Map<ReadPilotoDTO>(piloto), Times.Once());
        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<Piloto>()), Times.Once());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task AdicionarPilotoAsync_DeveLancarException_QuandoPilotoValido()
    {
        var piloto = PilotoFaker.Gerar().Generate();

        var pilotoCreateDTO = PilotoDTOFaker.GerarCreate().Generate();
        pilotoCreateDTO.Nome = piloto.Nome;

        var equipe = EquipeFaker.Gerar().Generate();
        equipe.Id = pilotoCreateDTO.EquipeId;

        mockEquipeQuery
            .Setup(q => q.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(equipe);

        mockMapper
            .Setup(mapper => mapper.Map<Piloto>(pilotoCreateDTO))
            .Returns(piloto);

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync((Piloto?)null);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(false);

        var resultado = await Assert.ThrowsAsync<Exception>(
            () => createService.AdicionarPilotoAsync(pilotoCreateDTO));

        Assert.Equal("Ocorreu um erro ao salvar o piloto no banco de dados.", resultado.Message);

        mockRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<Piloto>()), Times.Once());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }
}
