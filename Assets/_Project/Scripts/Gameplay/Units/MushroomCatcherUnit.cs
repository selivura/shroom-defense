using ShroomDefense.Gameplay;
using UnityEngine;
using System.Linq;


namespace ShroomDefense
{
    public class MushroomCatcherUnit : EnemyUnit
    {
        public override void ExecuteAction()
        {
            var playerMushrooms = _battleMap.Rows
                .SelectMany(row => row.Units.Values)
                .Where(unit => unit.Position.x < 0 && unit.CurrentHP > 0)
                .ToList();

            if (playerMushrooms.Count > 0)
            {
                int randomIndex = Random.Range(0, playerMushrooms.Count);
                Unit targetToEat = playerMushrooms[randomIndex];

                targetToEat.TakeDamage(new Damage(this, 9999));
                Debug.Log($"Гриболов съел {targetToEat.name}!");
            }
        }
    }
}