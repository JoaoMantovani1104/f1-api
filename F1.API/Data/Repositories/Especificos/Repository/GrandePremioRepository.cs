using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;
using F1.API.Data.Repositories.Genericos;

namespace F1.API.Data.Repositories.Especificos.Repository;

public class GrandePremioRepository : RepositoryBase<GrandePremio>, IRepositoryBase<GrandePremio>
{
    private readonly F1Context context;

    public GrandePremioRepository(F1Context context) : base(context) 
    {
        this.context = context;
    }
}