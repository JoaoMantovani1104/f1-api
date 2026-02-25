using Moq;
using AutoFixture.Xunit2;
using F1.API.Controllers;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.GrandePremioDTO;
using F1.API.Services.GpServices.Interfaces;

namespace F1.Teste.ControllersTests.GPControllerTests;

public class GpControllerPostTests
{
    [Theory, AutoMoqData]
    public async Task AdicionarGP_DeveRetornarCreatedAtActionComReadGpDTO_QuandoAdicionarComSucesso(
        CreateGpDTO gpCreateDTO,
        ReadGpDTO gpReadDTO,
        [Frozen] Mock<ICreateGrandePremioService> mockCreateService,
        GpController sut)
    {
        gpReadDTO.Nome = gpCreateDTO.Nome;

        mockCreateService
            .Setup(serv => serv.AdicionarGPAsync(gpCreateDTO))
            .ReturnsAsync(gpReadDTO);

        var resultado = await sut.AdicionarGP(gpCreateDTO);

        var createdAtActionResultado = Assert.IsType<CreatedAtActionResult>(resultado);
        var gpCriado = Assert.IsType<ReadGpDTO>(createdAtActionResultado.Value);

        Assert.Equal(gpCreateDTO.Nome, gpCriado.Nome);
    }

    [Theory, AutoMoqData]
    public async Task AdicionarGP_DeveRetornarConflict_QuandoEncontrarGPComMesmoNome(
        CreateGpDTO gpCreateDTO,
        [Frozen] Mock<ICreateGrandePremioService> mockCreateService,
        GpController sut)
    {
        var mensagemErro = "Grande Prêmio já existente.";

        mockCreateService
            .Setup(serv => serv.AdicionarGPAsync(gpCreateDTO))
            .ThrowsAsync(new InvalidOperationException(mensagemErro));

        var resultado = await sut.AdicionarGP(gpCreateDTO);

        var conflictResultado = Assert.IsType<ConflictObjectResult>(resultado);
        Assert.Equal(mensagemErro, conflictResultado.Value);
    }
}
