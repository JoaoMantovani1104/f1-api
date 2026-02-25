using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.EquipeDTO;
using Microsoft.AspNetCore.JsonPatch;
using F1.API.Services.EquipeServices.Interfaces;

namespace F1.API.Controllers;

[ApiController]
[Route("/Equipes")]
public class EquipeController : ControllerBase
{
    private IMapper mapper;
    private readonly ICreateEquipeService serviceCreate;
    private readonly IReadEquipeService serviceRead;
    private readonly IUpdateEquipeService serviceUpdate;
    private readonly IDeleteEquipeService serviceDelete;


    public EquipeController(ICreateEquipeService serviceCreate, IReadEquipeService serviceRead, IUpdateEquipeService serviceUpdate,
        IDeleteEquipeService serviceDelete, IMapper mapper)
    {
        this.serviceCreate = serviceCreate;
        this.serviceRead = serviceRead;
        this.serviceUpdate = serviceUpdate;
        this.serviceDelete = serviceDelete;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> LerEquipes()
    {
        var listaDeEquipes = await serviceRead.LerEquipesAsync();

        return Ok(listaDeEquipes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> LerEquipePorId(int id)
    {
        var equipeBuscada = await serviceRead.LerEquipePorIdAsync(id);

        if (equipeBuscada is null) return NotFound(); 
        
        return Ok(equipeBuscada); 
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarEquipe([FromBody] CreateEquipeDTO equipeDTO)
    {
        try
        {
            var equipeCriada = await serviceCreate.AdicionarEquipeAsync(equipeDTO);
            return CreatedAtAction(nameof(LerEquipePorId), new { id = equipeCriada.Id }, equipeCriada);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarEquipe(int id, [FromBody] UpdateEquipeDTO equipeDTO)
    {
        try
        {
            var atualizadoComSucesso = await serviceUpdate.AtualizarEquipeAsync(id, equipeDTO);

            if (!atualizadoComSucesso) return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message); 
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> AtualizarEquipeParcial(int id, [FromBody] JsonPatchDocument<UpdateEquipeDTO> patch)
    {
        try
        {
            var equipeExistente = await serviceRead.LerEquipePorIdAsync(id);
            if (equipeExistente is null) return NotFound();

            var equipeParaAtualizar = mapper.Map<UpdateEquipeDTO>(equipeExistente);
            patch.ApplyTo(equipeParaAtualizar, ModelState);

            bool atualizadoComSucesso = await serviceUpdate.AtualizarEquipeAsync(id, equipeParaAtualizar);

            if (!atualizadoComSucesso) return NotFound();

                return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message); 
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarEquipe(int id)
    {
        try
        {
            var equipeDeletadaComSucesso = await serviceDelete.DeletarEquipeAsync(id);
            if (!equipeDeletadaComSucesso) return NotFound();
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}