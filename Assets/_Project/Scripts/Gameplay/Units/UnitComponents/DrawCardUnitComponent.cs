using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/DrawCard")]
    public class DrawCardUnitComponent: UnitComponent
    {
        private PlayerController _player;
        [SerializeField] private int _amountOfCard = 1;

        [Inject]
        public void Construct(PlayerController player)
        {
            _player = player;
        }

        public override async UniTask DoComponentAction()
        {
            for (int i = 0; i < _amountOfCard; i++)
            {
                _player.DrawRandomCardFromDeck();
            }
        }
    }
}
