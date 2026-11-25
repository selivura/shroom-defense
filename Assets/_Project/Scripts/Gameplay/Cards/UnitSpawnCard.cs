using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public class UnitSpawnCard : Card
    {
        [field:SerializeField] public Unit Unit { get; private set; }
        [field: SerializeField] public FieldTargetSelectType _targetSelectType = FieldTargetSelectType.Player;
        protected UnitFactory _unitFactory;
        protected BattleMap _map;


        [Inject]
        public void Construct(UnitFactory factory, BattleMap map)
        {
            _unitFactory = factory;
            _map = map;
        }
        public override bool ExecuteAction(CardTargetData target)
        {
            if(_targetSelectType == FieldTargetSelectType.Enemy)
            {
                if(target.MapPostion.x < 0)
                {
                    return false;
                }
            }
            if (_targetSelectType == FieldTargetSelectType.Player)
            {
                if (target.MapPostion.x >= 0)
                {
                    return false;
                }
            }
            if (_map.IsOccupied(target.MapPostion))
                return false;
            _map.PlaceUnitAt(target.MapPostion, _unitFactory.Create(Unit));
            return true;
        }
    }
}
