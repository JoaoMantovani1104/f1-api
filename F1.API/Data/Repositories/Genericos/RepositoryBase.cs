using F1.Lib.Interfaces.Genericas;

namespace F1.API.Data.Repositories.Genericos;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly F1Context context;

    public RepositoryBase(F1Context context)
    {
        this.context = context;
    }

    public async Task AdicionarAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
    }

    public void Atualizar(T entity)
    {
        context.Set<T>().Update(entity);
    }

    public void Deletar(T entity)
    {
        context.Set<T>().Remove(entity);
    }
}
