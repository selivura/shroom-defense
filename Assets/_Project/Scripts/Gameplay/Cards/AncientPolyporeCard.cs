using ShroomDefense.Gameplay;
using UnityEngine;

namespace ShroomDefense
{
    public class AncientPolyporeCard : UnitSpawnCard
    {
        public override int GetCurrentCost()
        {
            return Mathf.Max(0, ManaCost - _map.DeadEnemyUnitsCount);
        }
    }
}