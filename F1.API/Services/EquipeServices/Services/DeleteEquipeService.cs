using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.EquipeServices.Interfaces;

namespace F1.API.Services.EquipeServices.Services;

public class DeleteEquipeService : IDeleteEquipeService
{
    private readonly IEquipeQuery equipeQuery;
    private readonly IRepositoryBase<Equipe> equipeRepository;

    public DeleteEquipeService(IEquipeQuery equipeQuery, IRepositoryBase<Equipe> equipeRepository)
    {
        this.equipeQuery = equipeQuery;
        this.equipeRepository = equipeRepository;
    }

    public async Task<bool> DeletarEquipeAsync(int id)
    {
        var equipeADeletar = await equipeQuery.BuscarPorCampoAsync(e => e.Id == id);

        if (equipeADeletar is null) return false;

        bool equipeComPilotosAssociados = await equipeQuery
            .BuscarPorCampoAsync(e => e.Id == id && e.Pilotos.Any())
            is not null;

        if (equipeComPilotosAssociados)
            throw new InvalidOperationException("Não é possível deletar uma equipe que possui pilotos associados.");

        return await equipeRepository.DeletarAsync(equipeADeletar); 
    }
}
