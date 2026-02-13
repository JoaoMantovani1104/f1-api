using AutoMapper;
using F1.Lib.Modelos;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.PilotoDTO;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.PilotoServices.Interfaces;

namespace F1.API.Services.PilotoServices.Services;

public class UpdatePilotoService : IUpdatePilotoService
{
    private readonly IMapper mapper;
    private readonly IPilotoQuery pilotoQuery;
    private readonly IRepositoryBase<Piloto> pilotoRepository;

    public UpdatePilotoService(IMapper mapper, IPilotoQuery pilotoQuery, IRepositoryBase<Piloto> pilotoRepository)
    {
        this.mapper = mapper;
        this.pilotoQuery = pilotoQuery;
        this.pilotoRepository = pilotoRepository;
    }

    public async Task<bool> AtualizarPilotoAsync(int id, [FromBody] UpdatePilotoDTO pilotoDTO)
    {
        var pilotoAAtualizar = await pilotoQuery.BuscarPorCampoAsync(p => p.Id == id);

        if (pilotoAAtualizar == null) return false;

        bool pilotoComMesmoNumero = pilotoQuery
            .BuscarPorCampoAsync(p => p.Numero == pilotoDTO.Numero) != null;

        if (pilotoComMesmoNumero)
            throw new InvalidOperationException("Já existe um piloto com esse número.");

        mapper.Map(pilotoDTO, pilotoAAtualizar);

        return await pilotoRepository.AtualizarAsync(pilotoAAtualizar); 
    }
}
