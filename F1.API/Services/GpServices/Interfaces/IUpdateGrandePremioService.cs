using F1.API.Data.Dtos.GrandePremioDTO;

namespace F1.API.Services.GpServices.Interfaces;

public interface IUpdateGrandePremioService
{
    Task<bool> AtualizarGPAsync(int id, UpdateGpDTO gpDTO);
}
