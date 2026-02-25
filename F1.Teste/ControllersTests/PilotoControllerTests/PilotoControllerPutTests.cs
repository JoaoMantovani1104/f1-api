using Moq;
using F1.API.Controllers;
using AutoFixture.Xunit2;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.PilotoDTO;
using F1.API.Services.PilotoServices.Interfaces;

namespace F1.Teste.ControllersTests.PilotoControllerTests;

public class PilotoControllerPutTests
{
    [Theory, AutoMoqData]
    public async Task AtualizarPiloto_DeveRetornarNoContent_QuandoAtualizarComSucesso(
        int id,
        UpdatePilotoDTO pilotoUpdateDTO,
        [Frozen] Mock<IUpdatePilotoService> mockUpdateService,
        PilotoController sut)
    {
        mockUpdateService
            .Setup(serv => serv.AtualizarPilotoAsync(id, pilotoUpdateDTO))
            .ReturnsAsync(true);

        var resultado = await sut.AtualizarPiloto(id, pilotoUpdateDTO);

        Assert.IsType<NoContentResult>(resultado);
    }

    [Theory, AutoMoqData]
    public async Task AtualizarPiloto_DeveRetornarNotFound_QuandoAtualizacaoFalhar(
        int id,
        UpdatePilotoDTO pilotoUpdateDTO,
        [Frozen] Mock<IUpdatePilotoService> mockUpdateService,
        PilotoController sut)
    {
        mockUpdateService
            .Setup(serv => serv.AtualizarPilotoAsync(id, pilotoUpdateDTO))
            .ReturnsAsync(false);

        var resultado = await sut.AtualizarPiloto(id, pilotoUpdateDTO);

        Assert.IsType<NotFoundResult>(resultado);
    }
}
