using Cysharp.Threading.Tasks;
using ShroomDefense.Gameplay;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/ApplyPlayerStatModifier")]
    public class ApplyPlayerStatModifierUnitComponent : UnitComponent
    {
        private PlayerController _player;
        [SerializeField] private List<StatType> _buffedStatTypes;
        [SerializeField] private StatModifier _statModifier = new(1, StatModType.Flat);

        [Inject]
        public void Construct(PlayerController player)
        {
            _player = player;
        }

        public override async UniTask DoComponentAction()
        {
            _statModifier.WithSource(unit);
            _player.StatController.AddModifierToAllStatsOfTypes(_buffedStatTypes, _statModifier);
            Debug.Log($"{unit.name} applied stat modifier to player!");
        }
    }
}