using F1.API.Data.Dtos.EquipeDTO;

namespace F1.API.Services.EquipeServices.Interfaces;

public interface ICreateEquipeService
{
    Task<ReadEquipeDTO> AdicionarEquipeAsync(CreateEquipeDTO equipeDTO);
}
