using System.Linq.Expressions;

namespace F1.Lib.Interfaces.Genericas;

public interface IQueryBase<T> where T : class
{
    Task<IEnumerable<T>> ObterTodosAsync(params Expression<Func<T, object?>>[] includes);
    Task<T?> BuscarPorPropriedadeAsync(Expression<Func<T, bool>> predicado, params Expression<Func<T, object?>>[] includes);
    Task<int> ContarAsync();
    Task<T?> ObterEntidadeComMaisVitoriasAsync(Expression<Func<T, int>> predicado);
}
