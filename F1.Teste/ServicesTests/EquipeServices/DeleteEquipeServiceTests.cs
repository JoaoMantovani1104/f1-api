using Moq;
using F1.Lib.Modelos;
using F1.Teste.Bogus;
using System.Linq.Expressions;

namespace F1.Teste.ServicesTests.EquipeServices;

public class DeleteEquipeServiceTests : BaseEquipeServiceTests
{
    [Fact]
    public async Task DeletarEquipeAsync_DeveRetornarFalse_QuandoNaoEncontrarEquipe()
    {
        mockQuery
            .Setup(q => q.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Equipe?)null);

        var resultado = await deleteService.DeletarEquipeAsync(1);

        Assert.False(resultado);

        mockQuery.Verify(q => q.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), 
            Times.Once);

        mockRepo.Verify(r => r.Deletar(It.IsAny<Equipe>()), Times.Never);
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task DeletarEquipeAsync_DeveLancarInvalidOperationException_QuandoEquipePossuirPilotos()
    {
        var equipeComPiloto = EquipeFaker.Gerar().Generate();
        equipeComPiloto.Pilotos = PilotoFaker.Gerar().Generate(2);

        mockQuery
            .Setup(q => q.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(equipeComPiloto);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => deleteService.DeletarEquipeAsync(equipeComPiloto.Id));

        Assert.Equal("Não é possível deletar uma equipe que possui pilotos associados.", resultado.Message);

        mockQuery.Verify(q => q.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), 
            Times.Once);

        mockRepo.Verify(repo => repo.Deletar(equipeComPiloto), Times.Never());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task DeletarEquipeAsync_DeveRetornarFalse_QuandoDelecaoFalharNoRepositorio()
    {
        var equipeADeletar = EquipeFaker.Gerar().Generate();
        equipeADeletar.Pilotos = [];

        mockQuery
            .Setup(q => q.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(equipeADeletar);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(false);

        var resultado = await deleteService.DeletarEquipeAsync(equipeADeletar.Id);

        Assert.False(resultado);

        mockQuery.Verify(q => q.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), 
            Times.Once);

        mockRepo.Verify(r => r.Deletar(It.IsAny<Equipe>()), Times.Once);
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task DeletarEquipeAsync_DeveRetornarTrue_QuandoDeletarComSucesso()
    {
        var equipeADeletar = EquipeFaker.Gerar().Generate();
        equipeADeletar.Pilotos = [];

        mockQuery
            .Setup(q => q.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(equipeADeletar);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(true);

        var resultado = await deleteService.DeletarEquipeAsync(equipeADeletar.Id);

        Assert.True(resultado);

        mockQuery.Verify(q => q.BuscarPorPropriedadeAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), 
            Times.Once);

        mockRepo.Verify(r => r.Deletar(It.IsAny<Equipe>()), Times.Once);
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task DeletarEquipeAsync_DeveRetornarTrue_QuandoListaDePilotosForNula()
    {
        var equipeComPilotosNulos = EquipeFaker.Gerar().Generate();
        equipeComPilotosNulos.Pilotos = null;

        mockQuery.Setup(q => q.BuscarPorPropriedadeAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(equipeComPilotosNulos);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(true);

        var resultado = await deleteService.DeletarEquipeAsync(equipeComPilotosNulos.Id);

        Assert.True(resultado);
        mockRepo.Verify(r => r.Deletar(equipeComPilotosNulos), Times.Once);
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }
}
