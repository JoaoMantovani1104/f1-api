using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.API.Data.Repositories.Genericos;

namespace F1.API.Data.Repositories.Especificos.Repository;

public class PilotoRepository : RepositoryBase<Piloto>, IRepositoryBase<Piloto>
{
    private readonly F1Context context;
    public PilotoRepository(F1Context context) : base(context) 
    {
        this.context = context;
    }
}
