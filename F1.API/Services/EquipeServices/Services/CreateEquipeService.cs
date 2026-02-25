using AutoMapper;
using F1.Lib.Modelos;
using F1.API.Data.Dtos.EquipeDTO;
using F1.Lib.Interfaces.Genericas;
using Microsoft.EntityFrameworkCore;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.EquipeServices.Interfaces;

namespace F1.API.Services.EquipeServices.Services;

public class CreateEquipeService : ICreateEquipeService
{
    private readonly IMapper mapper;
    private readonly IEquipeQuery equipeQuery;
    private readonly IRepositoryBase<Equipe> equipeRepository;
    private readonly IUnitOfWork unitOfWork;

    public CreateEquipeService(IMapper mapper, IEquipeQuery equipeQuery, 
        IRepositoryBase<Equipe> equipeRepository, IUnitOfWork unitOfWork)
    {
        this.mapper = mapper;
        this.equipeQuery = equipeQuery;
        this.equipeRepository = equipeRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<ReadEquipeDTO> AdicionarEquipeAsync(CreateEquipeDTO equipeDTO)
    {
        bool equipeJaExistente = await equipeQuery.BuscarPorPropriedadeAsync( 
            e => EF.Functions.ILike(e.Nome, equipeDTO.Nome.Trim()))
            is not null;
        
        if (equipeJaExistente) throw new InvalidOperationException($"Equipe com nome '{equipeDTO.Nome}' já existente.");

        var equipe = mapper.Map<Equipe>(equipeDTO);

        await equipeRepository.AdicionarAsync(equipe);

        bool salvoComSucesso = await unitOfWork.CommitAsync();

        if (!salvoComSucesso) throw new Exception("Ocorreu um erro ao salvar a equipe no banco de dados.");

        return mapper.Map<ReadEquipeDTO>(equipe);
    }
}
