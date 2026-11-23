using System.Collections.Generic;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/DebuffAllEnemyUnitsOnAction")]
    public class DebuffAllEnemyUnitsUnitComponent : UnitComponent
    {
        [SerializeField] private List<StatType> _buffedStatTypes;

        [SerializeField] private StatModifier _statModifier = new(-1, StatModType.Flat);
        public override void OnExecuteAction()
        {
            var enemies = unit.FindAllUnitsOnEnemyField();
            _statModifier.WithSource(unit);
            foreach (var enemy in enemies)
            {
                enemy.StatController.AddModifierToAllStatsOfTypes(_buffedStatTypes, _statModifier);
            }
            Debug.Log($"{name} debuffed all enemy units (ExecuteAction)");
        }
    }
}
          