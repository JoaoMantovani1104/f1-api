using Moq;
using AutoFixture.Xunit2;
using F1.API.Controllers;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Services.GpServices.Interfaces;

namespace F1.Teste.ControllersTests.GPControllerTests;

public class GpControllerDeleteTests
{
    [Theory, AutoMoqData]
    public async Task DeletarGP_DeveRetornarNotFound_QuandoDelecaoFalhar(
        [Frozen] Mock<IDeleteGrandePremioService> mockDeleteService,
        GpController sut)
    {
        mockDeleteService
            .Setup(serv => serv.DeletarGPAsync(1))
            .ReturnsAsync(false);

        var resultado = await sut.DeletarGP(1);

        Assert.IsType<NotFoundResult>(resultado);
    }

    [Theory, AutoMoqData]
    public async Task DeletarGP_DeveRetornarNoContent_QuandoDeletarComSucesso(
        [Frozen] Mock<IDeleteGrandePremioService> mockDeleteService,
        GpController sut)
    {
        mockDeleteService
            .Setup(serv => serv.DeletarGPAsync(1))
            .ReturnsAsync(true);

        var resultado = await sut.DeletarGP(1);

        Assert.IsType<NoContentResult>(resultado);
    }

    [Theory, AutoMoqData]
    public async Task DeletarGP_DeveRetornarBadRequest_QuandoDelecaoLancarExcessao(
        [Frozen] Mock<IDeleteGrandePremioService> mockDeleteService,
        GpController sut)
    {
        var mensagemErro = $"Não é possível deletar Grandes Prêmios com vencedores associados.";

        mockDeleteService
            .Setup(serv => serv.DeletarGPAsync(1))
            .ThrowsAsync(new InvalidOperationException(mensagemErro));

        var resultado = await sut.DeletarGP(1);

        var badRequestResultado = Assert.IsType<BadRequestObjectResult>(resultado);
        Assert.Equal(mensagemErro, badRequestResultado.Value);
    }
}
