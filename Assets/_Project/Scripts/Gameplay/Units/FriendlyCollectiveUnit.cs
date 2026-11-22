using ShroomDefense.Gameplay;
using System.Linq;
using UnityEngine;

namespace ShroomDefense
{
    public class FriendlyCollectiveUnit : Unit
    {
        public override void ExecuteAction()
        {
            Debug.Log("FriendlyCollectiveUnit executed action"); 
        }

        private void Start() //старт так как эффект только при розыгрыше
        {
            int otherMushroomsCount = 0;
            foreach (var row in _battleMap.Rows) //ищем кол-во грибов на нашем поле
            {
                otherMushroomsCount += row.Units.Values.Count(unit => unit != this && unit.Position.x < 0);
            }

            if (otherMushroomsCount > 0) //баф на найденное количество
            {
                BuffAttack(otherMushroomsCount);
                BuffMaxHP(otherMushroomsCount);
            }
        }
    }
}