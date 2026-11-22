using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public class UnitSpawnCard : Card
    {
        [field:SerializeField] public Unit Unit { get; private set; }
        private UnitFactory _unitFactory;
        private BattleMap _map;


        [Inject]
        public void Construct(UnitFactory factory, BattleMap map)
        {
            _unitFactory = factory;
            _map = map;
        }
        public override void ExecuteAction(CardTargetData target)
        {
            _map.PlaceUnitAt(target.MapPostion, _unitFactory.Create(Unit));
        }
    }
}
