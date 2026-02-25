using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.GpServices.Interfaces;

namespace F1.API.Services.GpServices.Services;

public class DeleteGrandePremioService : IDeleteGrandePremioService
{
    private readonly IGrandePremioQuery grandePremioQuery;
    private readonly IRepositoryBase<GrandePremio> grandePremioRepository;
    private readonly IUnitOfWork unitOfWork;

    public DeleteGrandePremioService(IGrandePremioQuery grandePremioQuery, 
        IRepositoryBase<GrandePremio> grandePremioRepository, IUnitOfWork unitOfWork)
    {
        this.grandePremioQuery = grandePremioQuery;
        this.grandePremioRepository = grandePremioRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<bool> DeletarGPAsync(int id)
    {
        var grandePremioADeletar = await grandePremioQuery.BuscarPorPropriedadeAsync(
            gp => gp.Id == id,
            gp => gp.Vencedor);

        if (grandePremioADeletar is null) return false;

        if (grandePremioADeletar.Vencedor is not null)
            throw new InvalidOperationException($"Não é possível deletar Grandes Prêmios com vencedores associados.");

        grandePremioRepository.Deletar(grandePremioADeletar);

        return await unitOfWork.CommitAsync();
    }
}
