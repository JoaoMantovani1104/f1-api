using F1.API.Data.Dtos.PilotoDTO;

namespace F1.API.Services.PilotoServices.Interfaces;

public interface IReadPilotoService
{
    Task<IEnumerable<ReadPilotoDTO>> LerPilotosAsync();
    Task<ReadPilotoDTO?> LerPilotoPorIdAsync(int id);
}
