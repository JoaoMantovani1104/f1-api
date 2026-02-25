using AutoMapper;
using F1.Lib.Modelos;
using F1.API.Data.Dtos.PilotoDTO;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.PilotoServices.Interfaces;

namespace F1.API.Services.PilotoServices.Services;

public class UpdatePilotoService : IUpdatePilotoService
{
    private readonly IMapper mapper;
    private readonly IEquipeQuery equipeQuery;
    private readonly IPilotoQuery pilotoQuery;
    private readonly IRepositoryBase<Piloto> pilotoRepository;
    private readonly IUnitOfWork unitOfWork;

    public UpdatePilotoService(IMapper mapper, IEquipeQuery equipeQuery, IPilotoQuery pilotoQuery, 
        IRepositoryBase<Piloto> pilotoRepository, IUnitOfWork unitOfWork)
    {
        this.mapper = mapper;
        this.equipeQuery = equipeQuery;
        this.pilotoQuery = pilotoQuery;
        this.pilotoRepository = pilotoRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<bool> AtualizarPilotoAsync(int id, UpdatePilotoDTO pilotoDTO)
    {
        var equipeExiste = await equipeQuery.BuscarPorPropriedadeAsync(e => e.Id == pilotoDTO.EquipeId) != null;

        if (!equipeExiste)
            throw new InvalidOperationException($"A equipe com o ID {pilotoDTO.EquipeId} não foi encontrada");

        var pilotoAAtualizar = await pilotoQuery.BuscarPorPropriedadeAsync(p => p.Id == id);

        if (pilotoAAtualizar == null) return false;

        if(pilotoAAtualizar.Numero != pilotoDTO.Numero)
        {
            bool pilotoComMesmoNumero = await pilotoQuery.BuscarPorPropriedadeAsync(
                p => p.Numero == pilotoDTO.Numero && p.Id != id) 
                is not null;

            if (pilotoComMesmoNumero)
                throw new InvalidOperationException("Já existe um piloto com esse número.");
        }

        mapper.Map(pilotoDTO, pilotoAAtualizar);

        pilotoRepository.Atualizar(pilotoAAtualizar);

        return await unitOfWork.CommitAsync();
    }
}
