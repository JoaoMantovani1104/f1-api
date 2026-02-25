using Moq;
using AutoFixture.Xunit2;
using F1.API.Controllers;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.EquipeDTO;
using F1.API.Services.EquipeServices.Interfaces;

namespace F1.Teste.ControllersTests.EquipeControllerTests;

public class EquipeControllerPostTests
{
    [Theory, AutoMoqData]
    public async Task AdicionarEquipe_DeveRetornarCreatedAtAction_QuandoNaoEncontrarEquipeComMesmoNome(
        CreateEquipeDTO equipeCreateDTO,
        ReadEquipeDTO equipeReadDTO,
        [Frozen] Mock<ICreateEquipeService> mockCreateService,
        EquipeController sut)
    {
        mockCreateService
            .Setup(serv => serv.AdicionarEquipeAsync(equipeCreateDTO))
            .ReturnsAsync(equipeReadDTO);

        var resultado = await sut.AdicionarEquipe(equipeCreateDTO);

        var createdAtActionResultado = Assert.IsType<CreatedAtActionResult>(resultado);
        var equipeCriada = Assert.IsType<ReadEquipeDTO>(createdAtActionResultado.Value);
        
        Assert.Equal(equipeReadDTO, equipeCriada);
    }

    [Theory, AutoMoqData]
    public async Task AdicionarEquipe_DeveRetornarConflict_QuandoEncontrarEquipeComMesmoNome(
        CreateEquipeDTO equipeCreateDTO,
        [Frozen] Mock<ICreateEquipeService> mockCreateService,
        EquipeController sut)
    {
        var mensagemErro = $"Equipe com nome '{equipeCreateDTO.Nome}' já existente.";

        mockCreateService
            .Setup(serv => serv.AdicionarEquipeAsync(equipeCreateDTO))
            .ThrowsAsync(new InvalidOperationException(mensagemErro));

        var resultado = await sut.AdicionarEquipe(equipeCreateDTO);

        var conflictResultado = Assert.IsType<ConflictObjectResult>(resultado);

        Assert.Equal(conflictResultado.Value, mensagemErro);
    }
}
