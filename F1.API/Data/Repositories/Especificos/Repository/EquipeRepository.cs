using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.API.Data.Repositories.Genericos;

namespace F1.API.Data.Repositories.Especificos.Repository;

public class EquipeRepository : RepositoryBase<Equipe>, IRepositoryBase<Equipe>
{
    private readonly F1Context context;
    public EquipeRepository(F1Context context) : base(context) 
    {
        this.context = context;
    }
}
