using ShroomDefense.Gameplay;
using System.Linq;
using UnityEngine;

namespace ShroomDefense
{
    public class RainWandererUnit : Unit
    {
        public override void ExecuteAction()
        {
            foreach (var row in _battleMap.Rows)
            {
                var enemiesToDamage = row.Units.Values.Where(unit => unit.Position.x > 0 && unit.CurrentHP > 0).ToList();
                foreach (var enemy in enemiesToDamage)
                {
                    enemy.TakeDamage(new Damage(this, 1));
                }
            }
            Debug.Log("RainWandererUnit executed action"); 
        }
    }
}

