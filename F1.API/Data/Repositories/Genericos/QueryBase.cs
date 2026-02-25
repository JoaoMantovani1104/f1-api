using F1.Lib.Interfaces.Genericas;
using F1.Lib.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace F1.API.Data.Repositories.Genericos;

public class QueryBase<T> : IQueryBase<T> where T : class
{
    protected readonly F1Context context;

        public QueryBase(F1Context context)
    {
        this.context = context;
    }

    public async Task<T?> BuscarPorPropriedadeAsync(Expression<Func<T, bool>> predicado, params Expression<Func<T, object?>>[] includes)
    {
        IQueryable<T> query = context.Set<T>().AsNoTracking();

        if (includes.Length > 0)
        {
            foreach (var include in includes.Distinct())
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(predicado);
    }

    public async Task<IEnumerable<T>> ObterTodosAsync(params Expression<Func<T, object?>>[] includes)
    {
        IQueryable<T> query = context.Set<T>().AsNoTracking();

        if (includes.Length > 0)
        {
            foreach (var include in includes.Distinct())
            {
                query = query.Include(include);
            }
        }
        
        return await query.ToListAsync();
    }

    public async Task<int> ContarAsync()
    {
        return await context.Set<T>().CountAsync();
    }

    public async Task<T?> ObterEntidadeComMaisVitoriasAsync(Expression<Func<T, int>> predicado)
    {
        return await context.Set<T>()
            .AsNoTracking()
            .OrderByDescending(predicado)
            .FirstOrDefaultAsync();
    }
}
