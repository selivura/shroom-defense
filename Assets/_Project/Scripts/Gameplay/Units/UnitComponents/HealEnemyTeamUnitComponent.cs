using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/HealEnemyTeam")]
    public class HealEnemyTeamUnitComponent : UnitComponent
    {
        [SerializeField] private int _healAmount = 1;
        public override async UniTask DoComponentAction()
        {
            var enemies = unit.FindAllUnitsOnEnemyField();

            foreach (var enemy in enemies)
            {
                enemy.Heal(_healAmount);
            }
            Debug.Log($"{unit.name} healed all enemies for {_healAmount} HP");
        }
    }
}