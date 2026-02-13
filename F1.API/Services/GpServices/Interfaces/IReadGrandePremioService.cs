using F1.API.Data.Dtos.GrandePremioDTO;

namespace F1.API.Services.GpServices.Interfaces;

public interface IReadGrandePremioService
{
    Task<IEnumerable<ReadGpDTO>> LerGPsAsync();
    Task<ReadGpDTO?> LerGPPorIdAsync(int id);
}
