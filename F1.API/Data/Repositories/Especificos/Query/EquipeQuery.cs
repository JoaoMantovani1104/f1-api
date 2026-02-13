using F1.Lib.Modelos;
using Microsoft.EntityFrameworkCore;
using F1.API.Data.Repositories.Genericos;
using F1.Lib.Interfaces.Especificas.Query;

namespace F1.API.Data.Repositories.Especificos.Query;

public class EquipeQuery : QueryBase<Equipe>, IEquipeQuery
{
    private readonly F1Context context;
    public EquipeQuery(F1Context context) : base(context) 
    {
        this.context = context;
    }
    
    public async Task<bool> ExistemPilotosNaEquipeAsync(int equipeId)
    {
        return await context.Set<Equipe>()
            .AsNoTracking()
            .AnyAsync(e => e.Id == equipeId && e.Pilotos.Any());
    }

    public async Task<Equipe?> ObterEquipeComMaisVitoriasAsync()
    {
        return await context.Set<Equipe>()
            .AsNoTracking()
            .OrderByDescending(e => e.Pilotos.Sum(p => p.GpsVencidos.Count))
            .FirstOrDefaultAsync();
    }
}
