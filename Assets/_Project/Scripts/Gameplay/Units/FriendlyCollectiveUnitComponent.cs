using NaughtyAttributes;
using ShroomDefense.Gameplay;
using System.Linq;
using UnityEngine;
using VContainer;

namespace ShroomDefense
{
    [AddComponentMenu("UnitComponents/FriendlyCollective")]
    public class FriendlyCollectiveUnitComponent : UnitComponent
    {
        private BattleMap _battleMap;
        [SerializeField][Required] private StatType _attackStatType;
        [SerializeField][Required] private StatType _maxHPStatType;

        [InfoBox("Stat Value will be overriden by the component")]
        [SerializeField] private StatModifier _attackModifier = new(0, StatModType.Flat);

        [InfoBox("Stat Value will be overriden by the component")]
        [SerializeField] private StatModifier _maxHPModifier = new(0, StatModType.Flat);

        [Inject]
        public void Construct(BattleMap battleMap)
        {
            _battleMap = battleMap;
        }
        private void Start() //старт так как эффект только при розыгрыше
        {
            int otherMushroomsCount = unit.FindAllUnitsOnMyTeam().Count(unit => unit != this);

            if (otherMushroomsCount <= 0) //баф на найденное количество
            {
                return;
            }
            if (unit.StatController.GetStat(_attackStatType, out var ownAttackStat))
            {
                _attackModifier.WithValue(otherMushroomsCount).WithSource(unit);
                ownAttackStat.AddModifier(_attackModifier);
            }
            if (unit.StatController.GetStat(_maxHPStatType, out var ownMaxHealthStat))
            {
                _maxHPModifier.WithValue(otherMushroomsCount).WithSource(unit);
                ownMaxHealthStat.AddModifier(_maxHPModifier);
            }
        }
    }
}