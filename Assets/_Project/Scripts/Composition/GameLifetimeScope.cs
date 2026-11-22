using NaughtyAttributes;
using ShroomDefense.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ShroomDefense
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] [Required] private BattleMap _map;
        [SerializeField][Required] private TurnManager _turnManager;
        [SerializeField][Required] private CardFactory _cardFactory;
        [SerializeField][Required] private UnitFactory _unitFactory;
        [SerializeField][Required] private PlayerController _playerController;
        [SerializeField][Required] private PlayerUIHandCardsController _playerUIHandCardsController;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance<CardFactory>(_cardFactory);
            builder.RegisterInstance<UnitFactory>(_unitFactory);
            builder.RegisterInstance<PlayerController>(_playerController);
            builder.RegisterInstance<PlayerUIHandCardsController>(_playerUIHandCardsController);

            builder.RegisterInstance(_map);
            builder.RegisterInstance(_turnManager);

            builder.RegisterEntryPoint<GameComposer>(Lifetime.Singleton);
        }
    }
}
