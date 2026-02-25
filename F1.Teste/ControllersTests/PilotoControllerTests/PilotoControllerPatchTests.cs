using Moq;
using AutoMapper;
using F1.API.Controllers;
using AutoFixture.Xunit2;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.PilotoDTO;
using Microsoft.AspNetCore.JsonPatch;
using F1.API.Services.PilotoServices.Interfaces;

namespace F1.Teste.ControllersTests.PilotoControllerTests;

public class PilotoControllerPatchTests
{
    [Theory, AutoMoqData]
    public async Task AtualizarPartePiloto_DeveRetornarNotFound_QuandoNaoEncontrarPiloto(
        int id,
        JsonPatchDocument<UpdatePilotoDTO> patch,
        [Frozen] Mock<IReadPilotoService> mockReadService,
        PilotoController sut)
    {
        mockReadService
            .Setup(serv => serv.LerPilotoPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync((ReadPilotoDTO?)null);

        var resultado = await sut.AtualizarPartePiloto(id, patch);

        Assert.IsType<NotFoundResult>(resultado);
    }

    [Theory, AutoMoqData]
    public async Task AtualizarPartePiloto_DeveRetornarNotFound_QuandoAtualizarFalharNaPersistencia(
        ReadPilotoDTO pilotoReadDTO,
        UpdatePilotoDTO pilotoUpdateDTO,
        JsonPatchDocument<UpdatePilotoDTO> patch,
        [Frozen] Mock<IReadPilotoService> mockReadService,
        [Frozen] Mock<IMapper> mockMapper,
        [Frozen] Mock<IUpdatePilotoService> mockUpdateService,
        PilotoController sut)
    {
        mockReadService
            .Setup(s => s.LerPilotoPorIdAsync(pilotoReadDTO.Id))
            .ReturnsAsync(pilotoReadDTO);

        mockMapper
            .Setup(m => m.Map<UpdatePilotoDTO>(pilotoReadDTO))
            .Returns(pilotoUpdateDTO);

        mockUpdateService
            .Setup(s => s.AtualizarPilotoAsync(pilotoReadDTO.Id, pilotoUpdateDTO))
            .ReturnsAsync(false);

        var resultado = await sut.AtualizarPartePiloto(pilotoReadDTO.Id, patch);

        Assert.IsType<NotFoundResult>(resultado);
    }

    [Theory, AutoMoqData]
    public async Task AtualizarPartePiloto_DeveRetornarNoContent_QuandoAtualizarComSucesso(
        ReadPilotoDTO pilotoExistente,
        UpdatePilotoDTO pilotoAAtualizar,
        string novoNomePatch,
        [Frozen] Mock<IReadPilotoService> mockReadService,
        [Frozen] Mock<IMapper> mockMapper,
        [Frozen] Mock<IUpdatePilotoService> mockUpdateService,
        PilotoController sut)
    {
        pilotoAAtualizar.Nome = pilotoExistente.Nome;

        var patch = new JsonPatchDocument<UpdatePilotoDTO>();
        patch.Replace(p => p.Nome, novoNomePatch);

        mockReadService
            .Setup(serv => serv.LerPilotoPorIdAsync(pilotoExistente.Id))
            .ReturnsAsync(pilotoExistente);

        mockMapper
            .Setup(mapper => mapper.Map<UpdatePilotoDTO>(pilotoExistente))
            .Returns(pilotoAAtualizar);

        mockUpdateService
            .Setup(s => s.AtualizarPilotoAsync(pilotoExistente.Id, pilotoAAtualizar))
            .ReturnsAsync(true);

        var resultado = await sut.AtualizarPartePiloto(pilotoExistente.Id, patch);

        Assert.IsType<NoContentResult>(resultado);

        mockMapper.Verify(mapper => mapper.Map<UpdatePilotoDTO>(pilotoExistente), Times.Once());
        mockUpdateService.Verify(serv => serv.AtualizarPilotoAsync(
            pilotoExistente.Id,
            It.Is<UpdatePilotoDTO>(dto => dto.Nome.Equals(novoNomePatch))), Times.Once());
    }
}
