using Moq;
using F1.API.Controllers;
using AutoFixture.Xunit2;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Services.PilotoServices.Interfaces;

namespace F1.Teste.ControllersTests.PilotoControllerTests;

public class PilotoControllerDeleteTests 
{
    [Theory, AutoMoqData]
    public async Task DeletarPiloto_DeveRetornarNotFound_QuandoDelecaoFalhar(
        [Frozen] Mock<IDeletePilotoService> mockDeleteService,
        PilotoController sut)
    {
        mockDeleteService
            .Setup(serv => serv.DeletarPilotoAsync(1))
            .ReturnsAsync(false);

        var resultado = await sut.DeletarPiloto(1);

        Assert.IsType<NotFoundResult>(resultado);
    }

    [Theory, AutoMoqData]
    public async Task DeletarPiloto_DeveRetornarNoContent_QuandoDeletarComSucesso(
        [Frozen] Mock<IDeletePilotoService> mockDeleteService,
        PilotoController sut)
    {
        mockDeleteService
            .Setup(serv => serv.DeletarPilotoAsync(1))
            .ReturnsAsync(true);

        var resultado = await sut.DeletarPiloto(1);

        Assert.IsType<NoContentResult>(resultado);
    }
}
