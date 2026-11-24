using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using ShroomDefense.Gameplay;
using System.Linq;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/HikeLeaderOnStartAndOnAction")]
    public class HikeLeaderUnitComponent : UnitComponent
    {
        private int _currentAttackBonus = 0;
        private int _currentHealthBonus = 0;
        [SerializeField] private StatType _attackStatType;
        [SerializeField] private StatType _maxHPStatType;

        [InfoBox("Stat Value is overriden by component")]
        [SerializeField] private StatModifier _attackModifier;

        [InfoBox("Stat Value is overriden by component")]
        [SerializeField] private StatModifier _maxHealthModifier;

        protected override void OnComponentInit()
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
        public override async UniTask OnExecuteAction()
        {
            int otherForagersCount = unit.FindAllUnitsOnMyTeam()
                .Count(ally => ally != unit && ally.CurrentHP > 0);

            int attackBonus = otherForagersCount - _currentAttackBonus;
            int healthBonus = otherForagersCount - _currentHealthBonus;
            _attackModifier.Value = attackBonus;
            _maxHealthModifier.Value = healthBonus;
            _currentAttackBonus = otherForagersCount;
            _currentHealthBonus = otherForagersCount;

            Debug.Log($"{unit.name} HikeLeader buffed its attack and health!");
        }
    }
}