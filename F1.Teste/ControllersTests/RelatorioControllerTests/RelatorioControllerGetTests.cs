using Moq;
using F1.Teste.Bogus;
using AutoFixture.Xunit2;
using F1.API.Controllers;
using F1.Teste.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.RelatorioDTO;
using F1.API.Services.RelatorioServices.Interfaces;

namespace F1.Teste.ControllersTests.RelatorioControllerTests;

public class RelatorioControllerGetTests
{
    [Theory, AutoMoqData]
    public async Task LerRelatorioGeral_DeveRetornarOkComRelatorio_QuandoChamado(
        [Frozen] Mock<IReadRelatorioService> mockReadService,
        RelatorioController sut)
    {
        var relatorio = RelatorioDTOFaker.Gerar().Generate();

        mockReadService
            .Setup(s => s.LerRelatorioGeralAsync())
            .ReturnsAsync(relatorio);

        var resultado = await sut.LerRelatorioGeral();

        var okResult = Assert.IsType<OkObjectResult>(resultado);
        var retorno = Assert.IsType<RelatorioDTO>(okResult.Value);

        Assert.Equal(relatorio.TotalEquipes, retorno.TotalEquipes);
        Assert.Equal(relatorio.EquipeMaisVencedora, retorno.EquipeMaisVencedora);
    }
}
