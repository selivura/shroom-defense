using NaughtyAttributes;
using ShroomDefense.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ShroomDefense.Composition
{
    public class DeckbuildLifetimeScope : LifetimeScope
    {
        [SerializeField] [Required] private DeckBuilder _deckBuilder;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_deckBuilder).As<DeckBuilder>();
            builder.RegisterEntryPoint<DeckbuildComposer>(Lifetime.Singleton);
        }
    }
}
