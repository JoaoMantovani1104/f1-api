using Bogus;
using F1.Lib.Modelos;

namespace F1.Teste.Bogus;

public static class EquipeFaker
{
    public static Faker<Equipe> Gerar()
    {
        return new Faker<Equipe>()
            .RuleFor(e => e.Id, f => f.IndexFaker + 1)
            .RuleFor(e => e.Nome, f => f.Vehicle.Manufacturer());
    }
}
