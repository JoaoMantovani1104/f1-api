using Moq;
using AutoMapper;
using F1.Lib.Modelos;
using System.Linq.Expressions;
using F1.API.Data.Dtos.EquipeDTO;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.EquipeServices.Services;

namespace F1.Teste.Services.EquipeServices;

public class CreateGPServiceTests 
{
    [Fact]
    public async Task AdicionarEquipeAsync_DeveRetornarReadEquipeDTO_QuandoEquipeValida()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IEquipeQuery>();
        var mockRepo = new Mock<IRepositoryBase<Equipe>>();

        var equipeCreateMapeada = new Equipe { Nome = "Equipe Teste" };
        var equipeCreateDTO = new CreateEquipeDTO { Nome = "Equipe Teste" };
        var equipeReadDTO = new ReadEquipeDTO { Id = 1, Nome = "Equipe Teste" };

        mockMapper
            .Setup(mapper => mapper.Map<Equipe>(equipeCreateDTO))
            .Returns(equipeCreateMapeada);

        mockMapper
            .Setup(mapper => mapper.Map<ReadEquipeDTO>(equipeCreateMapeada))
            .Returns(equipeReadDTO);

        mockRepo
            .Setup(repo => repo.AdicionarAsync(equipeCreateMapeada))
            .ReturnsAsync(equipeCreateMapeada);

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync((Equipe?)null);

        var serviceCreate = new CreateEquipeService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await serviceCreate.AdicionarEquipeAsync(equipeCreateDTO);

        Assert.Equal(1, resultado.Id);

        mockMapper.Verify(mapper => mapper.Map<Equipe>(equipeCreateDTO), Times.Once());
        mockMapper.Verify(mapper => mapper.Map<ReadEquipeDTO>(equipeCreateMapeada), Times.Once());
        mockQuery.Verify(query => query.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Equipe, bool>>>(),
            It.IsAny<Expression<Func<Equipe, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<Equipe>()), Times.Once);
    }

    [Fact]
    public async Task AdicionarEquipeAsync_DeveLancarInvalidOperationException_QuandoEquipeJaExistente()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IEquipeQuery>();
        var mockRepo = new Mock<IRepositoryBase<Equipe>>();

        var equipeEntrada = new Equipe { Id = 1, Nome = "Equipe Já Existe" };
        var equipeAAdicionar = new CreateEquipeDTO { Nome = "Equipe Já Existe" };

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Equipe, bool>>>(),
                It.IsAny<Expression<Func<Equipe, object?>>[]>()))
            .ReturnsAsync(equipeEntrada);

        var serviceCreate = new CreateEquipeService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => serviceCreate.AdicionarEquipeAsync(equipeAAdicionar));

        mockRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<Equipe>()), Times.Never);
        Assert.Equal($"Equipe com nome '{equipeAAdicionar.Nome}' já existente.", resultado.Message);
    }
}