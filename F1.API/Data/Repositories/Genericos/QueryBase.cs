using F1.Lib.Interfaces.Genericas;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace F1.API.Data.Repositories.Genericos;

public class QueryBase<T> : IQueryBase<T> where T : class
{
    private readonly F1Context context;

    public QueryBase(F1Context context)
    {
        this.context = context;
    }

    public async Task<T?> BuscarPorCampoAsync(Expression<Func<T, bool>> predicado, params Expression<Func<T, object?>>[] includes)
    {
        IQueryable<T> query = context.Set<T>().AsNoTracking();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(predicado);
    }

    public async Task<IEnumerable<T>> ObterTodosAsync(params Expression<Func<T, object?>>[] includes)
    {
        IQueryable<T> query = context.Set<T>().AsNoTracking();

        if (includes != null)
        {
            foreach (var include in includes)
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
}
