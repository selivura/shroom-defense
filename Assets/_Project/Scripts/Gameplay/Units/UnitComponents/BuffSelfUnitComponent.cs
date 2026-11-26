using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/BuffSelf")]
    public class BuffSelfUnitComponent : UnitComponent
    {
        [SerializeField] private List<StatType> _buffedStatTypes;

        [SerializeField] private StatModifier _statModifier = new(2, StatModType.Flat);

        public override async UniTask DoComponentAction()
        {
            unit.StatController.AddModifierToAllStatsOfTypes(_buffedStatTypes, _statModifier);
        }
    }
}

