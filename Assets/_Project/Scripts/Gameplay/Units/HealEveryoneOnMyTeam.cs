using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/HealEveryoneOnMyTeamOnAction")]
    public class HealEveryoneOnMyTeam : UnitComponent
    {
        [SerializeField] private int _healAmount = 1;
        public override async UniTask OnExecuteAction()
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
