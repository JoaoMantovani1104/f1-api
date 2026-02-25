using Moq;
using F1.API.Controllers;
using AutoFixture.Xunit2;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.PilotoDTO;
using F1.API.Services.PilotoServices.Interfaces;

namespace F1.Teste.ControllersTests.PilotoControllerTests;

public class PilotoControllerPostTests
{
    [Theory, AutoMoqData]
    public async Task AdicionarPiloto_DeveRetornarCreatedAtAction_QuandoAdicionarComSucesso(
        CreatePilotoDTO pilotoCreateDTO, ReadPilotoDTO pilotoReadDTO,
        [Frozen] Mock<ICreatePilotoService> mockCreateService,
        PilotoController sut)
    {
        pilotoReadDTO.Nome = pilotoCreateDTO.Nome;
        
        mockCreateService
            .Setup(serv => serv.AdicionarPilotoAsync(pilotoCreateDTO))
            .ReturnsAsync(pilotoReadDTO);

        var resultado = await sut.AdicionarPiloto(pilotoCreateDTO);

        var createdAtActionResultado = Assert.IsType<CreatedAtActionResult>(resultado);
        var pilotoCriado = Assert.IsType<ReadPilotoDTO>(createdAtActionResultado.Value);

        Assert.Equal(pilotoReadDTO, pilotoCriado);
    }

    [Theory, AutoMoqData]
    public async Task AdicionarPiloto_DeveRetornarConflict_QuandoAdicaoFalhar(
        CreatePilotoDTO pilotoCreateDTO,
        [Frozen] Mock<ICreatePilotoService> mockCreateService,
        PilotoController sut)
    {
        var mensagemErro = $"Piloto com o número {pilotoCreateDTO.Numero} já existente";

        mockCreateService
            .Setup(serv => serv.AdicionarPilotoAsync(pilotoCreateDTO))
            .ThrowsAsync(new InvalidOperationException(mensagemErro));

        var resultado = await sut.AdicionarPiloto(pilotoCreateDTO);

        var conflictResultado = Assert.IsType<ConflictObjectResult>(resultado);
        Assert.Equal(mensagemErro, conflictResultado.Value);
    }
}
