using ShroomDefense.Gameplay;
using UnityEngine;
using System.Linq;

namespace ShroomDefense
{
    public class HikeLeaderUnit : EnemyUnit
    {
        private int _currentBonus = 0;
        public override void ExecuteAction()
        {
            int otherForagersCount = _battleMap.Rows
                .SelectMany(row => row.Units.Values)
                .Count(unit => unit != this && unit.Position.x > 0 && unit.CurrentHP > 0);

            int difference = otherForagersCount - _currentBonus;

            if (difference > 0)
            {
                BuffAttack(difference);
            }
            else if (difference < 0)
            {
                DebuffAttack(-difference); 
            }

            _currentBonus = otherForagersCount;

            Debug.Log($"HikeLeader attacks with bonus {_currentBonus}");
        }
    }
}