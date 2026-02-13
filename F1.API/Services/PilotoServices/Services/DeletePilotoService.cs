using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.PilotoServices.Interfaces;

namespace F1.API.Services.PilotoServices.Services;

public class DeletePilotoService : IDeletePilotoService
{
    private readonly IPilotoQuery pilotoQuery;
    private readonly IRepositoryBase<Piloto> pilotoRepository;

    public DeletePilotoService(IPilotoQuery pilotoQuery, IRepositoryBase<Piloto> pilotoRepository)
    {
        this.pilotoQuery = pilotoQuery;
        this.pilotoRepository = pilotoRepository;
    }

    public async Task<bool> DeletarPilotoAsync(int id)
    {
        var pilotoADeletar = await pilotoQuery.BuscarPorCampoAsync(p => p.Id == id);

        if (pilotoADeletar == null) return false;

        return await pilotoRepository.DeletarAsync(pilotoADeletar);
    }
}
