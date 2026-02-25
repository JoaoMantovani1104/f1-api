using AutoMapper;
using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using Microsoft.EntityFrameworkCore;
using F1.API.Data.Dtos.GrandePremioDTO;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.GpServices.Interfaces;

namespace F1.API.Services.GpServices.Services;

public class UpdateGrandePremioService : IUpdateGrandePremioService
{
    private readonly IMapper mapper;
    private readonly IPilotoQuery pilotoQuery;
    private readonly IGrandePremioQuery grandePremioQuery;
    private readonly IRepositoryBase<GrandePremio> grandePremioRepository;
    private readonly IUnitOfWork unitOfWork;

    public UpdateGrandePremioService(IMapper mapper, IPilotoQuery pilotoQuery, IGrandePremioQuery grandePremioQuery, 
        IRepositoryBase<GrandePremio> grandePremioRepository, IUnitOfWork unitOfWork)
    {
        this.mapper = mapper;
        this.pilotoQuery = pilotoQuery;
        this.grandePremioQuery = grandePremioQuery;
        this.grandePremioRepository = grandePremioRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<bool> AtualizarGPAsync(int id, UpdateGpDTO grandePremioDTO)
    {
        if (grandePremioDTO.VencedorId.HasValue)
        {
            var pilotoVencedorExiste = await pilotoQuery
                .BuscarPorPropriedadeAsync(p => p.Id == grandePremioDTO.VencedorId.Value) != null;

            if (!pilotoVencedorExiste)
                throw new InvalidOperationException($"O Piloto vencedor com ID {grandePremioDTO.VencedorId} não foi encontrado.");
        }

        var grandePremioAAtualizar = await grandePremioQuery.BuscarPorPropriedadeAsync(gp => gp.Id == id);

        if (grandePremioAAtualizar == null) return false;

        if (!string.Equals(grandePremioAAtualizar.Nome, grandePremioDTO.Nome.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            bool grandePremioComMesmoNome = await grandePremioQuery.BuscarPorPropriedadeAsync(
                                        gp => EF.Functions.ILike(gp.Nome, grandePremioDTO.Nome.Trim())
                                        && gp.Id != id)
                                        is not null;

            if (grandePremioComMesmoNome)
                throw new InvalidOperationException($"Já existe outro Grande Prêmio cadastrado com o nome '{grandePremioDTO.Nome}'.");
        }

        mapper.Map(grandePremioDTO, grandePremioAAtualizar);
        
        grandePremioRepository.Atualizar(grandePremioAAtualizar);

        return await unitOfWork.CommitAsync();
    }
}
