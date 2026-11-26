using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using ShroomDefense.Gameplay;
using System.Linq;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/HikeLeaderAnd")]
    public class HikeLeaderUnitComponent : UnitComponent
    {
        [SerializeField] private StatType _attackStatType;

        [InfoBox("Stat Value is overriden by component")]
        [SerializeField] private StatModifier _attackModifier;
        private bool _isInitialized = false;

        public override async UniTask DoComponentAction()
        {
            if (!_isInitialized)
            {
                if (unit.StatController.GetStat(_attackStatType, out var ownAttackStat))
                {
                    _attackModifier.WithValue(0).WithSource(unit);
                    ownAttackStat.AddModifier(_attackModifier);
                }
                _isInitialized = true;
            }

            int otherForagersCount = unit.FindAllUnitsOnMyTeam()
                .Count(ally => ally != unit && ally.CurrentHP > 0);

            if (otherForagersCount <= 0)
            {
                return;
            }

            _attackModifier.Value += otherForagersCount;


            Debug.Log($"{unit.name} HikeLeader buffed its attack!");
        }
    }
}