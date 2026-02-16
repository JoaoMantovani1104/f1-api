using AutoMapper;
using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.API.Data.Dtos.GrandePremioDTO;
using F1.API.Services.GpServices.Interfaces;
using F1.Lib.Interfaces.Especificas.Query;
using Microsoft.EntityFrameworkCore;

namespace F1.API.Services.GpServices.Services;

public class UpdateGrandePremioService : IUpdateGrandePremioService
{
    private readonly IMapper mapper;
    private readonly IGrandePremioQuery grandePremioQuery;
    private readonly IRepositoryBase<GrandePremio> grandePremioRepository;

    public UpdateGrandePremioService(IMapper mapper, IGrandePremioQuery grandePremioQuery, 
        IRepositoryBase<GrandePremio> grandePremioRepository)
    {
        this.mapper = mapper;
        this.grandePremioQuery = grandePremioQuery;
        this.grandePremioRepository = grandePremioRepository;
    }

    public async Task<bool> AtualizarGPAsync(int id, UpdateGpDTO grandePremioDTO)
    {
        var grandePremioAAtualizar = await grandePremioQuery.BuscarPorCampoAsync(gp => gp.Id == id);

        if (grandePremioAAtualizar == null) return false;

        bool grandePremioComMesmoNome = await grandePremioQuery.BuscarPorCampoAsync(
                                    gp => EF.Functions.ILike(gp.Nome, grandePremioDTO.Nome.Trim()))
                                    is not null;

        if (grandePremioComMesmoNome)
            throw new InvalidOperationException($"Já existe outro Grande Prêmio cadastrado com o nome '{grandePremioDTO.Nome}'."); 

        mapper.Map(grandePremioDTO, grandePremioAAtualizar);
        
        return await grandePremioRepository.AtualizarAsync(grandePremioAAtualizar); ;
    }
}
