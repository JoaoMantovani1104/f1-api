using AutoMapper;
using F1.Lib.Modelos;
using F1.API.Data.Dtos.PilotoDTO;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.PilotoServices.Interfaces;

namespace F1.API.Services.PilotoServices.Services;

public class CreatePilotoService : ICreatePilotoService
{
    private readonly IMapper mapper;
    private readonly IEquipeQuery equipeQuery;
    private readonly IPilotoQuery pilotoQuery;
    private readonly IRepositoryBase<Piloto> pilotoRepository;
    private readonly IUnitOfWork unitOfWork;

    public CreatePilotoService(IMapper mapper, IEquipeQuery equipeQuery, IPilotoQuery pilotoQuery, 
        IRepositoryBase<Piloto> pilotoRepository, IUnitOfWork unitOfWork)
    {
        this.mapper = mapper;
        this.equipeQuery = equipeQuery;
        this.pilotoQuery = pilotoQuery;
        this.pilotoRepository = pilotoRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<ReadPilotoDTO> AdicionarPilotoAsync(CreatePilotoDTO pilotoDTO)
    {
        var equipeExiste = await equipeQuery.BuscarPorPropriedadeAsync(e => e.Id == pilotoDTO.EquipeId) != null;

        if (!equipeExiste) 
            throw new InvalidOperationException($"A equipe com o ID {pilotoDTO.EquipeId} não foi encontrada");

        bool pilotoJaExistente = await pilotoQuery.BuscarPorPropriedadeAsync(
            p => p.Numero == pilotoDTO.Numero) 
            is not null;

        if (pilotoJaExistente) 
            throw new InvalidOperationException($"Piloto com o número {pilotoDTO.Numero} já existente");

        var piloto = mapper.Map<Piloto>(pilotoDTO);

        await pilotoRepository.AdicionarAsync(piloto);

        bool salvoComSucesso = await unitOfWork.CommitAsync();

        if (!salvoComSucesso) throw new Exception("Ocorreu um erro ao salvar o piloto no banco de dados.");

        return mapper.Map<ReadPilotoDTO>(piloto);
    }
}
