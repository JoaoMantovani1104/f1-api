using Moq;
using AutoMapper;
using AutoFixture.Xunit2;
using F1.API.Controllers;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using F1.API.Data.Dtos.GrandePremioDTO;
using F1.API.Services.GpServices.Interfaces;

namespace F1.Teste.ControllersTests.GPControllerTests;

public class GpControllerPatchTests
{
    [Theory, AutoMoqData]
    public async Task AtualizarParteGP_DeveRetornarNotFound_QuandoNaoEncontrarGP(
        JsonPatchDocument<UpdateGpDTO> patch,
        [Frozen] Mock<IReadGrandePremioService> mockReadService,
        GpController sut)
    {
        mockReadService
            .Setup(serv => serv.LerGPPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync((ReadGpDTO?)null);

        var resultado = await sut.AtualizarParteGP(1, patch);

        Assert.IsType<NotFoundResult>(resultado);
    }

    [Theory, AutoMoqData]
    public async Task AtualizarParteGP_DeveRetornarNotFound_QuandoAtualicaoFalhar(
        [Frozen] Mock<IReadGrandePremioService> mockReadService,
        [Frozen] Mock<IMapper> mockMapper,
        [Frozen] Mock<IUpdateGrandePremioService> mockUpdateService,
        GpController sut)
    {
        mockReadService
            .Setup(serv => serv.LerGPPorIdAsync(1))
            .ReturnsAsync(new ReadGpDTO());

        mockMapper
            .Setup(mapper => mapper.Map<UpdateGpDTO>(It.IsAny<ReadGpDTO>()))
            .Returns(new UpdateGpDTO());

        mockUpdateService
            .Setup(serv => serv.AtualizarGPAsync(1, It.IsAny<UpdateGpDTO>()))
            .ReturnsAsync(false);

        var resultado = await sut.AtualizarParteGP(1, new JsonPatchDocument<UpdateGpDTO>());

        Assert.IsType<NotFoundResult>(resultado);
    }

    [Theory, AutoMoqData]
    public async Task AtualizarParteGP_DeveRetornarNoContent_QuandoAtualizarComSucesso(
        ReadGpDTO gpExistente,
        UpdateGpDTO gpAAtualizar,
        string nomeAtualizado,
        [Frozen] Mock<IReadGrandePremioService> mockReadService,
        [Frozen] Mock<IMapper> mockMapper,
        [Frozen] Mock<IUpdateGrandePremioService> mockUpdateService,
        GpController sut)
    {
        var patch = new JsonPatchDocument<UpdateGpDTO>();
        patch.Replace(gp => gp.Nome, nomeAtualizado);

        mockReadService
            .Setup(serv => serv.LerGPPorIdAsync(1))
            .ReturnsAsync(gpExistente);

        mockMapper
            .Setup(mapper => mapper.Map<UpdateGpDTO>(gpExistente))
            .Returns(gpAAtualizar);

        mockUpdateService
            .Setup(serv => serv.AtualizarGPAsync(1, It.IsAny<UpdateGpDTO>()))
            .ReturnsAsync(true);

        var resultado = await sut.AtualizarParteGP (1, patch);

        Assert.IsType<NoContentResult>(resultado);

        mockMapper.Verify(mapper => mapper.Map<UpdateGpDTO>(gpExistente), Times.Once());
        mockUpdateService.Verify(serv => serv.AtualizarGPAsync(1, 
            It.Is<UpdateGpDTO>(dto => dto.Nome.Equals(nomeAtualizado))), Times.Once());
    }
}
