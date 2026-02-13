using F1.Lib.Modelos;
using F1.API.Data.Repositories.Genericos;
using F1.Lib.Interfaces.Especificas.Query;
using Microsoft.EntityFrameworkCore;

namespace F1.API.Data.Repositories.Especificos.Query;

public class GrandePremioQuery : QueryBase<GrandePremio>, IGrandePremioQuery
{
    private readonly F1Context context;

    public GrandePremioQuery(F1Context context) : base(context)
    {
        this.context = context;
    }
}
