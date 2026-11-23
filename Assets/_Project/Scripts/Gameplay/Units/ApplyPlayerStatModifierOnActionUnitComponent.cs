using ShroomDefense.Gameplay;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace ShroomDefense
{
    [AddComponentMenu("UnitComponents/ApplyPlayerStatModifierOnAction")]
    public class ApplyPlayerStatModifierOnActionUnitComponent : UnitComponent
    {
        private PlayerController _player;
        [SerializeField] private List<StatType> _buffedStatTypes;
        [SerializeField] private StatModifier _statModifier = new(1, StatModType.Flat);

        [Inject]
        public void Construct(PlayerController player)
        {
            _player = player;
        }

        public override void OnExecuteAction()
        {
            _statModifier.WithSource(unit);
            _player.StatController.AddModifierToAllStatsOfTypes(_buffedStatTypes, _statModifier);
            Debug.Log($"{unit.name} applied stat modifier to player! (ExecuteAction)");
        }
    }
}