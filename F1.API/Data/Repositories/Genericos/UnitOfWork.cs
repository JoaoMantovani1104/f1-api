using F1.Lib.Interfaces.Genericas;

namespace F1.API.Data.Repositories.Genericos;

public class UnitOfWork : IUnitOfWork
{
    private readonly F1Context context;

    public UnitOfWork(F1Context context)
    {
        this.context = context; 
    }

    public async Task<bool> CommitAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
