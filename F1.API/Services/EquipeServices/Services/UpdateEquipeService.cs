using AutoMapper;
using F1.Lib.Modelos;
using F1.API.Data.Dtos.EquipeDTO;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.EquipeServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace F1.API.Services.EquipeServices.Services;

public class UpdateEquipeService : IUpdateEquipeService
{
    private readonly IMapper mapper;
    private readonly IEquipeQuery equipeQuery;
    private readonly IRepositoryBase<Equipe> equipeRepository;

    public UpdateEquipeService(IMapper mapper, IEquipeQuery equipeQuery, IRepositoryBase<Equipe> equipeRepository)
    {
        this.mapper = mapper;
        this.equipeQuery = equipeQuery;
        this.equipeRepository = equipeRepository;
    }

    public async Task<bool> AtualizarEquipeAsync(int id, UpdateEquipeDTO equipeDTO)
    {
        var equipeAAtualizar = await equipeQuery.BuscarPorCampoAsync(e => e.Id == id);

        if (equipeAAtualizar == null) return false;

        bool equipeComMesmoNome = await equipeQuery.BuscarPorCampoAsync(
                                    e => e.Id != id && 
                                    EF.Functions.ILike(e.Nome, equipeDTO.Nome.Trim())) 
                                    is not null;

        if (equipeComMesmoNome)
            throw new InvalidOperationException($"Já existe outra equipe cadastrada com o nome '{equipeDTO.Nome}'.");
    
        mapper.Map(equipeDTO, equipeAAtualizar);

        return await equipeRepository.AtualizarAsync(equipeAAtualizar); 
    }
}
