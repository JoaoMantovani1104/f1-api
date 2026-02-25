using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.EquipeServices.Interfaces;

namespace F1.API.Services.EquipeServices.Services;

public class DeleteEquipeService : IDeleteEquipeService
{
    private readonly IEquipeQuery equipeQuery;
    private readonly IRepositoryBase<Equipe> equipeRepository;
    private readonly IUnitOfWork unitOfWork;

    public DeleteEquipeService(IEquipeQuery equipeQuery, IRepositoryBase<Equipe> equipeRepository, IUnitOfWork unitOfWork)
    {
        this.equipeQuery = equipeQuery;
        this.equipeRepository = equipeRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<bool> DeletarEquipeAsync(int id)
    {
        var equipeADeletar = await equipeQuery.BuscarPorPropriedadeAsync(
            e => e.Id == id, 
            e => e.Pilotos);

        if (equipeADeletar is null) return false;

        if (equipeADeletar.Pilotos is not null && equipeADeletar.Pilotos.Any())
            throw new InvalidOperationException("Não é possível deletar uma equipe que possui pilotos associados.");

        equipeRepository.Deletar(equipeADeletar);

        return await unitOfWork.CommitAsync();
    }
}
