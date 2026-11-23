using ShroomDefense.Gameplay;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace ShroomDefense
{
    [AddComponentMenu("UnitComponents/DebuffAllEnemyUnits")]
    public class DebuffAllEnemyUnitsUnitComponent : UnitComponent
    {
        private BattleMap _battleMap;
        [SerializeField] private List<StatType> _buffedStatTypes;

        [SerializeField] private StatModifier _statModifier = new(-1, StatModType.Flat);
        [Inject]
        public void Construct(BattleMap battleMap)
        {
            _battleMap = battleMap;
        }
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
          