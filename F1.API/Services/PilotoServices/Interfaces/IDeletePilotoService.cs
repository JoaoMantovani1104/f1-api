namespace F1.API.Services.PilotoServices.Interfaces;

public interface IDeletePilotoService
{
    Task<bool> DeletarPilotoAsync(int id);
}
