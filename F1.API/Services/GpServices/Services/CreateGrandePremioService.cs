using AutoMapper;
using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.API.Data.Dtos.GrandePremioDTO;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.GpServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace F1.API.Services.GpServices.Services;

public class CreateGrandePremioService : ICreateGrandePremioService
{
    private readonly IMapper mapper;
    private readonly IGrandePremioQuery grandePremioQuery;
    private readonly IRepositoryBase<GrandePremio> grandePremioRepository;

    public CreateGrandePremioService(IMapper mapper, IGrandePremioQuery grandePremioQuery, 
        IRepositoryBase<GrandePremio> grandePremioRepository)
    {
        this.mapper = mapper;
        this.grandePremioQuery = grandePremioQuery;
        this.grandePremioRepository = grandePremioRepository;
    }

    public async Task<ReadGpDTO> AdicionarGPAsync(CreateGpDTO grandePremioDTO)
    {
        bool grandePremioJaExistente = await grandePremioQuery.BuscarPorCampoAsync(
            gp => EF.Functions.ILike(gp.Nome, grandePremioDTO.Nome.Trim()))
            is not null;

        if (grandePremioJaExistente) throw new InvalidOperationException("Grande Prêmio já existente.");

        var grandePremio = mapper.Map<GrandePremio>(grandePremioDTO);

        await grandePremioRepository.AdicionarAsync(grandePremio);

        return mapper.Map<ReadGpDTO>(grandePremio);
    }
}
