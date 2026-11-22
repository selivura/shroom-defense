using ShroomDefense.Gameplay;
using System.Linq;
using UnityEngine;

namespace ShroomDefense
{
    public class SpiderUnit : EnemyUnit
    {
        public override void ExecuteAction()
        {
            foreach (var row in _battleMap.Rows) //делаем список наших живых грибов и дебафаем на 1
            {
                var alliesInRow = row.Units.Values.Where(unit => unit != this && unit.Position.x < 0 && unit.CurrentHP > 0).ToList();
                foreach (var ally in alliesInRow)
                {
                    ally.DebuffAttack(1);
                }
            }
            Debug.Log("SpiderUnit unit executed action");
        }
    }
}
