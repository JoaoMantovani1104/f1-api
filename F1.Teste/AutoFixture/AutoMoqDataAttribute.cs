using AutoFixture;
using AutoFixture.Xunit2;
using F1.API.Controllers;
using AutoFixture.AutoMoq;

namespace F1.Teste.AutoFixture;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute() : base(() =>
    {
        var fixture = new Fixture();

        fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        fixture.Customize<PilotoController>(c => c.OmitAutoProperties());
        fixture.Customize<EquipeController>(c => c.OmitAutoProperties());
        fixture.Customize<GpController>(c => c.OmitAutoProperties());
        fixture.Customize<RelatorioController>(c => c.OmitAutoProperties());

        return fixture;
    })
    { }
}
