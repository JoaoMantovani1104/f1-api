namespace F1.API.Services.EquipeServices.Interfaces;

public interface IDeleteEquipeService
{
    Task<bool> DeletarEquipeAsync(int id);
}
