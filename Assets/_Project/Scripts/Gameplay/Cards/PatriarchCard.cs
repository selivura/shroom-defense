using ShroomDefense.Gameplay;
using UnityEngine;
using VContainer;

namespace ShroomDefense
{

    public class PatriarchCard : UnitSpawnCard
    {
        public override int GetCurrentCost()
        {
            return Mathf.Max(0, ManaCost - _map.DeadPlayerUnitsCount);
        }
    }
}