using NaughtyAttributes;
using System.Linq;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/Withered")]
    public class WitheredUnitComponent : UnitComponent
    {
        private BattleMap _battleMap;
        [SerializeField][Required] private StatType _attackStatType;
        [SerializeField] private float _attackThreshold = 3f;

        [Inject]
        public void Construct(BattleMap battleMap)
        {
            _battleMap = battleMap;
        }
        public void Start()
        {
            var currentRow = _battleMap.Rows[unit.Position.y];
            var firstEnemy = currentRow.Units.Values
                .Where(unit => unit.Position.x > 0 && unit.CurrentHP > 0)
                .OrderBy(unit => unit.Position.x)
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

