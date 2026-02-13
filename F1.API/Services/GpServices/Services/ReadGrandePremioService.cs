using AutoMapper;
using F1.API.Data;
using Microsoft.EntityFrameworkCore;
using F1.API.Data.Dtos.GrandePremioDTO;
using F1.API.Services.GpServices.Interfaces;
using F1.Lib.Interfaces.Especificas.Query;

namespace F1.API.Services.GpServices.Services;

public class ReadGrandePremioService : IReadGrandePremioService
{
    private readonly IMapper mapper;
    private readonly IGrandePremioQuery grandePremioQuery;

    public ReadGrandePremioService(IMapper mapper, IGrandePremioQuery grandePremioQuery)
    {
        this.mapper = mapper;
        this.grandePremioQuery = grandePremioQuery;
    }

    public async Task<ReadGpDTO?> LerGPPorIdAsync(int id)
    {
        var grandePremio = await grandePremioQuery.BuscarPorCampoAsync(gp => gp.Id == id, gp => gp.Vencedor);

        return grandePremio != null ?
            mapper.Map<ReadGpDTO>(grandePremio)
            : null;
    }

    public async Task<IEnumerable<ReadGpDTO>> LerGPsAsync()
    {
        var grandesPremios = await grandePremioQuery.ObterTodosAsync(gp => gp.Vencedor);

        return mapper.Map<IEnumerable<ReadGpDTO>>(grandesPremios);
    }
}
