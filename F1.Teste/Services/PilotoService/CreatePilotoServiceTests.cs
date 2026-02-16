using Moq;
using AutoMapper;
using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using System.Linq.Expressions;
using F1.API.Data.Dtos.PilotoDTO;
using F1.API.Services.PilotoServices.Services;

namespace F1.Teste.Services.PilotoService;

public class CreatePilotoServiceTests
{
    [Fact]
    public async Task AdicionarPilotoAsync_DeveLancarInvalidOperationException_QuandoJaExistePilotoComMesmoNumero()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IPilotoQuery>();
        var mockRepo = new Mock<IRepositoryBase<Piloto>>();

        var pilotoCreateDTO = new CreatePilotoDTO { Nome = "Piloto Novo", Numero = 10 };
        var pilotoComMesmoNumero = new Piloto { Nome = "Piloto Teste", Numero = 10 };
        var listaPilotos = new List<Piloto> { pilotoComMesmoNumero };

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

        var service = new CreatePilotoService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.AdicionarPilotoAsync(pilotoCreateDTO));

        Assert.Equal($"Piloto com o número {pilotoCreateDTO.Numero} já existente", resultado.Message);
        
        mockQuery.Verify(query => query.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<Piloto>()), Times.Never());
    }

    [Fact]
    public async Task AdicionarPilotoAsync_DeveRetornarReadPilotoDTO_QuandoPilotoValido()
    {
        var mockMapper = new Mock<IMapper>();
        var mockQuery = new Mock<IPilotoQuery>();
        var mockRepo = new Mock<IRepositoryBase<Piloto>>();

        var pilotoCreateDTO = new CreatePilotoDTO { Nome = "Piloto Novo" };
        var pilotoReadDTO = new ReadPilotoDTO { Id = 1, Nome = "Piloto Novo" };
        var piloto = new Piloto { Nome = "Piloto Novo" };

        mockMapper
            .Setup(mapper => mapper.Map<Piloto>(pilotoCreateDTO))
            .Returns(piloto);

        mockMapper
            .Setup(mapper => mapper.Map<ReadPilotoDTO>(piloto))
            .Returns(pilotoReadDTO);

        mockQuery
            .Setup(query => query.BuscarPorCampoAsync(
                It.IsAny<Expression<Func<Piloto, bool>>>(),
                It.IsAny<Expression<Func<Piloto, object?>>[]>()))
            .ReturnsAsync((Piloto?)null);

        mockRepo
            .Setup(repo => repo.AdicionarAsync(piloto))
            .ReturnsAsync(piloto);

        var service = new CreatePilotoService(mockMapper.Object, mockQuery.Object, mockRepo.Object);

        var resultado = await service.AdicionarPilotoAsync(pilotoCreateDTO);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);

        mockMapper.Verify(mapper => mapper.Map<Piloto>(pilotoCreateDTO), Times.Once());
        mockMapper.Verify(mapper => mapper.Map<ReadPilotoDTO>(piloto), Times.Once());
        mockQuery.Verify(query => query.BuscarPorCampoAsync(
            It.IsAny<Expression<Func<Piloto, bool>>>(),
            It.IsAny<Expression<Func<Piloto, object?>>[]>()), Times.Once());
        mockRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<Piloto>()), Times.Once());
    }
}
