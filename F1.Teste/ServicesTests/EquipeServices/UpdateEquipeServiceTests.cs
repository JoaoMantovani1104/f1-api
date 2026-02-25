using Moq;
using F1.Lib.Modelos;
using F1.Teste.Bogus;
using System.Linq.Expressions;
using F1.API.Data.Dtos.EquipeDTO;

namespace F1.Teste.ServicesTests.EquipeServices;

public class UpdateEquipeServiceTests : BaseEquipeServiceTests
{
    [Fact]
    public async Task AtualizarEquipeAsync_DeveRetornarFalse_QuandoNaoEncontrarEquipeParaAtualizar()
    {
        var equipeAAtualizar = EquipeFaker.Gerar().Generate();
        var equipeAAtualizarDTO = EquipeDTOFaker.GerarUpdate().Generate();

        mockMapper
            .Setup(m => m.Map(equipeAAtualizarDTO, equipeAAtualizar));

        mockQuery
            .Setup(q => q.BuscarPorPropriedadeAsync(It.IsAny<Expression<Func<Equipe, bool>>>()))
            .ReturnsAsync((Equipe?)null);

        var resultado = await updateService.AtualizarEquipeAsync(equipeAAtualizar.Id, equipeAAtualizarDTO);

        Assert.False(resultado);

        mockQuery.Verify(q => q.BuscarPorPropriedadeAsync(It.IsAny<Expression<Func<Equipe, bool>>>()), Times.Once);
        mockMapper.Verify(m => m.Map(It.IsAny<UpdateEquipeDTO>(), It.IsAny<Equipe>()), Times.Never);
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task AtualizarEquipeAsync_DeveLancarInvalidOperationException_QuandoExisteOutraEquipeComMesmoNome()
    {
        var equipeAAtualizar = EquipeFaker.Gerar().Generate();
        var equipeAAtualizarDTO = EquipeDTOFaker.GerarUpdate().Generate();

        var equipeComMesmoNome = EquipeFaker.Gerar().Generate();
        equipeComMesmoNome.Nome = equipeAAtualizarDTO.Nome;

        mockMapper
            .Setup(m => m.Map(equipeAAtualizarDTO, equipeAAtualizar));

        mockQuery
            .SetupSequence(q => q.BuscarPorPropriedadeAsync(It.IsAny<Expression<Func<Equipe, bool>>>()))
            .ReturnsAsync(equipeAAtualizar)
            .ReturnsAsync(equipeComMesmoNome);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => updateService.AtualizarEquipeAsync(equipeAAtualizar.Id, equipeAAtualizarDTO));

        Assert.Equal($"Já existe outra equipe cadastrada com o nome '{equipeAAtualizarDTO.Nome}'.", resultado.Message);

        mockRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<Equipe>()), Times.Never);
        mockUow.Verify(uow => uow.CommitAsync(), Times.Never());
    }

    [Fact]
    public async Task AtualizarEquipeAsync_DeveRetornarFalse_QuandoAtualizacaoFalhar()
    {
        var equipeAAtualizar = EquipeFaker.Gerar().Generate();
        var equipeAAtualizarDTO = EquipeDTOFaker.GerarUpdate().Generate();

        mockMapper
            .Setup(m => m.Map(equipeAAtualizarDTO, equipeAAtualizar));

        mockQuery
            .SetupSequence(q => q.BuscarPorPropriedadeAsync(It.IsAny<Expression<Func<Equipe, bool>>>()))
            .ReturnsAsync(equipeAAtualizar)  
            .ReturnsAsync((Equipe?)null);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(false);

        var resultado = await updateService.AtualizarEquipeAsync(equipeAAtualizar.Id, equipeAAtualizarDTO);

        Assert.False(resultado);

        mockQuery.Verify(q => q.BuscarPorPropriedadeAsync(It.IsAny<Expression<Func<Equipe, bool>>>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.Atualizar(equipeAAtualizar), Times.Once());
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task AtualizarEquipeAsync_DeveRetornarTrue_QuandoAtualizarComSucesso()
    {
        var equipeAAtualizar = EquipeFaker.Gerar().Generate();
        var equipeAAtualizarDTO = EquipeDTOFaker.GerarUpdate().Generate();

        mockMapper
            .Setup(m => m.Map(equipeAAtualizarDTO, equipeAAtualizar));

        mockQuery
            .SetupSequence(q => q.BuscarPorPropriedadeAsync(It.IsAny<Expression<Func<Equipe, bool>>>()))
            .ReturnsAsync(equipeAAtualizar) 
            .ReturnsAsync((Equipe?)null);

        mockUow
            .Setup(uow => uow.CommitAsync())
            .ReturnsAsync(true);

        var resultado = await updateService.AtualizarEquipeAsync(equipeAAtualizar.Id, equipeAAtualizarDTO);

        Assert.True(resultado);

        mockMapper.Verify(mapper => mapper.Map(equipeAAtualizarDTO, equipeAAtualizar), Times.Once);
        mockQuery.Verify(q => q.BuscarPorPropriedadeAsync(It.IsAny<Expression<Func<Equipe, bool>>>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.Atualizar(equipeAAtualizar), Times.Once);
        mockUow.Verify(uow => uow.CommitAsync(), Times.Once());
    }

}
