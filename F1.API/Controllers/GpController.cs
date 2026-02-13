using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using F1.API.Data.Dtos.GrandePremioDTO;
using F1.API.Services.GpServices.Interfaces;

namespace F1.API.Controllers;

[ApiController]
[Route("/GPs")]
public class GpController : ControllerBase
{
    private IMapper mapper;
    private readonly ICreateGrandePremioService serviceCreate;
    private readonly IReadGrandePremioService serviceRead;
    private readonly IUpdateGrandePremioService serviceUpdate;
    private readonly IDeleteGrandePremioService serviceDelete;

    public GpController(ICreateGrandePremioService serviceCreate, IReadGrandePremioService serviceRead,
        IUpdateGrandePremioService serviceUpdate, IDeleteGrandePremioService serviceDelete, IMapper mapper)
    {
        this.serviceCreate = serviceCreate;
        this.serviceRead = serviceRead;
        this.serviceUpdate = serviceUpdate;
        this.serviceDelete = serviceDelete;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<ReadGpDTO>> LerGPs()
    {
        var listaGrandesPremios = await serviceRead.LerGPsAsync();

        return listaGrandesPremios;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> LerGPPorId(int id)
    {
        var grandePremio = await serviceRead.LerGPPorIdAsync(id);

        return Ok(grandePremio);
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarGP([FromBody] CreateGpDTO grandePremioDTO)
    {
        try
        {
            var grandePremio = await serviceCreate.AdicionarGPAsync(grandePremioDTO);

            return CreatedAtAction(nameof(LerGPPorId), new { id = grandePremio.Id }, grandePremio);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarGP(int id, [FromBody] UpdateGpDTO grandePremioDTO)
    {
        bool grandePremioAtualizadoComSucesso = await serviceUpdate.AtualizarGPAsync(id, grandePremioDTO);

        if (!grandePremioAtualizadoComSucesso) return NotFound();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> AtualizarParteGP(int id, [FromBody] JsonPatchDocument<UpdateGpDTO> patch)
    {
        var gpExistente = await serviceRead.LerGPPorIdAsync(id);

        if (gpExistente is null) return NotFound();

        var gpParaAtualizar = mapper.Map<UpdateGpDTO>(gpExistente);
        patch.ApplyTo(gpParaAtualizar, ModelState);

        await serviceUpdate.AtualizarGPAsync(id, gpParaAtualizar);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarGP(int id)
    {
        try
        {
            var grandePremioDeletadoComSucesso = await serviceDelete.DeletarGPAsync(id);

            if (!grandePremioDeletadoComSucesso) return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}