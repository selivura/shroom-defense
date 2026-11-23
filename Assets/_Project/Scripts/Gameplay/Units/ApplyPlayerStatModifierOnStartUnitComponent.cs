using ShroomDefense.Gameplay;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace ShroomDefense
{
    [AddComponentMenu("UnitComponents/ApplyPlayerStatModifierOnStart")]
    public class ApplyPlayerStatModifierOnStartUnitComponent : UnitComponent
    {
        private PlayerController _player;
        [SerializeField] private List<StatType> _buffedStatTypes;
        [SerializeField] private StatModifier _statModifier = new(1, StatModType.Flat);

        [Inject]
        public void Construct(PlayerController player)
        {
            _player = player;
        }

        private void Start()
        {
            _statModifier.WithSource(unit);
            _player.StatController.AddModifierToAllStatsOfTypes(_buffedStatTypes, _statModifier);
        }
    }
}