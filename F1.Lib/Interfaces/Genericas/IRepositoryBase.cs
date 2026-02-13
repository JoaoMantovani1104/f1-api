namespace F1.Lib.Interfaces.Genericas;

public interface IRepositoryBase<T> where T : class
{
    Task<T> AdicionarAsync(T entity);
    Task<bool> AtualizarAsync(T entity);
    Task<bool> DeletarAsync(T entity);
}
