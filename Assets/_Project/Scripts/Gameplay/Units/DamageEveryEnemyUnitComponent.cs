using ShroomDefense.Gameplay;
using System.Linq;
using UnityEngine;
using VContainer;

namespace ShroomDefense
{
    [AddComponentMenu("UnitComponents/DamageEveryEnemy")]
    public class DamageEveryEnemyUnitComponent : UnitComponent
    {
        private BattleMap _battleMap;
        [SerializeField] private int _damageAmount = 1;
        [Inject]
        public void Construct(BattleMap battleMap)
        {
            _battleMap = battleMap;
        }
        public override void OnExecuteAction()
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
