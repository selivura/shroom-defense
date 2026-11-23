using NaughtyAttributes;
using System.Linq;
using UnityEngine;

namespace ShroomDefense.Gameplay
{
    [AddComponentMenu("UnitComponents/FriendlyCollectiveOnStart")]
    public class FriendlyCollectiveUnitComponent : UnitComponent
    {
        [SerializeField][Required] private StatType _attackStatType;
        [SerializeField][Required] private StatType _maxHPStatType;

        [InfoBox("Stat Value will be overriden by the component")]
        [SerializeField] private StatModifier _attackModifier = new(0, StatModType.Flat);

        [InfoBox("Stat Value will be overriden by the component")]
        [SerializeField] private StatModifier _maxHPModifier = new(0, StatModType.Flat);

        protected override void OnComponentInit()
        {
            int otherMushroomsCount = unit.FindAllUnitsOnMyTeam().Count(ally => ally != unit);

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