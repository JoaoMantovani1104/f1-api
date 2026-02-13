using F1.API.Data.Dtos.GrandePremioDTO;

namespace F1.API.Services.GpServices.Interfaces;

public interface ICreateGrandePremioService
{
    Task<ReadGpDTO> AdicionarGPAsync(CreateGpDTO createGPDTO);
}
