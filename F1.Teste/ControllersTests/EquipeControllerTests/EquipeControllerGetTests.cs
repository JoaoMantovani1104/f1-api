using Moq;
using AutoFixture.Xunit2;
using F1.API.Controllers;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.EquipeDTO;
using F1.API.Services.EquipeServices.Interfaces;

namespace F1.Teste.ControllersTests.EquipeControllerTests;

public class EquipeControllerGetTests 
{
    [Theory, AutoMoqData]
    public async Task LerEquipes_DeveRetornarOkComListaDeEquipes_QuandoChamado(
        IEnumerable<ReadEquipeDTO> listaEquipes,
        [Frozen] Mock<IReadEquipeService> mockReadService,
        EquipeController sut)
    {
        mockReadService
            .Setup(serv => serv.LerEquipesAsync())
            .ReturnsAsync(listaEquipes);

        var resultado = await sut.LerEquipes();

        var okResultado = Assert.IsType<OkObjectResult>(resultado);
        var listaRetornada = Assert.IsAssignableFrom<IEnumerable<ReadEquipeDTO>>(okResultado.Value);

        Assert.Equal(listaEquipes, listaRetornada);
        Assert.Equal(3, listaRetornada.Count());
    }

    [Theory, AutoMoqData]
    public async Task LerEquipesPorId_DeveRetornarOkComEquipeBuscada_QuandoEncontrarEquipeBuscada(
        ReadEquipeDTO equipeReadDTO,
        [Frozen] Mock<IReadEquipeService> mockReadService,
        EquipeController sut)
    {
        mockReadService
            .Setup(serv => serv.LerEquipePorIdAsync(equipeReadDTO.Id))
            .ReturnsAsync(equipeReadDTO);

        var resultado = await sut.LerEquipePorId(equipeReadDTO.Id);

        var okResultado = Assert.IsType<OkObjectResult>(resultado);
        var equipeRetornada = Assert.IsType<ReadEquipeDTO>(okResultado.Value);

        Assert.Equal(equipeReadDTO, equipeRetornada);
    }

    [Theory, AutoMoqData]
    public async Task LerEquipesPorId_DeveRetornarNotFound_QuandoNaoEncontrarEquipeBuscada(
        [Frozen] Mock<IReadEquipeService> mockReadService,
        EquipeController sut)
    {
        mockReadService
            .Setup(serv => serv.LerEquipePorIdAsync(1))
            .ReturnsAsync((ReadEquipeDTO?)null);

        var resultado = await sut.LerEquipePorId(1);

        Assert.IsType<NotFoundResult>(resultado);
    }
}
