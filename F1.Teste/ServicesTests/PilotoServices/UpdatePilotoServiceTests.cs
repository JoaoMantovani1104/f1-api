using Moq;
using F1.Lib.Modelos;
using F1.Teste.Bogus;
using System.Linq.Expressions;
using F1.Lib.Interfaces.Especificas.Query;

namespace F1.Teste.ServicesTests.PilotoServices;

public class UpdatePilotoServiceTests : BasePilotoServiceTests
{
    [Fact]
    public async Task AtualizarPilotoAsync_DeveLancarInvalidOperationException_QuandoEquipeNaoExistir()
    {
        var mockEquipeQuery = new Mock<IEquipeQuery>();

        var piloto = PilotoFaker.Gerar().Generate();
        var updateDto = PilotoDTOFaker.GerarUpdate().Generate();
        updateDto.EquipeId = -1;

        mockEquipeQuery.Setup(q => q.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Equipe?)null);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => updateService.AtualizarPilotoAsync(piloto.Id, updateDto));

        Assert.Equal($"A equipe com o ID {updateDto.EquipeId} não foi encontrada", resultado.Message);

        mockRepo.Verify(r => r.Atualizar(It.IsAny<Piloto>()), Times.Never);
    }

    [Fact]
    public async Task AtualizarPilotoAsync_DeveRetornarFalse_QuandoNaoEncontrarPiloto()
    {
        var pilotoDTO = PilotoDTOFaker.GerarUpdate().Generate();

        var equipe = EquipeFaker.Gerar().Generate();
        equipe.Id = pilotoDTO.EquipeId;

        mockQuery
            .Setup(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync((Piloto?)null);

        mockEquipeQuery.Setup(q => q.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(equipe);

        var resultado = await updateService.AtualizarPilotoAsync(1, pilotoDTO);

        Assert.False(resultado);

        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.Atualizar(It.IsAny<Piloto>()), Times.Never());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task AtualizarPilotoAsync_DeveLancarInvalidOperationException_QuandoExistirPilotoComMesmoNumero()
    {
        var pilotoAtual = PilotoFaker.Gerar().Generate();
        pilotoAtual.Numero = 44;

        var pilotoRival = PilotoFaker.Gerar().Generate();
        pilotoRival.Numero = 10; 

        var pilotoDTO = PilotoDTOFaker.GerarUpdate().Generate();
        pilotoDTO.Numero = pilotoRival.Numero;

        var equipe = EquipeFaker.Gerar().Generate();
        equipe.Id = pilotoDTO.EquipeId;

        mockQuery
            .SetupSequence(query => query.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync(pilotoAtual) 
            .ReturnsAsync(pilotoRival);

        mockEquipeQuery.Setup(q => q.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(equipe);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => updateService.AtualizarPilotoAsync(pilotoAtual.Id, pilotoDTO));

        Assert.Equal("Já existe um piloto com esse número.", resultado.Message);

        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.Atualizar(It.IsAny<Piloto>()), Times.Never());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task AtualizarPilotoAsync_DeveRetornarFalse_QuandoAtualizacaoFalhar()
    {
        var piloto = PilotoFaker.Gerar().Generate();
        piloto.Numero = 10;

        var pilotoDTO = PilotoDTOFaker.GerarUpdate().Generate();
        pilotoDTO.Numero = 99;

        var equipe = EquipeFaker.Gerar().Generate();
        equipe.Id = pilotoDTO.EquipeId;

        mockMapper.Setup(mapper => mapper.Map(pilotoDTO, piloto));

        mockQuery
            .SetupSequence(query => query.BuscarPorPropriedadeAsync(
                 It.IsAny<Expression<Func<Piloto, bool>>>(),
                 It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync(piloto)
            .ReturnsAsync((Piloto?)null);

        mockEquipeQuery.Setup(q => q.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(equipe);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(false);

        var resultado = await updateService.AtualizarPilotoAsync(piloto.Id, pilotoDTO);

        Assert.False(resultado);

        mockMapper.Verify(mapper => mapper.Map(pilotoDTO, piloto), Times.Once());
        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.Atualizar(It.IsAny<Piloto>()), Times.Once());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task AtualizarPilotoAsync_DeveRetornarTrue_QuandoAtualizarComSucesso()
    {
        var piloto = PilotoFaker.Gerar().Generate();

        var pilotoDTO = PilotoDTOFaker.GerarUpdate().Generate();
        pilotoDTO.Numero = piloto.Numero;

        var equipe = EquipeFaker.Gerar().Generate();
        equipe.Id = pilotoDTO.EquipeId;


        mockMapper.Setup(mapper => mapper.Map(pilotoDTO, piloto));

        mockQuery
             .Setup(query => query.BuscarPorPropriedadeAsync(
                  It.IsAny<Expression<Func<Piloto, bool>>>(),
                  It.IsAny<Expression<Func<Piloto, object?>>[]>()))
             .ReturnsAsync(piloto);

        mockEquipeQuery.Setup(q => q.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(equipe);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(true);

        var resultado = await updateService.AtualizarPilotoAsync(piloto.Id, pilotoDTO);

        Assert.True(resultado);

        mockMapper.Verify(mapper => mapper.Map(pilotoDTO, piloto), Times.Once());
        mockQuery.Verify(query => query.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.Atualizar(piloto), Times.Once());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }
}
