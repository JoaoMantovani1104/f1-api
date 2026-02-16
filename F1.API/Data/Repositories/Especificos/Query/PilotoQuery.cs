using F1.Lib.Modelos;
using Microsoft.EntityFrameworkCore;
using F1.API.Data.Repositories.Genericos;
using F1.Lib.Interfaces.Especificas.Query;

namespace F1.API.Data.Repositories.Especificos.Query;

public class PilotoQuery : QueryBase<Piloto>, IPilotoQuery
{
    private readonly F1Context context;

    public PilotoQuery(F1Context context) : base(context)
    {
        this.context = context;
    }

    public async Task<Piloto?> ExistePilotoComNumeroAsync(int numero)
    {
        return await context.Set<Piloto>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Numero == numero);
    }

    public async Task<double> ObterMediaIdade()
    {
        return await context.Set<Piloto>()
            .AsNoTracking()
            .AverageAsync(p => p.Idade);
    }

    public async Task<Piloto?> ObterPilotoComMaisVitoriasAsync()
    {
        return await context.Set<Piloto>()
            .AsNoTracking()
            .OrderByDescending(p => p.GpsVencidos.Count)
            .FirstOrDefaultAsync();
    }
}
