using F1.API.Data.Dtos.PilotoDTO;

namespace F1.API.Services.PilotoServices.Interfaces;

public interface IUpdatePilotoService
{
    Task<bool> AtualizarPilotoAsync(int id, UpdatePilotoDTO updatePilotoDTO);
}
