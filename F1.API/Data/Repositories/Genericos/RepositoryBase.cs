using F1.Lib.Interfaces.Genericas;

namespace F1.API.Data.Repositories.Genericos;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly F1Context context;
    public RepositoryBase(F1Context context)
    {
        this.context = context;
    }

    public async Task<T> AdicionarAsync(T entity)
    {
        var adicao = context.Set<T>().Add(entity);

        await context.SaveChangesAsync();

        return adicao.Entity;
    }

    public async Task<bool> AtualizarAsync(T entity)
    {
        var atualizacao = context.Set<T>().Update(entity);

        int linhasAfetadas = await context.SaveChangesAsync();

        return linhasAfetadas > 0;
    }

    public async Task<bool> DeletarAsync(T entity)
    {
        var delecao = context.Set<T>().Remove(entity);

        int linhasAfetadas = await context.SaveChangesAsync();

        return linhasAfetadas > 0;
    }
}
