namespace F1.Lib.Interfaces.Genericas;

public interface IRepositoryBase<T> where T : class
{
    Task AdicionarAsync(T entity);
    void Atualizar(T entity);
    void Deletar(T entity);
}
