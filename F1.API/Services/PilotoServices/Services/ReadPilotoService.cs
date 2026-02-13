
using AutoMapper;
using F1.API.Data.Dtos.PilotoDTO;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.PilotoServices.Interfaces;

namespace F1.API.Services.PilotoServices.Services;

public class ReadPilotoService : IReadPilotoService
{
    private readonly IMapper mapper;
    private readonly IPilotoQuery pilotoQuery;

    public ReadPilotoService(IMapper mapper, IPilotoQuery pilotoQuery)
    {
        this.mapper = mapper;
        this.pilotoQuery = pilotoQuery;
    }

    public async Task<ReadPilotoDTO?> LerPilotoPorIdAsync(int id)
    {
        var piloto = await pilotoQuery.BuscarPorCampoAsync(p => p.Id == id, [p => p.Equipe, p => p.GpsVencidos]);

        return piloto != null ?
            mapper.Map<ReadPilotoDTO>(piloto)
            : null;
    }

    public async Task<IEnumerable<ReadPilotoDTO>> LerPilotosAsync()
    {
        var pilotos = await pilotoQuery.ObterTodosAsync([p => p.Equipe, p => p.GpsVencidos]);

        return mapper.Map<IEnumerable<ReadPilotoDTO>>(pilotos);
    }
}
