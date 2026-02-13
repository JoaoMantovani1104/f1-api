using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.API.Services.GpServices.Interfaces;
using F1.Lib.Interfaces.Especificas.Query;

namespace F1.API.Services.GpServices.Services;

public class DeleteGrandePremioService : IDeleteGrandePremioService
{
    private readonly IGrandePremioQuery grandePremioQuery;
    private readonly IRepositoryBase<GrandePremio> grandePremioRepository;

    public DeleteGrandePremioService(IGrandePremioQuery grandePremioQuery, IRepositoryBase<GrandePremio> grandePremioRepository)
    {
        this.grandePremioQuery = grandePremioQuery;
        this.grandePremioRepository = grandePremioRepository;
    }

    public async Task<bool> DeletarGPAsync(int id)
    {
        var grandePremioADeletar = await grandePremioQuery.BuscarPorCampoAsync(gp => gp.Id == id);

        if (grandePremioADeletar is null) return false;

        bool grandePremioTemVencedor = await grandePremioQuery
            .BuscarPorCampoAsync(gp => gp.Id == id && gp.Vencedor != null) != null;

        if (grandePremioTemVencedor)
            throw new InvalidOperationException($"Não é possível deletar Grandes Prêmios com vencedores associados.");

        return await grandePremioRepository.DeletarAsync(grandePremioADeletar);
    }
}
