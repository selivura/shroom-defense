using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/DebuffAllEnemyUnitsOnAction")]
    public class DebuffAllEnemyUnitsUnitComponent : UnitComponent
    {
        [SerializeField] private List<StatType> _buffedStatTypes;
        [SerializeField] private int _debuffDuration = 1;

        [SerializeField] private StatModifier _statModifier = new(-1, StatModType.Flat);
        public override async UniTask OnExecuteAction()
        {
            var enemies = unit.FindAllUnitsOnEnemyField();
            _statModifier.WithSource(unit);
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<StatusEffectsController>()
                    .AddEffect(
                    new ChangeStatsStatusEffect(
                        _debuffDuration, 
                        _buffedStatTypes,
                        new StatModifier(_statModifier), 
                        ChangeStatsStatusEffect.NewInstanceBehaviour.StackValueAndRefreshDuration));
            }
            Debug.Log($"{name} debuffed all enemy units (ExecuteAction)");
        }
    }
}
          