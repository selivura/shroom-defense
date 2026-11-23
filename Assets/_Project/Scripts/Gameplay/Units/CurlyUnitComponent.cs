using NaughtyAttributes;
using System.Linq;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/CurlyUnit")]
    public class CurlyUnitComponent : UnitComponent
    {
        private BattleMap _battleMap;
        [SerializeField] [Required] private StatType _attackStatType;
        [SerializeField] [Required] private StatType _maxHPStatType;

        [InfoBox("Stat Value will be overriden by the component")]
        [SerializeField] private StatModifier _attackModifier;

        [InfoBox("Stat Value will be overriden by the component")]
        [SerializeField] private StatModifier _maxHPModifier;

        [Inject]
        public void Construct(BattleMap battleMap)
        {
            _battleMap = battleMap;
        }
        private void Start()
        {
            var allies = _battleMap.Rows
                .SelectMany(row => row.Units.Values)
                .Where(unit => unit != this && unit.Position.x < 0 && unit.CurrentHP > 0)
                .ToList();
            if (allies.Count == 0)
            {
                return;
            }

            float maxAttack = allies.Max(unit =>
            {
                if (unit.StatController.GetStat(_attackStatType, out var stat))
                {
                    return stat.Value;
                }
                return 0;
            });
            float maxHP = allies.Max(unit =>
            {
                if (unit.StatController.GetStat(_maxHPStatType, out var stat))
                {
                    return stat.Value;
                }
                return 0;
            });

            Stat ownAttackStat;
            Stat ownMaxHealthStat;
            if (unit.StatController.GetStat(_attackStatType, out ownAttackStat))
            {
                _attackModifier
                    .WithValue(maxAttack - ownAttackStat.Value)
                    .WithSource(unit);

                ownAttackStat.AddModifier(_attackModifier);
            }
            if (unit.StatController.GetStat(_maxHPStatType, out ownMaxHealthStat))
            {
                _maxHPModifier
                    .WithValue(maxHP - ownMaxHealthStat.Value)
                    .WithSource(unit);

                ownMaxHealthStat.AddModifier(_maxHPModifier);
            } 
        }
    }
}

