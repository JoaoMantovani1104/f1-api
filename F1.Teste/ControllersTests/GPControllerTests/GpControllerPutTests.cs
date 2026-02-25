using Moq;
using F1.Teste.Bogus;
using AutoFixture.Xunit2;
using F1.API.Controllers;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.GrandePremioDTO;
using F1.API.Services.GpServices.Interfaces;

namespace F1.Teste.ControllersTests.GPControllerTests;

public class GpControllerPutTests 
{
    [Theory, AutoMoqData]
    public async Task AtualizarGP_DeveRetornarNoContent_QuandoAtualizarComSucesso(
        UpdateGpDTO gpUpdateDTO,
        [Frozen] Mock<IUpdateGrandePremioService> mockUpdateService,
        GpController sut)
    {
        mockUpdateService
            .Setup(serv => serv.AtualizarGPAsync(1, gpUpdateDTO))
            .ReturnsAsync(true);

        var resultado = await sut.AtualizarGP(1, gpUpdateDTO);

        Assert.IsType<NoContentResult>(resultado);
    }

    [Theory, AutoMoqData]
    public async Task AtualizarGP_DeveRetornarNotFound_QuandoAtualizacaoFalhar(
        UpdateGpDTO gpUpdateDTO,
        [Frozen] Mock<IUpdateGrandePremioService> mockUpdateService,
        GpController sut)
    {
        mockUpdateService
            .Setup(serv => serv.AtualizarGPAsync(1, gpUpdateDTO))
            .ReturnsAsync(false);

        var resultado = await sut.AtualizarGP(1, gpUpdateDTO);

        Assert.IsType<NotFoundResult>(resultado);
    }
}
