using System.Collections.Generic;

namespace ShroomDefense.Gameplay
{
    public class ChangeStatsStatusEffect : StatusEffect
    {
        public enum NewInstanceBehaviour
        {
            Nothing,
            StackDuration,
            StackValue,
            RefreshDuration,
            StackValueAndRefreshDuration,
        }
        private List<StatType> _statTypes;
        private StatModifier _mod;
        private NewInstanceBehaviour _newInstanceBehaviour;
        public ChangeStatsStatusEffect(int duration, List<StatType> statTypes, StatModifier modifier, NewInstanceBehaviour newInstanceBehaviour = NewInstanceBehaviour.Nothing) : base(duration)
        {
            _statTypes = statTypes;
            _mod = modifier;
            _newInstanceBehaviour = newInstanceBehaviour;
        }
        protected override void OnApply()
        {
            unit.StatController.AddModifierToAllStatsOfTypes(_statTypes, _mod);
        }
        public override void OnExpire()
        {
            unit.StatController.RemoveModifierFromAllStatsOfTypes(_statTypes, _mod);
        }
        public override bool HandleAnotherInstance(StatusEffect newInstance)
        {
            switch(_newInstanceBehaviour)
            {
                case NewInstanceBehaviour.Nothing:
                    return false;
                case NewInstanceBehaviour.StackDuration:
                    DurationTurns += newInstance.DurationTurns;
                    return true;
                case NewInstanceBehaviour.StackValue:
                    if (newInstance is ChangeStatsStatusEffect newChangeStatsEffect)
                    {
                        _mod.Value += newChangeStatsEffect._mod.Value;
                        return true;
                    }
                    return false;
                case NewInstanceBehaviour.RefreshDuration:
                    DurationTurns = newInstance.DurationTurns;
                    return true;
                case NewInstanceBehaviour.StackValueAndRefreshDuration:
                    DurationTurns = newInstance.DurationTurns;
                    if (newInstance is ChangeStatsStatusEffect newChangeStatsEffect2)
                    {
                        _mod.Value += newChangeStatsEffect2._mod.Value;
                    }
                    return true;
                default:
                    return false;
            }
        }
    }
}
