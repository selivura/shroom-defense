using ShroomDefense.Gameplay;
using System.Linq;
using UnityEngine;

namespace ShroomDefense
{
    public class WitheredUnit : Unit
    {
        public void Start()
        {
            var currentRow = _battleMap.Rows[Position.y];
            var firstEnemy = currentRow.Units.Values
                .Where(unit => unit.Position.x > 0 && unit.CurrentHP > 0)
                .OrderBy(unit => unit.Position.x)
                .FirstOrDefault();
            if (firstEnemy != null && firstEnemy.CurrentAttack <= 3)
            {
                firstEnemy.TakeDamage(new Damage(this, 999));
            }
        }
        public override void ExecuteAction()
        {
            Debug.Log("WitheredUnit unit executed action"); 
        }
    }
}

