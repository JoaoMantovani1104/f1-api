using AutoMapper;
using F1.Lib.Modelos;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.PilotoDTO;
using Microsoft.AspNetCore.JsonPatch;
using F1.API.Services.PilotoServices.Interfaces;

namespace F1.API.Controllers;

[ApiController]
[Route("/Pilotos")]
public class PilotoController : ControllerBase
{
    private ICreatePilotoService serviceCreate;
    private IReadPilotoService serviceRead;
    private IUpdatePilotoService serviceUpdate;
    private IDeletePilotoService serviceDelete;
    private IMapper mapper;

    public PilotoController(ICreatePilotoService serviceCreate, IReadPilotoService serviceRead, IUpdatePilotoService serviceUpdate,
        IDeletePilotoService serviceDelete, IMapper mapper)
    {
        this.serviceCreate = serviceCreate;
        this.serviceRead = serviceRead;
        this.serviceUpdate = serviceUpdate;
        this.serviceDelete = serviceDelete;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> LerPilotos()
    {
        var pilotos = await serviceRead.LerPilotosAsync();

        return Ok(pilotos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> LerPilotoPorId(int id)
    {
        var piloto = await serviceRead.LerPilotoPorIdAsync(id);

        if (piloto is null) return NotFound();

        return Ok(piloto);
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarPiloto([FromBody] CreatePilotoDTO pilotoDTO)
    {
        try
        {
            var pilotoCriado = await serviceCreate.AdicionarPilotoAsync(pilotoDTO);
            return CreatedAtAction(nameof(LerPilotoPorId), new { id = pilotoCriado.Id }, pilotoCriado);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarPiloto(int id, [FromBody] UpdatePilotoDTO pilotoDTO)
    {
        try
        {
            bool pilotoAtualizadoComSucesso = await serviceUpdate.AtualizarPilotoAsync(id, pilotoDTO);

            if (!pilotoAtualizadoComSucesso) return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> AtualizarPartePiloto(int id, [FromBody] JsonPatchDocument<UpdatePilotoDTO> patch)
    {
        try
        {
            var pilotoExistente = await serviceRead.LerPilotoPorIdAsync(id);

            if (pilotoExistente is null) return NotFound();

            var pilotoParaAtualizar = mapper.Map<UpdatePilotoDTO>(pilotoExistente);
            patch.ApplyTo(pilotoParaAtualizar, ModelState);

            bool atualizadoComSucesso = await serviceUpdate.AtualizarPilotoAsync(id, pilotoParaAtualizar);

            if (!atualizadoComSucesso) return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarPiloto(int id)
    {
        var pilotoDeletadoComSucesso = await serviceDelete.DeletarPilotoAsync(id);

        if (!pilotoDeletadoComSucesso) return NotFound();

        return NoContent();
    }
}