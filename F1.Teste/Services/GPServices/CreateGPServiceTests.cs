using Moq;
using AutoMapper;
using F1.Lib.Modelos;
using System.Linq.Expressions;
using F1.Lib.Interfaces.Genericas;
using F1.API.Data.Dtos.GrandePremioDTO;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.GpServices.Services;

namespace F1.Teste.Services.GPServices;

public class CreateGPServiceTests
{
    [Fact]
    public async Task AdicionarGPAsync_DeveRetornarReadGpDTO_QuandoGPValido()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IGrandePremioQuery>();
        var mockRepo = new Mock<IRepositoryBase<GrandePremio>>();

        var createGpDTO = new CreateGpDTO { Nome = "GP Teste" };
        var gpCreateMapeado = new GrandePremio { Nome = "GP Teste" };
        var readGpDTO = new ReadGpDTO { Id = 1, Nome = "GP Teste" };

        mockMapper
            .Setup(mapper => mapper.Map<GrandePremio>(createGpDTO))
            .Returns(gpCreateMapeado);

        mockMapper
            .Setup(mapper => mapper.Map<ReadGpDTO>(gpCreateMapeado))
            .Returns(readGpDTO);

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync((GrandePremio?)null);

        mockRepo
            .Setup(repo => repo.AdicionarAsync(gpCreateMapeado))
            .ReturnsAsync(gpCreateMapeado);

        var service = new CreateGrandePremioService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await service.AdicionarGPAsync(createGpDTO);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);

        mockMapper.Verify(mapper => mapper.Map<ReadGpDTO>(gpCreateMapeado), Times.Once());
        mockMapper.Verify(mapper => mapper.Map<GrandePremio>(createGpDTO), Times.Once());

        mockQuery.Verify(query => query.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<GrandePremio, bool>>>(),
            It.IsAny<Expression<Func<GrandePremio, object?>>[]>()), 
            Times.Once());

        mockRepo.Verify(repo => repo.AdicionarAsync(gpCreateMapeado), Times.Once());
    }

    [Fact]
    public async Task AdicionarGPAsync_DeveLancarInvalidOperationException_QuandoGPJaExistente()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IGrandePremioQuery>();
        var mockRepo = new Mock<IRepositoryBase<GrandePremio>>();

        var createGpDTO = new CreateGpDTO { Nome = "GP Já Existente" };
        var gpJaExistente = new GrandePremio { Nome = "GP Já Existente" };

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<GrandePremio, bool>>>(),
                It.IsAny<Expression<Func<GrandePremio, object?>>[]>()))
            .ReturnsAsync(gpJaExistente);

        var service = new CreateGrandePremioService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.AdicionarGPAsync(createGpDTO));

        Assert.Equal("Grande Prêmio já existente.", resultado.Message);
        mockRepo.Verify(repo => repo.AdicionarAsync(gpJaExistente), Times.Never);
    }
}