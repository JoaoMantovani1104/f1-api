using F1.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using F1.API.Services.Relatorio.Services;

namespace F1.API.Controllers;

[ApiController]
[Route("/Relatorio")]
public class RelatorioController : ControllerBase
{
    private readonly F1Context context;
    private readonly ReadRelatorioService serviceRead;

    public RelatorioController(F1Context context, ReadRelatorioService serviceRead)
    {
        this.context = context;
        this.serviceRead = serviceRead;
    }

    [HttpGet("geral")]
    public async Task<IActionResult> LerRelatorioGeral()
    {
        var relatorio = serviceRead.LerRelatorioGeralAsync();

        return Ok(relatorio);
    }
}
