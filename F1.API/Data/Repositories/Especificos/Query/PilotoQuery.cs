using F1.Lib.Modelos;
using Microsoft.EntityFrameworkCore;
using F1.API.Data.Repositories.Genericos;
using F1.Lib.Interfaces.Especificas.Query;

namespace F1.API.Data.Repositories.Especificos.Query;

public class PilotoQuery : QueryBase<Piloto>, IPilotoQuery
{
    public PilotoQuery(F1Context context) : base(context) { }

    public async Task<bool> ExistePilotoComNumeroAsync(int numero)
    {
        return await context.Set<Piloto>()
            .AsNoTracking()
            .AnyAsync(p => p.Numero == numero);
    }

    public async Task<double> ObterMediaIdade()
    {
        var possuiPilotos = await context.Set<Piloto>().AnyAsync();

        if (!possuiPilotos) return 0;

        return await context.Set<Piloto>()
            .AsNoTracking()
            .AverageAsync(p => p.Idade);
    }
}
