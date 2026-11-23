using NaughtyAttributes;
using System.Linq;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/WitheredOnStart")]
    public class WitheredUnitComponent : UnitComponent
    {
        [SerializeField][Required] private StatType _attackStatType;
        [SerializeField] private float _attackThreshold = 3f;

        protected override void OnComponentInit()
        {
            var firstEnemy = unit.FindAllUnitsOnEnemyField()
                .Where(enemy => enemy.Position.y == unit.Position.y && enemy.CurrentHP > 0)
                .OrderBy(enemy => Mathf.Abs(enemy.Position.x - unit.Position.x))
                .FirstOrDefault();
            if (firstEnemy == null)
            {
                return;
            }

            bool hasAttackStat = firstEnemy.StatController.GetStat(_attackStatType, out Stat attackStat);
            if (!hasAttackStat || attackStat.Value <= _attackThreshold)
            {
                firstEnemy.TakeDamage(new Damage(unit, 999));
            }
        }
    }
}

