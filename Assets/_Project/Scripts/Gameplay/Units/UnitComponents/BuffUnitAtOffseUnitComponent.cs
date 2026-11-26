using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/BuffUnitAtOffset")]
    public class BuffUnitAtOffseUnitComponent : UnitComponent
    {
        [SerializeField] private List<StatType> _buffedStatTypes;

        [Tooltip("Задавать с перспективы игрока, если компонент будет висеть на враге, X будет инвертирован")]
        [SerializeField] private Vector2Int _relativeOffset = new(1, 0);

        [SerializeField] private StatModifier _statModifier = new(2, StatModType.Flat);

        public override async UniTask DoComponentAction()
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
            Unit targetUnit = unit.GetUnitAt(buffPos);

            if (targetUnit == null || targetUnit.CurrentHP <= 0)
            {
                return;
            }
            if ((targetUnit.Position.x < 0) != (unit.Position.x < 0))
            {
                return;
            }
            _statModifier.WithSource(unit);
            targetUnit.StatController.AddModifierToAllStatsOfTypes(_buffedStatTypes, _statModifier);
        }
    }
}

