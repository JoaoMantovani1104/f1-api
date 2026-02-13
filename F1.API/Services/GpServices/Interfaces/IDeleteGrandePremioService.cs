namespace F1.API.Services.GpServices.Interfaces;

public interface IDeleteGrandePremioService
{
    Task<bool> DeletarGPAsync(int id);
}
