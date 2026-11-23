using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/HealEveryoneOnTeam")]
    public class HealEveryoneOnMyTeam : UnitComponent
    {
        private BattleMap _battleMap;
        [SerializeField] private int _healAmount = 1;
        [Inject]
        public void Construct(BattleMap battleMap)
        {
            _battleMap = battleMap;
        }
        public override void OnExecuteAction()
        {
            var allies = unit.FindAllUnitsOnMyTeam();
            foreach (var ally in allies)
            {
                ally.Heal(_healAmount);
            }
            Debug.Log($"{unit.name} healed everyone on their team (ExecuteAction)"); 
        }
    }
}
