using AutoMapper;
using F1.API.Data;
using F1.Lib.Modelos;
using F1.API.Data.Dtos.EquipeDTO;
using Microsoft.EntityFrameworkCore;
using F1.API.Services.EquipeServices.Interfaces;
using F1.Lib.Interfaces.Especificas.Query;
using F1.Lib.Interfaces.Genericas;

namespace F1.API.Services.EquipeServices.Services;

public class CreateEquipeService : ICreateEquipeService
{
    private readonly IMapper mapper;
    private readonly IEquipeQuery equipeQuery;
    private readonly IRepositoryBase<Equipe> equipeRepository;

    public CreateEquipeService(IMapper mapper, IEquipeQuery equipeQuery, IRepositoryBase<Equipe> equipeRepository)
    {
        this.mapper = mapper;
        this.equipeQuery = equipeQuery;
        this.equipeRepository = equipeRepository;
    }

    public async Task<ReadEquipeDTO> AdicionarEquipeAsync(CreateEquipeDTO equipeDTO)
    {
        bool equipeJaExistente = await equipeQuery.BuscarPorCampoAsync( 
            e => EF.Functions.ILike(e.Nome, equipeDTO.Nome.Trim()))
            is not null;
        
        if (equipeJaExistente) throw new InvalidOperationException($"Equipe com nome '{equipeDTO.Nome}' já existente.");

        var equipe = mapper.Map<Equipe>(equipeDTO);

        await equipeRepository.AdicionarAsync(equipe);

        return mapper.Map<ReadEquipeDTO>(equipe);
    }
}
