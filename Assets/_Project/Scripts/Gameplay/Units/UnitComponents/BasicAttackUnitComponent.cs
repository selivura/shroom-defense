using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    /// <summary>
    /// Uses <see cref="_attackStatType"/> as damage amount, and deals damage to nearest enemy unit in the same row,
    /// If no enemy unit is found, it will deal damage to the player instead (Only if unit is opposed to player).
    /// </summary>
    public class BasicAttackUnitComponent : UnitComponent
    {
        [SerializeField] private StatType _attackStatType;
        private PlayerController _player;

        [Inject]
        public void Construct(PlayerController player)
        {
            _player = player;
        }
        public override void OnExecuteAction()
        {
            if (!unit.StatController.GetStatValue(_attackStatType, out var statValue))
            {
                return;
            }
            if(statValue == 0)
            {
                return;
            }

            // Find closest enemy unit with the same Y coordinate as this unit, or hit Player instead.
            Unit closestEnemy = null;
            int closestDistance = int.MaxValue;

            var enemyUnits = unit.FindAllUnitsOnEnemyField();

            foreach (var enemy in enemyUnits)
            {
                var distance = enemy.Position.x - unit.Position.x;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            if (closestEnemy != null)
            {
                closestEnemy.TakeDamage(new Damage(unit, Mathf.RoundToInt(statValue)));
                Debug.Log($"{unit.name} attacked {closestEnemy.name} for {statValue} damage!");
                return;
            }
            if (unit.Position.x < 0) return;

            _player.TakeDamage(new Damage(unit, Mathf.RoundToInt(statValue)));
            Debug.Log($"{unit.name} attacked the player for {statValue} damage!");
        }
    }
}
