using AutoMapper;
using F1.API.Profiles;

namespace F1.Teste.ProfilesTests;

public class AutoMapperConfigurationTests
{
    [Fact]
    public void AutoMapper_Configuracao_Valida()
    {
        var configuracao = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<EquipeProfile>();
            cfg.AddProfile<PilotoProfile>();
            cfg.AddProfile<GpProfile>();    
        });

        configuracao.AssertConfigurationIsValid();
    }
}
