using Moq;
using AutoMapper;
using AutoFixture.Xunit2;
using F1.API.Controllers;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.EquipeDTO;
using Microsoft.AspNetCore.JsonPatch;
using F1.API.Services.EquipeServices.Interfaces;

namespace F1.Teste.ControllersTests.EquipeControllerTests;

public class EquipeControllerPatchTests
{
    [Theory, AutoMoqData]
    public async Task AtualizarEquipeParcial_DeveRetornarNotFound_QuandoNaoEncontrarEquipeDesejada(
        JsonPatchDocument<UpdateEquipeDTO> patch,
        [Frozen] Mock<IReadEquipeService> mockReadService,
        EquipeController sut)
    {
        mockReadService
            .Setup(serv => serv.LerEquipePorIdAsync(It.IsAny<int>()))
            .ReturnsAsync((ReadEquipeDTO?)null);

        var resultado = await sut.AtualizarEquipeParcial(1, patch);

        Assert.IsType<NotFoundResult>(resultado);
    }

    [Theory, AutoMoqData]
    public async Task AtualizarEquipeParcial_DeveRetornarNotFound_QuandoAtualizacaoFalhar(
        ReadEquipeDTO equipeExistente,
        UpdateEquipeDTO equipeAAtualizar,
        JsonPatchDocument<UpdateEquipeDTO> patch,
        [Frozen] Mock<IReadEquipeService> mockReadService,
        [Frozen] Mock<IMapper> mockMapper,
        [Frozen] Mock<IUpdateEquipeService> mockUpdateService,
        EquipeController sut)
    {
        equipeAAtualizar.Nome = equipeExistente.Nome;

        mockReadService
            .Setup(s => s.LerEquipePorIdAsync(equipeExistente.Id))
            .ReturnsAsync(equipeExistente);

        mockMapper
            .Setup(m => m.Map<UpdateEquipeDTO>(equipeExistente))
            .Returns(equipeAAtualizar);

        mockUpdateService
            .Setup(s => s.AtualizarEquipeAsync(equipeExistente.Id, equipeAAtualizar))
            .ReturnsAsync(false);

        var resultado = await sut.AtualizarEquipeParcial(equipeExistente.Id, patch);

        Assert.IsType<NotFoundResult>(resultado);
    }

    [Theory, AutoMoqData]
    public async Task AtualizarEquipeParcial_DeveRetornarNoContent_QuandoAtualizarComSucesso(
        ReadEquipeDTO equipeExistente,
        UpdateEquipeDTO equipeAAtualizar,
        string nomeAtualizado,
        [Frozen] Mock<IReadEquipeService> mockReadService,
        [Frozen] Mock<IMapper> mockMapper,
        [Frozen] Mock<IUpdateEquipeService> mockUpdateService,
        EquipeController sut)
    {
        equipeAAtualizar.Nome = equipeExistente.Nome;

        var patch = new JsonPatchDocument<UpdateEquipeDTO>();
        patch.Replace(e => e.Nome, nomeAtualizado);

        mockReadService
            .Setup(serv => serv.LerEquipePorIdAsync(equipeExistente.Id))
            .ReturnsAsync(equipeExistente);

        mockMapper
            .Setup(mapper => mapper.Map<UpdateEquipeDTO>(equipeExistente))
            .Returns(equipeAAtualizar);

        mockUpdateService
            .Setup(serv => serv.AtualizarEquipeAsync(equipeExistente.Id, equipeAAtualizar))
            .ReturnsAsync(true);

        var resultado = await sut.AtualizarEquipeParcial(equipeExistente.Id, patch);

        Assert.IsType<NoContentResult>(resultado);

        mockMapper.Verify(mapper => mapper.Map<UpdateEquipeDTO>(equipeExistente), Times.Once());
        mockUpdateService.Verify(serv => serv.AtualizarEquipeAsync(
            equipeExistente.Id, 
            It.Is<UpdateEquipeDTO>(dto => dto.Nome.Equals(nomeAtualizado))), Times.Once());
    }
}
