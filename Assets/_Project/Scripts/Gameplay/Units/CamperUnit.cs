using ShroomDefense.Gameplay;
using UnityEngine;
using System.Linq;

namespace ShroomDefense
{
    public class CamperUnit : EnemyUnit
    {
        public override void ExecuteAction()
        {
            var enemies = _battleMap.Rows
                .SelectMany(row => row.Units.Values)
                .Where(unit => unit.Position.x > 0 && unit.CurrentHP > 0)
                .ToList();

            foreach (var enemy in enemies)
            {
                enemy.Heal(1);
            }
            Debug.Log("Camper healed all enemies for 1 HP");
        }
    }
}