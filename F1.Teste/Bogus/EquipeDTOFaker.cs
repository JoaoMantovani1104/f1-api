using Bogus;
using F1.API.Data.Dtos.EquipeDTO;

namespace F1.Teste.Bogus;

public static class EquipeDTOFaker
{
    public static Faker<CreateEquipeDTO> GerarCreate()
    {
        return new Faker<CreateEquipeDTO>()
            .RuleFor(e => e.Nome, f => f.Vehicle.Manufacturer());      
    }

    public static Faker<ReadEquipeDTO> GerarRead()
    {
        return new Faker<ReadEquipeDTO>()
            .RuleFor(e => e.Id, f => f.IndexFaker + 1)
            .RuleFor(e => e.Nome, f => f.Vehicle.Manufacturer());
    }

    public static Faker<UpdateEquipeDTO> GerarUpdate()
    {
        return new Faker<UpdateEquipeDTO>()
            .RuleFor(e => e.Nome, f => f.Vehicle.Manufacturer());
    }
}
