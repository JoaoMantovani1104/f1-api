using Microsoft.AspNetCore.Mvc;
using F1.API.Services.RelatorioServices.Interfaces;

namespace F1.API.Controllers;

[ApiController]
[Route("/Relatorio")]
public class RelatorioController : ControllerBase
{
    private readonly IReadRelatorioService serviceRead;

    public RelatorioController(IReadRelatorioService serviceRead)
    {
        this.serviceRead = serviceRead;
    }

    [HttpGet("geral")]
    public async Task<IActionResult> LerRelatorioGeral()
    {
        var relatorio = await serviceRead.LerRelatorioGeralAsync();

        return Ok(relatorio);
    }
}
