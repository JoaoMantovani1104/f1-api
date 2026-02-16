using Moq;
using AutoMapper;
using F1.Lib.Modelos;
using System.Linq.Expressions;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.PilotoServices.Services;
using F1.API.Data.Dtos.PilotoDTO;

namespace F1.Teste.Services.PilotoService;

public class UpdatePilotoServiceTests
{
    [Fact]
    public async Task AtualizarPilotoAsync_DeveRetornarFalse_QuandoNaoEncontrarPiloto()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IPilotoQuery>();
        var mockRepo = new Mock<IRepositoryBase<Piloto>>();

        var pilotoDTO = new UpdatePilotoDTO { Nome = "Piloto Teste" };

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync((Piloto?)null);

        var service = new UpdatePilotoService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await service.AtualizarPilotoAsync(1, pilotoDTO);

        Assert.False(resultado);

        mockQuery.Verify(query => query.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.AtualizarAsync(It.IsAny<Piloto>()), Times.Never());
    }

    [Fact]
    public async Task AtualizarPilotoAsync_DeveLancarInvalidOperationException_QuandoExistirPilotoComMesmoNumero()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IPilotoQuery>();
        var mockRepo = new Mock<IRepositoryBase<Piloto>>();

        var pilotoAtual = new Piloto { Id = 1, Nome = "Hamilton", Numero = 44 };
        var pilotoRival = new Piloto { Id = 2, Nome = "Gasly", Numero = 10 };
        var pilotoDTO = new UpdatePilotoDTO { Numero = 10 };

        var listaPilotos = new List<Piloto> { pilotoAtual, pilotoRival };

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

        var service = new UpdatePilotoService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.AtualizarPilotoAsync(1, pilotoDTO));

        Assert.Equal("Já existe um piloto com esse número.", resultado.Message);

        mockQuery.Verify(query => query.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.AtualizarAsync(It.IsAny<Piloto>()), Times.Never());
    }

    [Fact]
    public async Task AtualizarPilotoAsync_DeveRetornarFalse_QuandoAtualizacaoFalhar()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IPilotoQuery>();
        var mockRepo = new Mock<IRepositoryBase<Piloto>>();

        var piloto = new Piloto { Id = 1, Nome = "Piloto 1", Numero = 10 };
        var pilotoDTO = new UpdatePilotoDTO { Nome = "Piloto 1 Atualizado", Numero = 99 };

        var listaPilotos = new List<Piloto> { piloto };

        mockMapper
            .Setup(mapper => mapper.Map(pilotoDTO, piloto));

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

        mockRepo
            .Setup(repo => repo.AtualizarAsync(piloto))
            .ReturnsAsync(false);

        var service = new UpdatePilotoService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await service.AtualizarPilotoAsync(1, pilotoDTO);

        Assert.False(resultado);

        mockMapper.Verify(mapper => mapper.Map(pilotoDTO, piloto), Times.Once());
        mockQuery.Verify(query => query.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.AtualizarAsync(It.IsAny<Piloto>()), Times.Once());
    }

    [Fact]
    public async Task AtualizarPilotoAsync_DeveRetornarTrue_QuandoAtualizacaoSucesso()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IPilotoQuery>();
        var mockRepo = new Mock<IRepositoryBase<Piloto>>();

        var piloto = new Piloto { Id = 1, Nome = "Piloto 1", Numero = 10 };
        var pilotoDTO = new UpdatePilotoDTO { Nome = "Piloto 1 Atualizado", Numero = 99 };

        var listaPilotos = new List<Piloto> { piloto };

        mockMapper
            .Setup(mapper => mapper.Map(pilotoDTO, piloto));

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

        mockRepo
            .Setup(repo => repo.AtualizarAsync(piloto))
            .ReturnsAsync(true);

        var service = new UpdatePilotoService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await service.AtualizarPilotoAsync(piloto.Id, pilotoDTO);

        Assert.True(resultado);

        mockMapper.Verify(mapper => mapper.Map(pilotoDTO, piloto), Times.Once());
        mockQuery.Verify(query => query.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.AtualizarAsync(piloto), Times.Once());
    }
}
