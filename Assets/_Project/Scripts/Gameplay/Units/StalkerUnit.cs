using ShroomDefense.Gameplay;
using UnityEngine;
using VContainer;

namespace ShroomDefense
{
    public class StalkerUnit : EnemyUnit
    {
        private PlayerController _player;

        [Inject]
        public void Construct(BattleMap map, PlayerController player)
        {
            _battleMap = map;
            _player = player;
        }

        public override void ExecuteAction()
        {
            _player.DecreaseMaxMana(1);
            Debug.Log("Stalker decreased player's max mana by 1");
        }
    }
}