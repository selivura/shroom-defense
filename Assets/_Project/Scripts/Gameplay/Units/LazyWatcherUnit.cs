using ShroomDefense.Gameplay;
using UnityEngine;

namespace ShroomDefense
{
    public class LazyWatcherUnit : Unit
    {
        private void Start() //баф впередистоящего 
        {
            int frontX = Position.x + 1;
            int currentY = Position.y;
            var rowUnits = _battleMap.Rows[currentY].Units;
            if (rowUnits.TryGetValue(frontX, out Unit targetUnit))
            {
                if (targetUnit.Position.x < 0 && targetUnit.CurrentHP > 0)
                {
                    targetUnit.BuffAttack(2);
                    targetUnit.BuffMaxHP(2);
                }
            }
        }
        public override void ExecuteAction()
        {
            Debug.Log("LazyWatcherUnit action"); 
        }
    }
}

