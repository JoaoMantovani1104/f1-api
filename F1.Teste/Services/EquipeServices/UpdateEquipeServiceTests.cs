using Moq;
using AutoMapper;
using F1.Lib.Modelos;
using System.Linq.Expressions;
using F1.API.Data.Dtos.EquipeDTO;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.EquipeServices.Services;

namespace F1.Teste.Services.EquipeServices;

public class UpdateGPServiceTests
{
    [Fact]
    public async Task AtualizarEquipeAsync_DeveRetornarFalse_QuandoNaoEncontrarEquipeParaAtualizar()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IEquipeQuery>();
        var mockRepo = new Mock<IRepositoryBase<Equipe>>();

        var equipeAAtualizar = new Equipe { Id = 1, Nome = "Equipe 1" };
        var equipeAAtualizarDTO = new UpdateEquipeDTO { Nome = "Equipe 1 Atualizada" };

        mockMapper
            .Setup(m => m.Map(equipeAAtualizarDTO, equipeAAtualizar));

        mockQuery
            .Setup(q => q.BuscarPorCampoAsync(It.IsAny<Expression<Func<Equipe, bool>>>()))
            .ReturnsAsync((Equipe?)null);

        var service = new UpdateEquipeService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await service.AtualizarEquipeAsync(equipeAAtualizar.Id, equipeAAtualizarDTO);

        Assert.False(resultado);
        mockQuery.Verify(q => q.BuscarPorCampoAsync(It.IsAny<Expression<Func<Equipe, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task AtualizarEquipeAsync_DeveLancarInvalidOperationException_QuandoExisteOutraEquipeComMesmoNome()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IEquipeQuery>();
        var mockRepo = new Mock<IRepositoryBase<Equipe>>();

        var equipeAAtualizar = new Equipe { Id = 1, Nome = "Equipe 1" };
        var equipeAAtualizarDTO = new UpdateEquipeDTO { Nome = "Equipe 1 Atualizada" };
        var equipeComMesmoNome = new Equipe { Id = 2, Nome = "Equipe 1 Atualizada" };
        var listaEquipes = new List<Equipe> { equipeAAtualizar };

        mockMapper
            .Setup(m => m.Map(equipeAAtualizarDTO, equipeAAtualizar));

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

        mockQuery
            .Setup(q => q.BuscarPorCampoAsync(It.IsAny<Expression<Func<Equipe, bool>>>()))
            .ReturnsAsync(equipeComMesmoNome);

        var service = new UpdateEquipeService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.AtualizarEquipeAsync(equipeAAtualizar.Id, equipeAAtualizarDTO));

        mockRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<Equipe>()), Times.Never);
        Assert.Equal($"Já existe outra equipe cadastrada com o nome '{equipeAAtualizarDTO.Nome}'.", resultado.Message);
    }

    [Fact]
    public async Task AtualizarEquipeAsync_DeveRetornarFalse_QuandoAlteracaoFalha()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IEquipeQuery>();
        var mockRepo = new Mock<IRepositoryBase<Equipe>>();

        var equipeAAtualizar = new Equipe { Id = 1, Nome = "Equipe 1" };
        var equipeAAtualizarDTO = new UpdateEquipeDTO { Nome = "Equipe 1 Atualizada" };
        var listaEquipes = new List<Equipe> { equipeAAtualizar };

        mockMapper
            .Setup(m => m.Map(equipeAAtualizarDTO, equipeAAtualizar));

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

        mockRepo
            .Setup(repo => repo.AtualizarAsync(It.IsAny<Equipe>()))
            .ReturnsAsync(false);

        var service = new UpdateEquipeService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await service.AtualizarEquipeAsync(equipeAAtualizar.Id, equipeAAtualizarDTO);

        Assert.False(resultado);
        mockQuery.Verify(q => q.BuscarPorCampoAsync(It.IsAny<Expression<Func<Equipe, bool>>>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.AtualizarAsync(It.IsAny<Equipe>()), Times.Once);
        mockMapper.Verify(mapper => mapper.Map(equipeAAtualizarDTO, equipeAAtualizar), Times.Once);
    }

    [Fact]
    public async Task AtualizarEquipeAsync_DeveRetornarTrue_QuandoAlteracaoSucesso()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IEquipeQuery>();
        var mockRepo = new Mock<IRepositoryBase<Equipe>>();

        var equipeAAtualizar = new Equipe { Id = 1, Nome = "Equipe 1" };
        var equipeAAtualizarDTO = new UpdateEquipeDTO { Nome = "Equipe 1 Atualizada" };
        var listaEquipes = new List<Equipe> { equipeAAtualizar };

        mockMapper
            .Setup(m => m.Map(equipeAAtualizarDTO, equipeAAtualizar));

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Expression<Func<Equipe, bool>> expression,
                           Expression<Func<Equipe, object?>>[] includes) =>
            {
                var func = expression.Compile();
                return listaEquipes.FirstOrDefault(func);
            }); ;

        mockRepo
            .Setup(repo => repo.AtualizarAsync(It.IsAny<Equipe>()))
            .ReturnsAsync(true);

        var service = new UpdateEquipeService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await service.AtualizarEquipeAsync(equipeAAtualizar.Id, equipeAAtualizarDTO);

        Assert.True(resultado);
        mockQuery.Verify(q => q.BuscarPorCampoAsync(It.IsAny<Expression<Func<Equipe, bool>>>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.AtualizarAsync(It.IsAny<Equipe>()), Times.Once);
        mockMapper.Verify(mapper => mapper.Map(equipeAAtualizarDTO, equipeAAtualizar), Times.Once);
    }

}
