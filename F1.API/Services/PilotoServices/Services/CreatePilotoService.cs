using AutoMapper;
using F1.API.Data;
using F1.Lib.Modelos;
using Microsoft.AspNetCore.Mvc;
using F1.API.Data.Dtos.PilotoDTO;
using Microsoft.EntityFrameworkCore;
using F1.API.Services.PilotoServices.Interfaces;
using F1.Lib.Interfaces.Especificas.Query;
using F1.Lib.Interfaces.Genericas;

namespace F1.API.Services.PilotoServices.Services;

public class CreatePilotoService : ICreatePilotoService
{
    private readonly IMapper mapper;
    private readonly IPilotoQuery pilotoQuery;
    private readonly IRepositoryBase<Piloto> pilotoRepository;

    public CreatePilotoService(IMapper mapper, IPilotoQuery pilotoQuery, IRepositoryBase<Piloto> repositoryBase)
    {
        this.mapper = mapper;
        this.pilotoQuery = pilotoQuery;
        this.pilotoRepository = repositoryBase;
    }

    public async Task<ReadPilotoDTO> AdicionarPilotoAsync([FromBody] CreatePilotoDTO pilotoDTO)
    {
        bool pilotoJaExistente = await pilotoQuery.BuscarPorCampoAsync(p => p.Numero == pilotoDTO.Numero) != null;

        if (pilotoJaExistente) throw new InvalidOperationException($"Piloto com o número {pilotoDTO.Numero} já existente");

        var piloto = mapper.Map<Piloto>(pilotoDTO);

        await pilotoRepository.AdicionarAsync(piloto);

        return mapper.Map<ReadPilotoDTO>(piloto);
    }
}
