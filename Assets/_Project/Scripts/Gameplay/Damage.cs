namespace ShroomDefense.Gameplay
{
    public class Damage
    {
        public readonly IDamageSource Source;
        public readonly int Amount;

        public Damage(IDamageSource source, int amount)
        {
            Source = source;
            Amount = amount;
        }
    }
}
