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

        /// <summary>
        /// ОСТОРОЖНО с аргументом modifier, потому что этот StatusEffect МОЖЕТ ИЗМЕНЯТЬ ЕГО ЗНАЧЕНИЯ!
        /// Если нужно сохранить значения оригинала, лучше сделать копию с помощью <see cref="StatModifier.StatModifier(StatModifier)"/>
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="statTypes"></param>
        /// <param name="modifier"></param>
        /// <param name="newInstanceBehaviour"></param>
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
                    StackValue(newInstance as ChangeStatsStatusEffect);
                    return true;
                case NewInstanceBehaviour.RefreshDuration:
                    DurationTurns = newInstance.DurationTurns;
                    return true;
                case NewInstanceBehaviour.StackValueAndRefreshDuration:
                    DurationTurns = newInstance.DurationTurns;
                    StackValue(newInstance as ChangeStatsStatusEffect);
                    return true;
                default:
                    return false;
            }
        }

        private void StackValue(ChangeStatsStatusEffect newChangeStatsEffect)
        {
            _mod.Value += newChangeStatsEffect._mod.Value;
        }
    }
}
