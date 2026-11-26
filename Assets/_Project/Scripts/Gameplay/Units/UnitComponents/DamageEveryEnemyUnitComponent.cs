using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/DamageEveryEnemy")]
    public class DamageEveryEnemyUnitComponent : UnitComponent
    {
        [SerializeField] private int _damageAmount = 1;
        public override async UniTask DoComponentAction()
        {
            var enemies = unit.FindAllUnitsOnEnemyField();
            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(new Damage(unit, _damageAmount));
            }
            Debug.Log($"{unit.name} damaged everyone on enemy team (ExecuteAction)");
        }
    }
}
