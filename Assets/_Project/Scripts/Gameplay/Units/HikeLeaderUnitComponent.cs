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
        [SerializeField] private StatType _attackStatType;

        [InfoBox("Stat Value is overriden by component")]
        [SerializeField] private StatModifier _attackModifier;

        protected override void OnComponentInit()
        {
            if (unit.StatController.GetStat(_attackStatType, out var ownAttackStat))
            {
                _attackModifier.WithValue(0).WithSource(unit);
                ownAttackStat.AddModifier(_attackModifier);
            }
        }
        public override async UniTask OnExecuteAction()
        {
            int otherForagersCount = unit.FindAllUnitsOnMyTeam()
                .Count(ally => ally != unit && ally.CurrentHP > 0);

            int attackBonus = otherForagersCount;
            _attackModifier.Value = attackBonus;


            Debug.Log($"{unit.name} HikeLeader buffed its attack!");
        }
    }
}