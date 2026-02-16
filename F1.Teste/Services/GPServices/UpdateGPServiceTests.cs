using Moq;
using AutoMapper;
using F1.Lib.Modelos;
using System.Linq.Expressions;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.GpServices.Services;
using F1.API.Data.Dtos.GrandePremioDTO;

namespace F1.Teste.Services.GPServices;


public class UpdateGPServiceTests
{
    [Fact]
    public async Task AtualizarGPAsync_DeveRetornarFalse_QuandoNaoEncontrarGP()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IGrandePremioQuery>();
        var mockRepo = new Mock<IRepositoryBase<GrandePremio>>();

        var gpDTO = new UpdateGpDTO { Nome = "GP Atualizado" };

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((GrandePremio?)null);

        var service = new UpdateGrandePremioService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await service.AtualizarGPAsync(1, gpDTO);

        Assert.False(resultado);

        mockQuery.Verify(query => query.BuscarPorCampoAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.AtualizarAsync(It.IsAny<GrandePremio>()), Times.Never());
    }

    [Fact]
    public async Task AtualizarGPAsync_DeveLancarInvalidOperationException_QuandoJaHouverGPComMesmoNome()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IGrandePremioQuery>();
        var mockRepo = new Mock<IRepositoryBase<GrandePremio>>();

        var gp = new GrandePremio { Nome = "GP Teste" };
        var gpComMesmoNome = new GrandePremio { Nome = "GP Atualizado" };
        var gpDTO = new UpdateGpDTO { Nome = "GP Atualizado" };

        var listaGps = new List<GrandePremio> { gp };

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((Expression<Func<GrandePremio, bool>> expression,
                Expression<Func<GrandePremio, object?>>[] includes) =>
            {
                var func = expression.Compile();
                return listaGps.FirstOrDefault(func);
            });

        mockQuery
            .Setup(q => q.BuscarPorCampoAsync(It.IsAny<Expression<Func<GrandePremio, bool>>>()))
            .ReturnsAsync(gpComMesmoNome);

        var service = new UpdateGrandePremioService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.AtualizarGPAsync(1, gpDTO));

        Assert.Equal($"Já existe outro Grande Prêmio cadastrado com o nome '{gpDTO.Nome}'.", resultado.Message);

        mockQuery.Verify(query => query.BuscarPorCampoAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.AtualizarAsync(It.IsAny<GrandePremio>()), Times.Never());
    }

    [Fact]
    public async Task AtualizarGPAsync_DeveRetornarFalse_QuandoAtualizacaoFalhar()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IGrandePremioQuery>();
        var mockRepo = new Mock<IRepositoryBase<GrandePremio>>();

        var gpDTO = new UpdateGpDTO { Nome = "GP 1 Atualizado" };
        var gp = new GrandePremio { Id = 1, Nome = "GP 1", Vencedor = null };

        var listaGps = new List<GrandePremio> { gp };

        mockMapper
            .Setup(mapper => mapper.Map(gpDTO, gp));

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((Expression<Func<GrandePremio, bool>> expression,
                Expression<Func<GrandePremio, object?>>[] includes) =>
            {
                string expressao = expression.ToString();

                if (expressao.Contains("ILike") || expressao.Contains("Functions")) return null;

                try
                {
                    var func = expression.Compile();
                    return listaGps.FirstOrDefault(func);
                }
                catch
                {
                    return null;
                }
            });

        mockRepo
            .Setup(repo => repo.AtualizarAsync(gp))
            .ReturnsAsync(false);

        var service = new UpdateGrandePremioService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await service.AtualizarGPAsync(gp.Id, gpDTO);

        Assert.False(resultado);

        mockMapper.Verify(mapper => mapper.Map(gpDTO, gp), Times.Once());
        mockQuery.Verify(query => query.BuscarPorCampoAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.AtualizarAsync(It.IsAny<GrandePremio>()), Times.Once());
    }

    [Fact]
    public async Task AtualizarGPAsync_DeveRetornarTrue_QuandoDelecaoSucesso()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IGrandePremioQuery>();
        var mockRepo = new Mock<IRepositoryBase<GrandePremio>>();

        var gpDTO = new UpdateGpDTO { Nome = "GP 1" };
        var gp = new GrandePremio { Id = 1, Nome = "GP 1 Atualizado", Vencedor = null };

        var listaGps = new List<GrandePremio> { gp };

        mockMapper
            .Setup(mapper => mapper.Map(gpDTO, gp));

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                 It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                 It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((Expression<Func<GrandePremio, bool>> expression,
                Expression<Func<GrandePremio, object?>>[] includes) =>
            {
                string expressao = expression.ToString();

                if (expressao.Contains("ILike") || expressao.Contains("Functions")) return null;

                try
                {
                    var func = expression.Compile();
                    return listaGps.FirstOrDefault(func);
                } catch {
                    return null;
                }
            });

        mockRepo
            .Setup(repo => repo.AtualizarAsync(gp))
            .ReturnsAsync(true);

        var service = new UpdateGrandePremioService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await service.AtualizarGPAsync(gp.Id, gpDTO);

        Assert.True(resultado);

        mockMapper.Verify(mapper => mapper.Map(gpDTO, gp), Times.Once());
        mockQuery.Verify(query => query.BuscarPorCampoAsync(
                    It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                    It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), Times.Exactly(2));
        mockRepo.Verify(repo => repo.AtualizarAsync(It.IsAny<GrandePremio>()), Times.Once());
    }
}
