using F1.API.Data.Dtos.EquipeDTO;

namespace F1.API.Services.EquipeServices.Interfaces;

public interface IReadEquipeService
{
    Task<IEnumerable<ReadEquipeDTO>?> LerEquipesAsync();
    Task<ReadEquipeDTO?> LerEquipePorIdAsync(int id);
}
