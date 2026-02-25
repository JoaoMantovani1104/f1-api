using Moq;
using AutoFixture.Xunit2;
using F1.API.Controllers;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Services.EquipeServices.Interfaces;
using F1.API.Data.Dtos.EquipeDTO;

namespace F1.Teste.ControllersTests.EquipeControllerTests;

public class EquipeControllerPutTests 
{
    [Theory, AutoMoqData]
    public async Task AtualizarEquipe_DeveRetornarNoContent_QuandoAtualizarComSucesso(
        UpdateEquipeDTO equipeUpdateDTO,
        [Frozen] Mock<IUpdateEquipeService> mockUpdateService,
        EquipeController sut)
    {
        mockUpdateService
            .Setup(serv => serv.AtualizarEquipeAsync(1, equipeUpdateDTO))
            .ReturnsAsync(true);

        var resultado = await sut.AtualizarEquipe(1, equipeUpdateDTO);

        Assert.IsType<NoContentResult>(resultado);
    }

    [Theory, AutoMoqData]
    public async Task AtualizarEquipe_DeveRetornarNotFound_QuandoAtualizacaoFalhar(
        UpdateEquipeDTO equipeUpdateDTO,
        [Frozen] Mock<IUpdateEquipeService> mockUpdateService,
        EquipeController sut)
    {
        mockUpdateService
            .Setup(serv => serv.AtualizarEquipeAsync(1, equipeUpdateDTO))
            .ReturnsAsync(false);

        var resultado = await sut.AtualizarEquipe(1, equipeUpdateDTO);

        Assert.IsType<NotFoundResult>(resultado);
    }
}
