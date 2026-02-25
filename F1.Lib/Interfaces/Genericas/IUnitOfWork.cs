namespace F1.Lib.Interfaces.Genericas;

public interface IUnitOfWork
{
    Task<bool> CommitAsync();
}
