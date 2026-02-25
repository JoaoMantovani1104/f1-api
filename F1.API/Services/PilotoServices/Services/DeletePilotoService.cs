using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.PilotoServices.Interfaces;

namespace F1.API.Services.PilotoServices.Services;

public class DeletePilotoService : IDeletePilotoService
{
    private readonly IPilotoQuery pilotoQuery;
    private readonly IRepositoryBase<Piloto> pilotoRepository;
    private readonly IUnitOfWork unitOfWork;

    public DeletePilotoService(IPilotoQuery pilotoQuery, IRepositoryBase<Piloto> pilotoRepository, IUnitOfWork unitOfWork)
    {
        this.pilotoQuery = pilotoQuery;
        this.pilotoRepository = pilotoRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<bool> DeletarPilotoAsync(int id)
    {
        var pilotoADeletar = await pilotoQuery.BuscarPorPropriedadeAsync(p => p.Id == id);

        if (pilotoADeletar == null) return false;

        pilotoRepository.Deletar(pilotoADeletar);

        return await unitOfWork.CommitAsync();
    }
}
