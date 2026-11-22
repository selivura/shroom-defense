using ShroomDefense.Gameplay;
using System.Linq;
using UnityEngine;

namespace ShroomDefense
{
    public class GovorushkaUnit : Unit
    {
        public override void ExecuteAction()
        {
            foreach (var row in _battleMap.Rows) //делаем список наших живых грибов и хилим их на 1
            {
                var alliesInRow = row.Units.Values.Where(unit => unit != this && unit.Position.x < 0 && unit.CurrentHP > 0).ToList();
                foreach (var ally in alliesInRow)
                {
                    ally.Heal(1);
                }
            }
            Debug.Log("GovorushkaUnit unit executed action"); 
        }
    }
}
