using Moq;
using AutoFixture.Xunit2;
using F1.API.Controllers;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Services.EquipeServices.Interfaces;

namespace F1.Teste.ControllersTests.EquipeControllerTests;

public class EquipeControllerDeleteTests
{
    [Theory, AutoMoqData]
    public async Task DeletarEquipe_DeveRetornarNotFound_QuandoDelecaoFalhar(
        [Frozen] Mock<IDeleteEquipeService> mockDeleteService,
        EquipeController sut)
    {
        mockDeleteService
            .Setup(serv => serv.DeletarEquipeAsync(1))
            .ReturnsAsync(false);

        var resultado = await sut.DeletarEquipe(1);

        Assert.IsType<NotFoundResult>(resultado);
    }

    [Theory, AutoMoqData]
    public async Task DeletarEquipe_DeveRetornarNoContent_QuandoDeletarComSucesso(
        [Frozen] Mock<IDeleteEquipeService> mockDeleteService,
        EquipeController sut)
    {
        mockDeleteService
            .Setup(serv => serv.DeletarEquipeAsync(1))
            .ReturnsAsync(true);

        var resultado = await sut.DeletarEquipe(1);

        Assert.IsType<NoContentResult>(resultado);
    }

    [Theory, AutoMoqData]
    public async Task DeletarEquipe_DeveRetornarBadRequest_QuandoHouverException(
        [Frozen] Mock<IDeleteEquipeService> mockDeleteService,
        EquipeController sut)
    {
        var mensagemErro = "Não é possível deletar uma equipe que possui pilotos associados.";

        mockDeleteService
            .Setup(serv => serv.DeletarEquipeAsync(1))
            .ThrowsAsync(new InvalidOperationException(mensagemErro));

        var resultado = await sut.DeletarEquipe(1);

        var badRequestResultado = Assert.IsType<BadRequestObjectResult>(resultado);
        Assert.Equal(mensagemErro, badRequestResultado.Value);
    }
}
