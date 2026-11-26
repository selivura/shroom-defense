using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/Victim")]
    public class VictimUnitComponent : UnitComponent
    {
        [SerializeField] private UnitComponent _effectToExecuteOnSuccess;
        private BattleMap _battleMap;
        [Inject]
        public void Construct(BattleMap battleMap) { 
            _battleMap = battleMap; 
        }
        public override async UniTask DoComponentAction()
        {
            Vector2Int victimPosition = new Vector2Int(unit.Position.x + 1, unit.Position.y);

            if (victimPosition.x >= 0)
            {
                return;
            }
            Unit targetUnit = unit.GetUnitAt(victimPosition);
            if(targetUnit == null)
            {
                return;
            }
            targetUnit.Instakill(new Damage(unit, 0));
            _battleMap.RegisterVictim();
            await _effectToExecuteOnSuccess.DoComponentAction();
        }
    }
}
