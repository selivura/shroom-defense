using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/Advance")]
    public class AdvanceUnitComponent : UnitComponent
    {
        private BattleMap _battleMap;

        [Inject]
        public void Construct(BattleMap battleMap)
        {
            _battleMap = battleMap;
        }
        public void Advance()
        {
            int direction = 1;
            if (unit.Position.x > 0)
            {
                direction = -1;
            }
            var movePosition = unit.Position + new Vector2Int(direction, 0);
            if (_battleMap.ValidatePosition(movePosition) && !_battleMap.IsOccupied(movePosition))
            {
                unit.Move(movePosition);
            }
        }
    }
}
