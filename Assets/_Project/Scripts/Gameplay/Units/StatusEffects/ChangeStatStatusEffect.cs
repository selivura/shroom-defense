namespace ShroomDefense.Gameplay
{
    public class ChangeStatStatusEffect : StatusEffect
    {
        private StatType _statType;
        private StatModifier _mod;
        private Stat _stat;
        public ChangeStatStatusEffect(int duration, StatType statType, StatModifier modifier) : base(duration)
        {
            _statType = statType;
            _mod = modifier;
        }
        protected override void OnApply()
        {
            if(unit.StatController.Stats.TryGetValue(_statType, out _stat))
            {
                _stat.AddModifier(_mod);
            }
        }
        public override void OnExpire()
        {
            if(_stat != null)
            {
                _stat.RemoveModifier(_mod);
            }
        }
    }
}
