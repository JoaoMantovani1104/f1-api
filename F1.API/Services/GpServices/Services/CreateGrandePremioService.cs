using AutoMapper;
using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using Microsoft.EntityFrameworkCore;
using F1.API.Data.Dtos.GrandePremioDTO;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.GpServices.Interfaces;

namespace F1.API.Services.GpServices.Services;

public class CreateGrandePremioService : ICreateGrandePremioService
{
    private readonly IMapper mapper;
    private readonly IPilotoQuery pilotoQuery;
    private readonly IGrandePremioQuery grandePremioQuery;
    private readonly IRepositoryBase<GrandePremio> grandePremioRepository;
    private readonly IUnitOfWork unitOfWork;

    public CreateGrandePremioService(IMapper mapper, IPilotoQuery pilotoQuery, IGrandePremioQuery grandePremioQuery, 
        IRepositoryBase<GrandePremio> grandePremioRepository, IUnitOfWork unitOfWork)
    {
        this.mapper = mapper;
        this.pilotoQuery = pilotoQuery;
        this.grandePremioQuery = grandePremioQuery;
        this.grandePremioRepository = grandePremioRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<ReadGpDTO> AdicionarGPAsync(CreateGpDTO grandePremioDTO)
    {
        if (grandePremioDTO.VencedorId.HasValue)
        {
            var pilotoVencedorExiste = await pilotoQuery
                .BuscarPorPropriedadeAsync(p => p.Id == grandePremioDTO.VencedorId.Value) != null;

            if (!pilotoVencedorExiste)
                throw new InvalidOperationException($"O Piloto vencedor com ID {grandePremioDTO.VencedorId} não foi encontrado.");
        }

        bool grandePremioExiste = await grandePremioQuery.BuscarPorPropriedadeAsync(
            gp => EF.Functions.ILike(gp.Nome, grandePremioDTO.Nome.Trim()))
            is not null;

        if (grandePremioExiste) throw new InvalidOperationException("Grande Prêmio já existente.");

        var grandePremio = mapper.Map<GrandePremio>(grandePremioDTO);

        await grandePremioRepository.AdicionarAsync(grandePremio);

        bool salvoComSucesso = await unitOfWork.CommitAsync();

        if (!salvoComSucesso) throw new Exception("Ocorreu um erro ao salvar o Grande Prêmio no banco de dados.");

        return mapper.Map<ReadGpDTO>(grandePremio);
    }
}
