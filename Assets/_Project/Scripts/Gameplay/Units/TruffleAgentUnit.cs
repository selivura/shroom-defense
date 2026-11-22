using ShroomDefense.Gameplay;
using UnityEngine;
using VContainer;

namespace ShroomDefense
{
    public class TruffleAgentUnit : Unit
    {
        private PlayerController _player;

        [Inject]
        public void Construct(BattleMap map, PlayerController player)
        {
            _battleMap = map;
            _player = player;
        }

        private void Start()
        {   
            _player.IncreaseMaxMana(1); 

        }

        public override void ExecuteAction()
        {
            Debug.Log("TruffleAgentUnitxecuteAction");
        }
    }
}