using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/DamageEveryEnemyOnAction")]
    public class DamageEveryEnemyUnitComponent : UnitComponent
    {
        [SerializeField] private int _damageAmount = 1;
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
