using Moq;
using AutoFixture.Xunit2;
using F1.API.Controllers;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.GrandePremioDTO;
using F1.API.Services.GpServices.Interfaces;

namespace F1.Teste.ControllersTests.GPControllerTests;

public class GpControllerGetTests
{
    [Theory, AutoMoqData]
    public async Task LerGPs_DeveRetornarOkComListaDeGPsCadastrados_QuandoChamado(
        IEnumerable<ReadGpDTO> listaGPs,
        [Frozen] Mock<IReadGrandePremioService> mockReadService,
        GpController sut)
    {
        mockReadService
            .Setup(serv => serv.LerGPsAsync())
            .ReturnsAsync(listaGPs);

        var resultado = await sut.LerGPs();

        var okResultado = Assert.IsType<OkObjectResult>(resultado);
        var listaRetornada = Assert.IsAssignableFrom<IEnumerable<ReadGpDTO>>(okResultado.Value);

        Assert.Equal(listaGPs, listaRetornada);
        Assert.Equal(3, listaRetornada.Count());
    }

    [Theory, AutoMoqData]
    public async Task LerGPPorId_DeveRetornarOkComGPBuscado_QuandoEncontrarGP(
        ReadGpDTO gpReadDTO,
        [Frozen] Mock<IReadGrandePremioService> mockReadService,
        GpController sut)
    {
        mockReadService
            .Setup(serv => serv.LerGPPorIdAsync(gpReadDTO.Id))
            .ReturnsAsync(gpReadDTO);

        var resultado = await sut.LerGPPorId(gpReadDTO.Id);

        var okResultado = Assert.IsType<OkObjectResult>(resultado);
        var gpRetornado = Assert.IsType<ReadGpDTO>(okResultado.Value);

        Assert.Equal(gpReadDTO, gpRetornado);
    }

    [Theory, AutoMoqData]
    public async Task LerGPPorId_DeveRetornarNotFound_QuandoNaoEncontrarGP(
        [Frozen] Mock<IReadGrandePremioService> mockReadService,
        GpController sut)
    {
        mockReadService
            .Setup(serv => serv.LerGPPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync((ReadGpDTO?)null);

        var resultado = await sut.LerGPPorId(1);

        Assert.IsType<NotFoundResult>(resultado);
        }
}