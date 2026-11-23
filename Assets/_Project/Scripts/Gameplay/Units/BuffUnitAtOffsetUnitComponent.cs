using ShroomDefense.Gameplay;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace ShroomDefense
{
    [AddComponentMenu("UnitComponents/BuffUnitAtOffset")]
    public class BuffUnitAtOffsetUnitComponent : UnitComponent
    {
        private BattleMap _battleMap;
        [SerializeField] private List<StatType> _buffedStatTypes;

        [Tooltip("Задавать с перспективы игрока, если компонент будет висеть на враге, X будет инвертирован")]
        [SerializeField] private Vector2Int _relativeOffset = new(1, 0);

        [SerializeField] private StatModifier _statModifier = new(2, StatModType.Flat);

        [Inject]
        public void Construct(BattleMap battleMap)
        {
            _battleMap = battleMap;
        }
        private void Start() //баф стоящего на позиции относительно себя юнита
        {
            Vector2Int buffPos = unit.Position;
            if(unit.Position.x < 0)
            {
                buffPos += _relativeOffset;
            }
            else
            {
                buffPos += new Vector2Int(-_relativeOffset.x, _relativeOffset.y);
            }
            var rowUnits = _battleMap.Rows[buffPos.y].Units;
            if (!rowUnits.TryGetValue(buffPos.x, out Unit targetUnit))
            {
                return;
            }
            if (targetUnit.Position.x >= 0 || targetUnit.CurrentHP <= 0)
            {
                return;
            }
            _statModifier.WithSource(unit);
            targetUnit.StatController.AddModifierToAllStatsOfTypes(_buffedStatTypes, _statModifier);
        }
    }
}

