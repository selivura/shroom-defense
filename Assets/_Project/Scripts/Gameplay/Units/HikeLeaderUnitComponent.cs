using ShroomDefense.Gameplay;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/HikeLeader")]
    public class HikeLeaderUnitComponent : UnitComponent
    {
        private int _currentBonus = 0;
        [SerializeField] private StatType _attackStatType;
        [SerializeField] private StatType _maxHPStatType;

        [InfoBox("Stat Value is overriden by component")]
        [SerializeField] private StatModifier _attackModifier;

        [InfoBox("Stat Value is overriden by component")]
        [SerializeField] private StatModifier _maxHealthModifier;

        private void Start()
        {
            if (unit.StatController.GetStat(_attackStatType, out var ownAttackStat))
            {
                _attackModifier.WithValue(0).WithSource(unit);
                ownAttackStat.AddModifier(_attackModifier);
            }
            if (unit.StatController.GetStat(_maxHPStatType, out var ownMaxHealthStat))
            {
                _maxHealthModifier.WithValue(0).WithSource(unit);
                ownMaxHealthStat.AddModifier(_maxHealthModifier);
            }
        }
        public override void OnExecuteAction()
        {
            int otherForagersCount = unit.FindAllUnitsOnMyTeam()
                .Count(unit => unit != this && unit.CurrentHP > 0);

            _attackModifier.Value = otherForagersCount - _currentBonus;
            _currentBonus = otherForagersCount;

            Debug.Log($"{unit.name} HikeLeader buffed his attack!");
        }
    }
}