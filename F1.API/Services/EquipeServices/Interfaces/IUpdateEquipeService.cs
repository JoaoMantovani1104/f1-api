using F1.API.Data.Dtos.EquipeDTO;

namespace F1.API.Services.EquipeServices.Interfaces;

public interface IUpdateEquipeService
{
    Task<bool> AtualizarEquipeAsync(int id, UpdateEquipeDTO equipeDTO);
}
