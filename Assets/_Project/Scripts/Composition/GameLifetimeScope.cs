using NaughtyAttributes;
using ShroomDefense.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ShroomDefense.Composition
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] [Required] private BattleMap _map;
        [SerializeField][Required] private TurnManager _turnManager;
        [SerializeField][Required] private CardFactory _cardFactory;
        [SerializeField][Required] private UnitFactory _unitFactory;
        [SerializeField][Required] private PlayerController _playerController;
        [SerializeField][Required] private PlayerUIHandCardsController _playerUIHandCardsController;
        [SerializeField][Required] private WaveController _waveController;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_cardFactory);
            builder.RegisterInstance(_unitFactory);
            builder.RegisterInstance(_playerController);
            builder.RegisterInstance(_playerUIHandCardsController);
            builder.RegisterInstance(_waveController);

            builder.RegisterInstance(_map);
            builder.RegisterInstance(_turnManager);

            builder.RegisterEntryPoint<GameComposer>(Lifetime.Singleton);
        }
    }
}
