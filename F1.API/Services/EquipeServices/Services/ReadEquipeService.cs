using AutoMapper;
using F1.API.Data.Dtos.EquipeDTO;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.EquipeServices.Interfaces;

namespace F1.API.Services.EquipeServices.Services;

public class ReadEquipeService : IReadEquipeService
{
    private readonly IMapper mapper;
    private readonly IEquipeQuery equipeQuery;

    public ReadEquipeService(IMapper mapper, IEquipeQuery equipeQuery)
    {
        this.mapper = mapper;
        this.equipeQuery = equipeQuery;
    }

    public async Task<ReadEquipeDTO?> LerEquipePorIdAsync(int id)
    {
        var equipe = await equipeQuery.BuscarPorCampoAsync(e => e.Id == id, e => e.Pilotos);

        return equipe is not null ?
            mapper.Map<ReadEquipeDTO>(equipe)
            : null;
    }

    public async Task<IEnumerable<ReadEquipeDTO>?> LerEquipesAsync()
    {
        var equipes = await equipeQuery.ObterTodosAsync(e => e.Pilotos);

        return mapper.Map<IEnumerable<ReadEquipeDTO>>(equipes);
    }
}
