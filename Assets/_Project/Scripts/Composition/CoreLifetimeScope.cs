using ShroomDefense.Input;
using VContainer;
using VContainer.Unity;

namespace ShroomDefense.Composition
{
    public class CoreLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<InputService>(Lifetime.Singleton);
            builder.Register<DeckbuildResults>(Lifetime.Singleton);
            builder.RegisterEntryPoint<CoreComposer>(Lifetime.Singleton);
        }
    }

}