using Moq;
using F1.API.Controllers;
using AutoFixture.Xunit2;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.PilotoDTO;
using F1.API.Services.PilotoServices.Interfaces;

namespace F1.Teste.ControllersTests.PilotoControllerTests;

public class PilotoControllerGetTests
{
    [Theory, AutoMoqData]
    public async Task LerPilotos_DeveRetornarOkComListaPilotos_QuandoChamado(
        IEnumerable<ReadPilotoDTO> listaPilotos,
        [Frozen] Mock<IReadPilotoService> mockReadService,
        PilotoController sut)
    {
        mockReadService
            .Setup(serv => serv.LerPilotosAsync())
            .ReturnsAsync(listaPilotos);

        var resultado = await sut.LerPilotos();

        var okResultado = Assert.IsType<OkObjectResult>(resultado);
        var pilotosRetornados = Assert.IsAssignableFrom<IEnumerable<ReadPilotoDTO>>(okResultado.Value);

        Assert.Equal(listaPilotos, pilotosRetornados);
        Assert.Equal(3, pilotosRetornados.Count());
    }

    [Theory, AutoMoqData]
    public async Task LerPilotoPorId_DeveRetornarOkComPilotoBuscado_QuandoEncontrarPiloto(
        ReadPilotoDTO piloto,
        [Frozen] Mock<IReadPilotoService> mockReadService,
        PilotoController sut)
    {
        mockReadService
            .Setup(serv => serv.LerPilotoPorIdAsync(piloto.Id))
            .ReturnsAsync(piloto);

        var resultado = await sut.LerPilotoPorId(piloto.Id);

        var okResultado = Assert.IsType<OkObjectResult>(resultado);
        var pilotoRetornado = Assert.IsType<ReadPilotoDTO>(okResultado.Value);
        Assert.Equal(piloto, pilotoRetornado);
    }

    [Theory, AutoMoqData]
    public async Task LerPilotoPorId_DeveRetornarNotFound_QuandoNaoEncontrarPiloto(
        int id,
        [Frozen] Mock<IReadPilotoService> mockReadService,
        PilotoController sut)
    {
        mockReadService
            .Setup(serv => serv.LerPilotoPorIdAsync(id))
            .ReturnsAsync((ReadPilotoDTO?)null);

        var resultado = await sut.LerPilotoPorId(id);

        Assert.IsType<NotFoundResult>(resultado);
    }
}
